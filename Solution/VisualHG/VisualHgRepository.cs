﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HgLib;
using Microsoft.VisualStudio.Shell.Interop;

namespace VisualHg
{
    public class VisualHgRepository : HgUpdatingRepository
    {
        private bool _solutionBuilding;
        private List<string> _solutionFiles;


        public VisualHgRepository()
        {
            _solutionFiles = new List<string>();
        }


        public void SolutionBuildStarted()
        {
            _solutionBuilding = true;
        }

        public void SolutionBuildEnded()
        {
            _solutionBuilding = false;
        }


        public string[] AddSolutionFiles(IVsHierarchy hierarchy)
        {
            var project = hierarchy as IVsSccProject2;
            var files = SccProvider.GetProjectFiles(project);

            lock (_solutionFiles)
            {
                foreach (var fileName in files)
                {
                    _solutionFiles.Add(fileName.ToLower());
                }
            }

            return files;
        }

        public void ClearSolutionFiles()
        {
            lock (_solutionFiles)
            {
                _solutionFiles.Clear();
            }
        }

        public void RemoveSolutionFiles(IVsHierarchy hierarchy)
        {
            if (_solutionFiles.Count > 0)
            {
                var project = hierarchy as IVsSccProject2;
                var files = SccProvider.GetProjectFiles(project);

                RemoveSolutionFiles(files);
            }
        }

        private void RemoveSolutionFiles(string[] fileNames)
        {
            lock (_solutionFiles)
            {
                foreach (var fileName in fileNames)
                {
                    _solutionFiles.Remove(fileName.ToLower());
                }
            }
        }

        
        public void UpdateSolution(IVsSolution solution)
        {
            foreach (var directory in GetProjectDirectories(solution))
            {
                UpdateRootDirectory(directory);
            }
        }

        public void UpdateProject(IVsSccProject2 project)
        {
            UpdateRootDirectory(SccProvider.GetDirectoryName((IVsHierarchy)project));
        }


        private void UpdateRootDirectory(string directory)
        {
            Enqueue(new UpdateRootStatusHgCommand(directory));
        }

        private static string[] GetProjectDirectories(IVsSolution solution)
        {
            return GetProjectFiles(solution)
                .Where(x => !String.IsNullOrEmpty(x))
                .Select(x => Path.GetDirectoryName(x) + '\\')
                .ToArray();
        }

        private static string[] GetProjectFiles(IVsSolution solution)
        {
            uint numberOfProjects;
            solution.GetProjectFilesInSolution(0, 0, null, out numberOfProjects);

            var projectFiles = new string[numberOfProjects];
            solution.GetProjectFilesInSolution(0, numberOfProjects, projectFiles, out numberOfProjects);
            
            return projectFiles;
        }


        protected override void Update()
        {
            if (!_solutionBuilding)
            {
                base.Update();
            }
        }

        protected override void OnStatusChanged(string[] dirtyFiles)
        {
            bool statusChanged = false;

            lock (_solutionFiles)
            {
                statusChanged = dirtyFiles.Any(x => _solutionFiles.Contains(x.ToLower()));
            }

            if (statusChanged)
            {
                base.OnStatusChanged(dirtyFiles);
            }
        }
    }
}

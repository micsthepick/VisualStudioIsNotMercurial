﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace HgLib
{
    public static class TortoiseHg
    {
        public static string Version { get; }


        static TortoiseHg()
        {
            Version = ProcessLauncher.RunTortoiseHg("version", "").FirstOrDefault();
        }


        public static Process ShowCommitWindow(string directory)
        {
            return Start("commit", directory);
        }

        public static Process ShowWorkbenchWindow(string directory)
        {
            return Start("workbench", directory);
        }

        public static Process ShowStatusWindow(string directory)
        {
            return Start("status", directory);
        }

        public static Process ShowSynchronizeWindow(string directory)
        {
            return Start("sync", directory);
        }

        public static Process ShowUpdateWindow(string directory)
        {
            return Start("update", directory);
        }

        public static Process ShowCreateRepositoryWindow(string directory)
        {
            return Start("init", directory);
        }

        public static Process ShowRepositorySettingsWindow(string directory)
        {
            return Start("repoconfig", directory);
        }

        public static Process ShowUserSettingsWindow(string directory)
        {
            return Start("userconfig", directory);
        }

        public static Process ShowShelveWindow(string directory)
        {
            return Start("shelve", directory);
        }

        public static void ShowAddWindow(string[] files)
        {
            StartForEachRoot("add ", files);
        }

        public static void ShowCommitWindow(string[] files)
        {
            StartForEachRoot("commit ", files);
        }

        public static void ShowRevertWindow(string[] files)
        {
            StartForEachRoot("revert", files);
        }

        public static void ShowHistoryWindow(string fileName)
        {
            var root = HgPath.FindRepositoryRoot(fileName);

            if (!string.IsNullOrEmpty(root))
            {
                fileName = fileName.Substring(root.Length + 1);

                Start($"history \"{fileName}\"", root);
            }
        }


        private static Process Start(string args, string workingDirectory)
        {
            while (!Directory.Exists(workingDirectory) && workingDirectory.Length > 0)
            {
                workingDirectory = workingDirectory.Substring(0, workingDirectory.LastIndexOf('\\'));
            }

            return ProcessLauncher.StartTortoiseHg(args, workingDirectory);
        }


        private static void StartForEachRoot(string command, string[] files)
        {
            var commandWithOptions = string.Concat("--nofork ", command);

            foreach (var group in files.GroupBy(HgPath.FindRepositoryRoot))
            {
                if (string.IsNullOrEmpty(group.Key))
                    continue;

                Start(commandWithOptions, group.Key, group);
            }
        }

        private static void Start(string command, string root, IEnumerable<string> files)
        {
            var listFile = HgPath.GetRandomTemporaryFileName();
            var listCommand = $"{command} --listfile \"{listFile}\"";

            CreateListFile(listFile, files);

            var process = Start(listCommand, root);

            try
            {
                process.WaitForExit();
            }
            catch (InvalidOperationException)
            {
            }

            DeleteListFile(listFile);
        }

        private static void CreateListFile(string listFileName, IEnumerable<string> files)
        {
            using (var writer = File.CreateText(listFileName))
            {
                foreach (var fileName in files) 
                    writer.WriteLine(fileName);
            }
        }

        private static void DeleteListFile(string groupListFile)
        {
            try
            {
                File.Delete(groupListFile);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }
    }
}
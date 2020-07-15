﻿using System;
using System.IO;

namespace HgLib
{
    public class HgFileInfo
    {
        private bool _exists;
        private DateTime _lastWriteTime;
        private readonly HgFileStatus _status;


        internal HgFileInfo? OriginalFile { get; set; }

        public string Root { get; }

        public string RootName { get; }

        public string Name { get; }

        public string ShortName { get; }

        public string FullName { get; }

        public HgFileStatus Status
        {
            get
            {
                if (OriginalFile != null)
                    return OriginalFile._exists ? HgFileStatus.Copied : HgFileStatus.Renamed;

                return _status;
            }
        }

        public string OriginalName => OriginalFile != null ? OriginalFile.Name : Name;

        public string OriginalFullName => OriginalFile != null ? OriginalFile.FullName : FullName;

        public bool HasChanged
        {
            get
            {
                if (StatusMatches(HgFileStatus.NotAdded))
                    return false;

                try
                {
                    var file = new FileInfo(FullName);

                    return _exists != file.Exists || file.LastWriteTime != _lastWriteTime;
                }
                catch
                {
                    return false;
                }
            }
        }


        public HgFileInfo(string root, string name, char status)
        {
            Root = root;
            Name = name;
            _status = Hg.ConvertToStatus(status);
            RootName = new DirectoryInfo(root).Name;
            ShortName = Path.GetFileName(name);
            FullName = Path.Combine(root, name);

            if (Status != HgFileStatus.None && !StatusMatches(HgFileStatus.Deleted)) 
                InitializeFileProperties(FullName);
        }

        private void InitializeFileProperties(string fileName)
        {
            try
            {
                var file = new FileInfo(fileName);

                if (file.Exists)
                {
                    _exists = true;
                    _lastWriteTime = file.LastWriteTime;
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }


        public bool StatusMatches(HgFileStatus pattern)
        {
            return Status == pattern || (Status & pattern) > 0;
        }

        public static HgFileInfo FromHgOutput(string root, string output)
        {
            return new HgFileInfo(root, output.Substring(2), output[0]);
        }

        public override string ToString()
        {
            return string.Concat(Status, ' ', Name);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileBrowsing.Models
{
    public class FolderModels
    {
        public string PathName { get; set; }
        public DirectoryInfo CurrentPath { get; set; }
        public string FullPath { get; set; }
        public string ParentPath { get; set; }
    }
}
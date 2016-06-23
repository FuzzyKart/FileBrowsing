using FileBrowsing.Models;
using FileBrowsing.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileBrowsing.ViewModels
{
    public class FileFolderViewModel
    {
        public FileCountModels GetCount { get; set; }
        private List<FolderModels> _getFolders = null;
        private List<FileModels> _getFiles = null;
        public List<FolderModels> GetFolders { get { return _getFolders; } }
        public List<FileModels> GetFiles { get { return _getFiles; } }
        public string GetCurrentPath { get; set; }
        public string GetParentPath { get; set; }

        public void FileFolderInitializer(List<FolderModels> folders, List<FileModels> files)
        {
            string getParentPath = folders.Select(root => root.ParentPath).FirstOrDefault();
            try {
                GetCurrentPath = folders.Select(root => root.CurrentPath).FirstOrDefault().ToString();
                if (getParentPath == GetCurrentPath)
                    GetParentPath = "none";
                else
                    GetParentPath = getParentPath;
            }
            catch(NullReferenceException ex)
            {
                ExceptionProcessing.ErrorWriting(ex.Message);
            }

            _getFolders = folders;
            _getFiles = files.Where(path => path.PathName == GetCurrentPath).ToList();

            GetCount = FileCountModels.GetCount();
        }

        public void LogicalDrivesInitializer(string driveName, List<FileModels> files)
        {
            GetCurrentPath = "My computer";
            GetParentPath = "none";

            _getFolders.Add(new FolderModels
            {
                CurrentPath = new DirectoryInfo("My computer"),
                PathName = driveName
            });
            _getFiles = files.Where(path => path.PathName == driveName).ToList();
            GetCount = FileCountModels.GetCount();

            /*FileCountModels count = GetFilesCount(files);
            GetCount.FileLenghtLess += count.FileLenghtLess;
            GetCount.FileLenghtMiddle += count.FileLenghtMiddle;
            GetCount.FileLenghtMore += count.FileLenghtMore;
            */
        }
    }
}
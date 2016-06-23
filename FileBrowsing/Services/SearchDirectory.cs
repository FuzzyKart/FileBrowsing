using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FileBrowsing.Models;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;

namespace FileBrowsing.Services
{
    class SearchDirectory
    {
        private int less = 0;
        private int middle = 0;
        private int more = 0;
        private bool isRootDir = false;
        private int isTopDirectory = 0;
        private static string parentPath = null;
        private DirectoryInfo currentPath = null;
        private HashSet<string> distancePath = new HashSet<string>();
        private HashSet<string> exceptionPath = new HashSet<string>();
        private static List<FileModels> listOfFiles = new List<FileModels>();
        private static List<FolderModels> listOfDirectory = new List<FolderModels>();

        public SearchDirectory()
        {
            listOfDirectory.Clear();
            listOfFiles.Clear();
            distancePath.Clear();
        }
        private DirectoryInfo GetCurrentPath(DirectoryInfo Path, string isLastFolder)
        {
            if (isLastFolder == null)
                return new DirectoryInfo(Path.FullName);
            else
                return (Path.Parent == null) ? new DirectoryInfo(Path.FullName) :
                    new DirectoryInfo(Path.Parent.FullName);
        }
        private string GetParentPath(DirectoryInfo Path, DirectoryInfo currentPath, string isLastFolder, bool isException)
        {
            try
            {
                if (Path.Parent == null)
                    parentPath = Path.Root.ToString();
                if (isLastFolder != null)
                {
                    if (Path.Parent.FullName != Path.FullName &&
                        Path.Parent.FullName != currentPath.FullName)
                        parentPath = (Path.Parent.Root.Name == Path.Name) ? Path.Root.ToString() : Path.Parent.FullName;
                    if (Path.Parent.Parent != null)
                        parentPath = (Path.Parent.Root.Name == Path.Name) ? Path.Root.ToString() : Path.Parent.Parent.FullName;
                    else
                        parentPath = (Path.Parent.Root.Name == Path.Name) ? Path.Root.ToString() : Path.Parent.FullName;
                }
                else
                    parentPath = (Path.Parent.Root.Name == Path.Name) ? Path.Root.ToString() : Path.Parent.FullName;
                if (isException)
                    SearchDir(currentPath);
            }
            catch (Exception ex)
            {
                ExceptionProcessing.ErrorWriting(ex.Message);
            }
            return parentPath;
        }
        private DirectoryInfo[] CheckDirectory(DirectoryInfo Path)
        {
            DirectoryInfo[] directories = Path.GetDirectories();
            if (directories.Count() == 0)
                AddFolder(Path, null);
            return directories;
        }

        private void AddFolder(DirectoryInfo path, string folderOrMessage, bool isException = false)
        {
            currentPath = GetCurrentPath(path, folderOrMessage);
            listOfDirectory.Add(new FolderModels
            {
                PathName = folderOrMessage,
                CurrentPath = currentPath,
                FullPath = path.FullName,
                ParentPath = GetParentPath(path, currentPath, folderOrMessage, isException)
            });
        }
        public List<FolderModels> SearchDir(DirectoryInfo Path)
        {
            try
            {
                DirectoryInfo[] directories = CheckDirectory(Path);

                foreach (var directory in directories)
                {
                    AddFolder(directory, directory.Name);
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessing.ErrorWriting(ex.Message);
                AddFolder(Path, ex.Message, true);
                listOfDirectory.Insert(0, listOfDirectory.Last());
                listOfDirectory.RemoveAt(listOfDirectory.Count - 1);
            }
            return listOfDirectory;
        }
        private DirectoryInfo ProcessingExceprionPath(string message, DirectoryInfo path, bool isRoot)
        {
            ExceptionProcessing.ErrorWriting(message);
            if (isRoot)
                path = path.Parent;
            currentPath = GetCurrentPath(path, null);
            exceptionPath.Add(path.FullName);
            return new DirectoryInfo(GetParentPath(path, currentPath, null, false));
        }

        public List<FileModels> SearchFile(DirectoryInfo Path)
        {
            isTopDirectory++;
            if (Path.FullName == Path.Root.FullName)
                isRootDir = true;
            try
            {
                if (Path.FullName != exceptionPath.Where(exception => exception == Path.FullName).FirstOrDefault()
                        && Path.FullName != distancePath.Where(path => path == Path.FullName).FirstOrDefault())
                {
                    distancePath.Add(Path.FullName);
                    SetCountFiles(Path.GetFiles(".", SearchOption.AllDirectories));
                    if (isTopDirectory == 1)
                    {
                        FileInfo[] files = Path.GetFiles();
                        foreach (FileInfo file in files)
                        {
                            listOfFiles.Add(new FileModels
                            {
                                PathName = file.DirectoryName,
                                FileName = file.Name,
                                FileLenght = file.Length / 1024
                            });
                        }
                    }
                }
                else
                    CountFiles(Path);
            }
            catch (UnauthorizedAccessException unEx)
            {
                CountFiles(
                    ProcessingExceprionPath(unEx.Message, 
                    new DirectoryInfo(unEx.Message.Substring(unEx.Message.IndexOf('\"')).
                        Substring(1).TrimEnd(new char[] { '"', '.' })), isRootDir));
            }
            catch (IOException ioEx)
            {
                ExceptionProcessing.ErrorWriting(ioEx.Message);
            }

            return listOfFiles;
        }
        public void CountFiles(DirectoryInfo Path)
        {
            DirectoryInfo[] directoryInfo = Path.GetDirectories();
            if (distancePath.Where(path => path == Path.FullName).
                FirstOrDefault() != Path.FullName && Path.GetFiles().Count() > 0)
                    SetCountFiles(Path.GetFiles());
            if (directoryInfo != null && directoryInfo.Count() > 0)
            {
                for (int index = 0; index < directoryInfo.Length; index++)
                {
                    if (directoryInfo[index].FullName !=
                        exceptionPath.Where(exception => exception == directoryInfo[index].FullName).FirstOrDefault()
                        && directoryInfo[index].FullName !=
                        distancePath.Where(path => path == directoryInfo[index].FullName).FirstOrDefault())
                        if (Directory.Exists(directoryInfo[index].FullName))
                            SearchFile(directoryInfo[index]);
                }
            }
        }

        private void SetCountFiles(FileInfo[] files)
        {
            this.less += files.Where(file => file.Length / 1024 < 10000).Count();
            this.middle += files.Where(file => file.Length / 1024 >= 10000 && file.Length / 1024 <= 50000).Count();
            this.more += files.Where(file => file.Length / 1024 > 100000).Count();
            FileCountModels.SetCount(less, middle, more);
        }
    }
}

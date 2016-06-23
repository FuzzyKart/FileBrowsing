using FileBrowsing.Services;
using FileBrowsing.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace FileBrowsing.Controllers
{
    public class FileFolderController : ApiController
    {
        private string _path = AppDomain.CurrentDomain.BaseDirectory;
        SearchDirectory searchDirectory = new SearchDirectory();
        FileFolderViewModel fileFolderViewModel = new FileFolderViewModel();
        // GET: api/values
        [HttpGet]
        public FileFolderViewModel Get()
        {
            fileFolderViewModel.FileFolderInitializer(searchDirectory.SearchDir(new DirectoryInfo(_path)),
                searchDirectory.SearchFile(new DirectoryInfo(_path)));
            return fileFolderViewModel;
        }

        // GET api/values/5
        [HttpGet]
        public FileFolderViewModel Get(string path)
        {
            if (path == "MyComputer") {
                string[] logicalDrives = Environment.GetLogicalDrives();
                for (int index = 0; index < logicalDrives.Length; index++)
                    fileFolderViewModel.LogicalDrivesInitializer(logicalDrives[index], 
                        searchDirectory.SearchFile(new DirectoryInfo(logicalDrives[index])));
            }
            else
                fileFolderViewModel.FileFolderInitializer(searchDirectory.SearchDir(new DirectoryInfo(path)),
                    searchDirectory.SearchFile(new DirectoryInfo(path)));
            return fileFolderViewModel;
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}

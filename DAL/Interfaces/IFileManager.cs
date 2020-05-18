using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interfaces
{
    public interface IFileManager
    {
        bool CreateDirectories(string rootPath, string fileName);
        string GetSourceFolder(string rootPath, string fileName);
        string GetHDVideoOuputFilePath(string rootPath, string fileName);
        public string GetThumbnailsOutputFolder(string rootPath, string fileName);
    }
}

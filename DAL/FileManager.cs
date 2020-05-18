using BL.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BL
{
    public class FileManager : IFileManager
    {
        private const string _sourceVideoFolderPath = @"\Media\SourceVideo\";
        private const string _hdVideoFolderPath = @"\Media\HDVideo\";
        private const string _thumbnailsFolderPath = @"\Media\Thumbnails\";
        private readonly ILogger<FileManager> _logger;
        public FileManager(ILogger<FileManager> logger)
        {
            _logger = logger;
        }
        public bool CreateDirectories(string rootPath, string fileName)
        {
            try
            {
                var shortFileName = Path.GetFileNameWithoutExtension(fileName);
                string sourceFolderPath = string.Format(@"{0}{1}{2}", rootPath, _sourceVideoFolderPath, shortFileName);
                if (!Directory.Exists(sourceFolderPath))
                {
                    Directory.CreateDirectory(sourceFolderPath);
                }
                
                string hdVideoFolder = string.Format(@"{0}{1}{2}", rootPath, _hdVideoFolderPath, shortFileName);
                if (!Directory.Exists(hdVideoFolder))
                {
                    Directory.CreateDirectory(hdVideoFolder);
                }

                string thumbnailFolderPath = string.Format(@"{0}{1}{2}", rootPath, _thumbnailsFolderPath, shortFileName);
                if (!Directory.Exists(thumbnailFolderPath))
                {
                    Directory.CreateDirectory(thumbnailFolderPath);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return false;
        }

        public string GetSourceFolder(string rootPath, string fileName)
        {
            var shortFileName = Path.GetFileNameWithoutExtension(fileName);
            return string.Format(@"{0}{1}{2}", rootPath, _sourceVideoFolderPath, shortFileName);
        }

        public string GetHDVideoOuputFilePath(string rootPath, string fileName)
        {
            var shortFileName = Path.GetFileNameWithoutExtension(fileName);
            return string.Format(@"{0}{1}{2}\{3}_HD.mp4", rootPath, _hdVideoFolderPath, shortFileName, fileName);
        }

        public string GetThumbnailsOutputFolder(string rootPath, string fileName)
        {
            var shortFileName = Path.GetFileNameWithoutExtension(fileName);
            return string.Format(@"{0}{1}{2}", rootPath, _thumbnailsFolderPath, shortFileName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BL.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AngularVideoConverter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private IWebHostEnvironment _hostingEnvironment;
        private ILogger<FileUploadController> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public FileUploadController(IWebHostEnvironment hostingEnvironment, ILogger<FileUploadController> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _hostingEnvironment = hostingEnvironment;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        [HttpPost, DisableRequestSizeLimit, Route("file-upload")]
        public async Task<IActionResult> FileUpload()
        {
            try
            {
                var fileInput = Request.Form.Files.FirstOrDefault();
                if (fileInput != null)
                {
                    string sourceVideoFolderPath = string.Empty;
                    string rootPath = _hostingEnvironment.ContentRootPath;
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var fileManagerService = scope.ServiceProvider.GetRequiredService<IFileManager>();
                        var videoConverterService = scope.ServiceProvider.GetRequiredService<IVideoConverter>();
                        var IsDirectoriesCreated = fileManagerService.CreateDirectories(rootPath, fileInput.FileName);
                        if (IsDirectoriesCreated)
                        {
                            sourceVideoFolderPath = fileManagerService.GetSourceFolder(rootPath, fileInput.FileName);
                            string fileName = ContentDispositionHeaderValue.Parse(fileInput.ContentDisposition).FileName.Trim('"');
                            string sourceFilefullPath = Path.Combine(sourceVideoFolderPath, fileName);
                            using (var stream = new FileStream(sourceFilefullPath, FileMode.Create))
                            {
                                fileInput.CopyTo(stream);
                            }
                            string hdOutputFolder = fileManagerService.GetHDVideoOuputFilePath(rootPath, fileName);
                            var conversionHdResult = await videoConverterService.HDVideoConvert(sourceFilefullPath, fileName + "HD.mp4");
                        }
                    }
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
            }
            return BadRequest();
        }
    }
}
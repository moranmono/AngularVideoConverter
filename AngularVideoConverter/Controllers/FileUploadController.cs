using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AngularVideoConverter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private IWebHostEnvironment _hostingEnvironment;
        private ILogger<FileUploadController> _logger;

        public FileUploadController(IWebHostEnvironment hostingEnvironment, ILogger<FileUploadController> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        [HttpPost, DisableRequestSizeLimit, Route("file-upload")]
        public IActionResult FileUpload()
        {
            try
            {
                ///TODO: folder as file name
                var file = Request.Form.Files[0];
                var folderName = file.FileName;
                var sourceVideoFolderPath = string.Format(@"{0}\Media\SourceVideo\{1}", _hostingEnvironment.ContentRootPath, Path.GetFileNameWithoutExtension(file.FileName));
                if(!Directory.Exists(sourceVideoFolderPath))
                {
                    Directory.CreateDirectory(sourceVideoFolderPath);
                }
                if(file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(sourceVideoFolderPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
            }
            return BadRequest();
        }
    }
}
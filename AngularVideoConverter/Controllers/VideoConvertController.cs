using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BL.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AngularVideoConverter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoConvertController : ControllerBase
    {
        private readonly IVideoConverter _videoConverter;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<VideoConvertController> _logger;

        public VideoConvertController(IVideoConverter videoConverter, ILogger<VideoConvertController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _videoConverter = videoConverter;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }
        [HttpPost, Route("convert-hd")]
        public async Task<IActionResult> ConvertHDVideo(string fullFileName)
        {
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(fullFileName);
                string inputFilePath = string.Format(@"{0}\Media\SourceVideo\{1}", _hostingEnvironment.ContentRootPath, fileName);
                string outputFolderPath = string.Format(@"{0}\Media\HDVideo\{1}", _hostingEnvironment.ContentRootPath, fileName);
                
                if (!Directory.Exists(outputFolderPath))
                {
                    Directory.CreateDirectory(outputFolderPath);
                }
                var conversionResult = _videoConverter.HDVideoConvert(inputFilePath, Path.Combine(outputFolderPath, fileName + "HD.mp4"));

                return Ok(conversionResult.Result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return BadRequest();
        }
    }
}
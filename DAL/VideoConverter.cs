using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BL.Interfaces;
using Microsoft.Extensions.Logging;
using Xabe.FFmpeg;

namespace BL
{
    public class VideoConverter: IVideoConverter
    {
        private readonly ILogger _logger;
        public VideoConverter(ILogger<VideoConverter> logger)
        {
            _logger = logger;
        }

        public async Task<IConversionResult> HDVideoConvert(string inputFilePath, string outputPath)
        {
            var cancellationVideoConverterTokenSource = new CancellationTokenSource();
            try
            {
                var info = await FFmpeg.GetMediaInfo(inputFilePath);
                IVideoStream videoStreamResize = info.VideoStreams.FirstOrDefault()
                    ?.SetCodec(VideoCodec.h264)
                    ?.SetSize(VideoSize.Hd480)
                    ?.SetFramerate(30);

                IConversionResult conversionVideoResult = await FFmpeg.Conversions.New()
                    .AddStream(videoStreamResize)
                    .SetOutput(outputPath)
                    .Start(cancellationVideoConverterTokenSource.Token);
                return conversionVideoResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return null;
        }

        public Task<IConversionResult> ThumbnailVideoConvert(string inputFilePath)
        {
            throw new NotImplementedException();
        }
    }
}

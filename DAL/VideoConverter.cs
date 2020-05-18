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
    public class VideoConverter : IVideoConverter
    {
        private readonly ILogger _logger;
        public VideoConverter(ILogger<VideoConverter> logger)
        {
            _logger = logger;
        }

        public async Task<IConversionResult> HDVideoConvert(string inputFilePath, string outputFilePath)
        {
            var cancellationVideoToken = new CancellationTokenSource();
            try
            {
                var info = await FFmpeg.GetMediaInfo(inputFilePath);
                IVideoStream videoStreamResize = info.VideoStreams.FirstOrDefault()
                    ?.SetCodec(VideoCodec.h264)
                    ?.SetSize(VideoSize.Hd480)
                    ?.SetFramerate(30);

                IConversionResult conversionVideoResult = await FFmpeg.Conversions.New()
                    .AddStream(videoStreamResize)
                    .SetOutput(outputFilePath)
                    .Start(cancellationVideoToken.Token);
                return conversionVideoResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return null;
        }

        public async Task<IConversionResult> ThumbnailVideoConvert(string inputFilePath, string outputFolder, string fileName, int frameSecond)
        {
            var cancellationThumbnailToken = new CancellationTokenSource();
            try
            {
                var info = await FFmpeg.GetMediaInfo(inputFilePath);
                IVideoStream videoStreamThumbnail = info.VideoStreams.FirstOrDefault()
                  ?.SetCodec(VideoCodec.png);
                IConversionResult conversionImageResult = await FFmpeg.Conversions.New()
                    .AddStream(videoStreamThumbnail)
                    .ExtractNthFrame(frameSecond, (number) => { return string.Format(@"{0}\{1}_{2}.png",outputFolder ,fileName, number); })
                    .Start(cancellationThumbnailToken.Token);
                return conversionImageResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return null;
        }
    }
}

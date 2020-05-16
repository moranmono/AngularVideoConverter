using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace BL.Interfaces
{
    public interface IVideoConverter
    {
        Task<IConversionResult> HDVideoConvert(string inputFilePath, string outputPath);
        Task<IConversionResult> ThumbnailVideoConvert(string inputFilePath);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace BL.Interfaces
{
    public interface IVideoConverter
    {
        Task<IConversionResult> HDVideoConvert(string inputFilePath, string outputFilePath);
        Task<IConversionResult> ThumbnailVideoConvert(string inputFilePath, string outputFolder, string fileName, int frameSecond);
    }
}

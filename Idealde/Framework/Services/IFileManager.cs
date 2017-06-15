using System.Threading.Tasks;

namespace Idealde.Framework.Services
{
    public interface IFileManager
    {
        string GetTempFilePath(string filePath);

        Task Write(string filePath, string fileContent);

        Task<string> ReadToEnd(string filePath);

        Task Copy(string sourceFilePath, string destinationFilePath);
    }
}
#region Using Namespace

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

#endregion

namespace Idealde.Framework.Services
{
    public class FileManager : IFileManager
    {
        private readonly Dictionary<string, string> _realFilePathToTempFilePathLookup;

        public FileManager()
        {
            _realFilePathToTempFilePathLookup = new Dictionary<string, string>();
        }

        public string GetTempFilePath(string filePath)
        {
            string tempFilePath;
            if (!_realFilePathToTempFilePathLookup.TryGetValue(filePath, out tempFilePath))
            {
                tempFilePath = Path.GetTempPath() + Guid.NewGuid() + "\\" + Path.GetFileName(filePath);

                // ReSharper disable once AssignNullToNotNullAttribute
                Directory.CreateDirectory(Path.GetDirectoryName(tempFilePath));

                _realFilePathToTempFilePathLookup.Add(filePath, tempFilePath);
            }

            return tempFilePath;
        }

        public async Task Write(string filePath, string fileContent)
        {
            var writer = new StreamWriter(filePath);
            await writer.WriteAsync(fileContent);
            writer.Close();
        }

        public async Task<string> ReadToEnd(string filePath)
        {
            var reader = new StreamReader(filePath);
            var content = await reader.ReadToEndAsync();
            reader.Close();
            return content;
        }

        public Task Copy(string sourceFilePath, string destinationFilePath)
        {
            var source = new FileStream(sourceFilePath, FileMode.OpenOrCreate);
            var destination = new FileStream(destinationFilePath, FileMode.Truncate);

            return source.CopyToAsync(destination);
        }
    }
}
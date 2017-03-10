using System.IO;
using ExeAttacher.Core.Services;

namespace ExeAttacher.Services.FileServices
{
    public class FileHandlingService : IFileHandlingService
    {
        public Stream GetFileStream(string filePath)
        {
            return new FileStream(filePath, FileMode.OpenOrCreate);
        }

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public string ChangeExtension(string filePath, string newExtesion)
        {
            return Path.ChangeExtension(filePath, newExtesion);
        }
    }
}
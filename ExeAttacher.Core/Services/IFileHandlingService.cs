using System.IO;

namespace ExeAttacher.Core.Services
{
    public interface IFileHandlingService
    {
        Stream GetFileStream(string filePath);

        bool FileExists(string filePath);

        string ChangeExtension(string filePath, string newExtesion);        
    }
}
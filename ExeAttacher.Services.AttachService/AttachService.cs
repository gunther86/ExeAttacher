using ExeAttacher.Core.Constants;
using ExeAttacher.Core.Exceptions;
using ExeAttacher.Core.Services;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ExeAttacher.Services.AttachService
{
    public class AttachService : IAttachService
    {
        private const string ExeMagicBytes = "MZ";
        private readonly IFileHandlingService fileHandlingService;

        public AttachService(IFileHandlingService fileHandlingService)
        {
            this.fileHandlingService = fileHandlingService;
        }

        public async Task AttachExe(string filePath)
        {
            this.EnsureFileExists(filePath);
            this.EnsureIsExeFileByName(filePath);

            using (var sourceFile = this.fileHandlingService.GetFileStream(filePath))
            using (var attachedFile = this.fileHandlingService.GetFileStream(Path.ChangeExtension(filePath, FileConsts.AttachedExtension)))
            {
                var header = new byte[ExeMagicBytes.Length];
                await sourceFile.ReadAsync(header, 0, header.Length);

                if (Encoding.UTF8.GetString(header) == ExeMagicBytes)
                {
                    await sourceFile.CopyToAsync(attachedFile);
                }
                else
                {
                    throw new Exception<NoExeFileExceptionArgs>(new NoExeFileExceptionArgs(filePath));
                }
            }
        }

        public async Task RevertExe(string filePath)
        {
            if (filePath.EndsWith(FileConsts.AttachedExtension) && this.fileHandlingService.FileExists(filePath))
            {
                using (var sourceFile = this.fileHandlingService.GetFileStream(filePath))
                using (var revertedFile = this.fileHandlingService.GetFileStream(Path.ChangeExtension(filePath, FileConsts.ExeFileExtension)))
                {
                    var header = Encoding.UTF8.GetBytes(ExeMagicBytes);
                    await revertedFile.WriteAsync(header, 0, header.Length);
                    await sourceFile.CopyToAsync(revertedFile);
                }
            }
            else
            {
                // todo throw custom exception.
            }
        }

        private void EnsureIsExeFileByName(string filePath)
        {
            if (!filePath.EndsWith(FileConsts.ExeFileExtension))
            {
                throw new Exception<NoExeFileExceptionArgs>(new NoExeFileExceptionArgs(filePath));
            }
        }

        private void EnsureFileExists(string filePath)
        {
            if(!this.fileHandlingService.FileExists(filePath))
            {
                throw new Exception<NoAccessFileExceptionArgs>(new NoAccessFileExceptionArgs(filePath));
            }
        }
    }
}
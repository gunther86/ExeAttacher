using System;
using ExeAttacher.Core.Resources;

namespace ExeAttacher.Core.Exceptions
{
    [Serializable]
    public sealed class NoExeFileExceptionArgs : ExceptionArgs
    {
        private readonly string filePath;

        public NoExeFileExceptionArgs(string filePath)
        {
            this.filePath = filePath;
        }

        public string FilePath => this.filePath;

        public override string Message => string.Format(ErrorMessages.InvalidExeFile, this.filePath);
    }
}
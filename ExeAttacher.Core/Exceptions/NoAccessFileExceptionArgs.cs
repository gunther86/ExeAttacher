using System;
using ExeAttacher.Core.Resources;

namespace ExeAttacher.Core.Exceptions
{
    [Serializable]
    public sealed class NoAccessFileExceptionArgs : ExceptionArgs
    {
        private readonly string filePath;

        public NoAccessFileExceptionArgs(string filePath)
        {
            this.filePath = filePath;
        }

        public string FilePath => this.filePath;

        public override string Message => string.Format(ErrorMessages.CannotAccessToFile, this.filePath);
    }
}
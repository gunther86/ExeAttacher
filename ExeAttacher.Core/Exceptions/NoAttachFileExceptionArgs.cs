using ExeAttacher.Core.Resources;
using System;

namespace ExeAttacher.Core.Exceptions
{
    [Serializable]
    public sealed class NoAttachFileExceptionArgs : ExceptionArgs
    {
        private readonly string filePath;

        public NoAttachFileExceptionArgs(string filePath)
        {
            this.filePath = filePath;
        }

        public string FilePath => this.filePath;

        public override string Message => string.Format(ErrorMessages.InvalidAttachedFile, this.filePath);
    }
}
using ExeAttacher.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

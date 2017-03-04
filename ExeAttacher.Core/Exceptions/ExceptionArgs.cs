using System;

namespace ExeAttacher.Core.Exceptions
{
    [Serializable]
    public abstract class ExceptionArgs
    {
        public virtual string Message => string.Empty;
    }
}
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace ExeAttacher.Core.Exceptions
{
    [Serializable]
    public sealed class Exception<TExceptionArgs> : Exception, ISerializable
    where TExceptionArgs : ExceptionArgs
    {
        private readonly TExceptionArgs args;

        public TExceptionArgs Args => this.args;

        public Exception(string message = null, Exception innerException = null)
        : this(null, message, innerException) { }

        public Exception(TExceptionArgs args, string message = null, Exception innerException = null)
        : base(message, innerException)
        {
            this.args = args;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        private Exception(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
            this.args = (TExceptionArgs)info.GetValue(nameof(this.Args), typeof(TExceptionArgs));
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(this.Args), this.args);
            base.GetObjectData(info, context);
        }

        public override string Message
        {
            get
            {
                string baseMessage = base.Message;
                return (args == null) ? baseMessage : args.Message;
            }
        }

        public override bool Equals(object obj)
        {
            Exception<TExceptionArgs> other = obj as Exception<TExceptionArgs>;
            if (other == null)
            {
                return false;
            }

            return object.Equals(args, other.args) && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
using System;

namespace XLAutoDeploy.Manifests
{
    public sealed class InvalidAddInException : Exception
    {
        public InvalidAddInException()
        {
        }

        public InvalidAddInException(string message)
            : base(message)
        {
        }

        public InvalidAddInException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

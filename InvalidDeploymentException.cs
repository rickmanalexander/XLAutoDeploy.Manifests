using System;

namespace XLAutoDeploy.Manifests
{
    public sealed class InvalidDeploymentException: Exception
    {
        public InvalidDeploymentException()
        {
        }

        public InvalidDeploymentException(string message)
            : base(message)
        {
        }

        public InvalidDeploymentException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

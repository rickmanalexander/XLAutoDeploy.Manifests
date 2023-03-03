using System;

namespace XLAutoDeploy.Manifests.Utilities
{
    public static class ManifestFileNaming
    {
        public static string AddInManifestFileName(string addInName)
        {
            return String.Format(Constants.AddInManifestParameterizedFileName, addInName);
        }

        public static string DeploymentManifestFileName(string addInName)
        {
            return String.Format(Constants.DeploymentManifestParameterizedFileName, addInName);
        }

        public static string UpdateQueryInfoManifestFileName(string addInName)
        {
            return String.Format(Constants.UpdateQueryInfoParameterizedFileName, addInName);
        }
    }
}

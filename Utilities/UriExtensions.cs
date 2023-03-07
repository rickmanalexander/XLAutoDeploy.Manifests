using System;

namespace XLAutoDeploy.Manifests.Utilities
{
    public static class UriExtensions
    {
        public static string AsPath(this Uri uri)
        {
            if (uri == null)
                return null;

            if (uri.IsFile || uri.IsUnc)
                return uri.LocalPath;

            return uri.ToString();
        }

        public static bool IsSupportedScheme(this Uri uri) 
        { 
            return uri.Scheme == Uri.UriSchemeFile || uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
        } 
    }
}

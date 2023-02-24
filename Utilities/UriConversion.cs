using System;

namespace XLAutoDeploy.Manifests.Utilities
{
    public static class UriConversion
    {
        public static string AsString(this Uri uri)
        {
            if (uri == null)
                return null;

            if (uri.IsAbsoluteUri)
                return uri.AbsoluteUri;

            if (uri.IsFile || uri.IsUnc)
                return uri.LocalPath;

            return uri.ToString();
        }

        public static Uri AsUri(this string s)
        {
            if (s == null)
                return null;

            return new Uri(s);
        }

        public static bool IsSupportedScheme(Uri uri) 
        { 
            return uri.Scheme == Uri.UriSchemeFile || uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
        } 
    }
}

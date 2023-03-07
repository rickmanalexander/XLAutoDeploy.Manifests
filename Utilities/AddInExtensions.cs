using System;
using System.IO;
using System.Linq;

namespace XLAutoDeploy.Manifests.Utilities
{
    public static class AddInExtensions
    {
        public static void AppendManifestFileExtension(this AddIn addIn)
        {
            RemoveManifestFileExtension(addIn);

            var fullExtension = "." + Constants.XLAutoDeployFileExtention;

            if (addIn?.Dependencies?.Any() == true)
            {
                foreach (var dependency in addIn.Dependencies)
                {
                    dependency.Uri = new Uri(dependency.Uri.AbsoluteUri + fullExtension);

                    if (dependency?.AssetFiles?.Any() == true)
                    {
                        foreach (var file in dependency.AssetFiles)
                        {
                            file.Uri = new Uri(file.Uri.AbsoluteUri + fullExtension);
                        }
                    }
                }
            }

            if (addIn?.AssetFiles?.Any() == true)
            {
                foreach (var file in addIn.AssetFiles)
                {
                    file.Uri = new Uri(file.Uri.AbsoluteUri + fullExtension);
                }
            }
        }

        public static void RemoveManifestFileExtension(this AddIn addIn)
        {
            var fullExtension = "." + Constants.XLAutoDeployFileExtention;
            var extensionChars = fullExtension.ToCharArray();

            if (addIn?.Dependencies?.Any() == true)
            {
                foreach (var dependency in addIn.Dependencies)
                {
                    if (Path.GetExtension(dependency.Uri.AbsoluteUri).Equals(Constants.XLAutoDeployFileExtention, StringComparison.OrdinalIgnoreCase))
                    {
                        dependency.Uri = new Uri(dependency.Uri.AbsoluteUri.TrimEnd(extensionChars));
                    }

                    if (dependency?.AssetFiles?.Any() == true)
                    {
                        foreach (var file in dependency.AssetFiles)
                        {
                            if (Path.GetExtension(file.Uri.AbsoluteUri).Equals(Constants.XLAutoDeployFileExtention, StringComparison.OrdinalIgnoreCase))
                            {
                                file.Uri = new Uri(file.Uri.AbsoluteUri.TrimEnd(extensionChars));
                            }
                        }
                    }
                }
            }

            if (addIn?.AssetFiles?.Any() == true)
            {
                foreach (var file in addIn.AssetFiles)
                {
                    if (Path.GetExtension(file.Uri.AbsoluteUri).Equals(Constants.XLAutoDeployFileExtention, StringComparison.OrdinalIgnoreCase))
                    {
                        file.Uri = new Uri(file.Uri.AbsoluteUri.TrimEnd(extensionChars));
                    }
                }
            }
        }

        /// <summary>
        /// Returns the sum total of bytes in an <see cref="AddIn"/> manifest from the 
        /// <see cref="Dependency"/>(s) and <see cref="AssetFile"/>(s), excluding the size of add-in file itself.
        /// </summary>  
        public static long GetTotalSize(this AddIn addIn)
        {
            return addIn?.Dependencies?.Sum(d => d?.Size + d?.AssetFiles?.Sum(a => a?.Size) ?? 0) ?? 0
                + addIn?.AssetFiles?.Sum(d => d.Size) ?? 0;
        }
    }
}

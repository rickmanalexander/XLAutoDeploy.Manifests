using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace XLAutoDeploy.Manifests.Utilities
{
    public static class Validation
    {
        public static HashSet<string> ProcessorArchitectureValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "NONE",
            "MSIL",
            "X86",
            "IA64",
            "AMD64",
            "ARM"
        };

        public static void ValidateDeploymentAndAddIn(Deployment deployment, AddIn addIn)
        {
            ValidateDeployment(deployment);
            ValidateAddIn(addIn);

            if (deployment.Settings.MapFileExtensions)
            {
                if (addIn?.Dependencies?.Any() == true)
                {
                    foreach(var dependency in addIn.Dependencies)
                    {
                        if(!Path.GetExtension(dependency.Uri.AbsoluteUri).Equals(Constants.XLAutoDeployFileExtention, StringComparison.OrdinalIgnoreCase))
                        {
                            throw new InvalidAddInException(Errors.GetFormatedErrorMessage($"Attempting to validate a {nameof(Dependency.Uri)} instance.",
                                $"The {nameof(Dependency.Uri)} must have a .{Constants.XLAutoDeployFileExtention} file extension.",
                                $"Supply a valid value for {nameof(Dependency.Uri)}."));
                        }

                        if (dependency?.AssetFiles?.Any() == true)
                        {
                            foreach (var file in dependency.AssetFiles)
                            {
                                if (!Path.GetExtension(file.Uri.AbsoluteUri).Equals(Constants.XLAutoDeployFileExtention, StringComparison.OrdinalIgnoreCase))
                                {
                                    throw new InvalidAddInException(Errors.GetFormatedErrorMessage($"Attempting to validate a {nameof(AssetFile.Uri)} instance.",
                                        $"The {nameof(AssetFile.Uri)} must have a .{Constants.XLAutoDeployFileExtention} file extension.",
                                        $"Supply a valid value for {nameof(AssetFile.Uri)}."));
                                }
                            }
                        }
                    }
                }

                if (addIn?.AssetFiles?.Any() == true)
                {
                    foreach (var file in addIn.AssetFiles)
                    {
                        if (!Path.GetExtension(file.Uri.AbsoluteUri).Equals(Constants.XLAutoDeployFileExtention, StringComparison.OrdinalIgnoreCase))
                        {
                            throw new InvalidAddInException(Errors.GetFormatedErrorMessage($"Attempting to validate a {nameof(AssetFile.Uri)} instance.",
                                $"The {nameof(AssetFile.Uri)} must have a .{Constants.XLAutoDeployFileExtention} file extension.",
                                $"Supply a valid value for {nameof(AssetFile.Uri)}."));
                        }
                    }
                }
            }
        }

        public static void ValidateDeployment(Deployment deployment)
        {
            var errorContext = $"Attempting to validate a {nameof(Deployment)} instance.";

            if (deployment == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(Deployment)} cannot be null.",
                    $"Supply a valid {nameof(Deployment)} instance."));
            }

            if (deployment.AddInUri == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(Deployment.AddInUri)} cannot be null.",
                    $"Supply a valid value for {nameof(Deployment.AddInUri)}."));
            }

            ValidateDescription(deployment.Description);
            ValidateDeploymentSettings(deployment.Settings);
            ValidateRequiredOperatingSystem(deployment.RequiredOperatingSystem);
            ValidateCompatibleFrameworks(deployment.CompatibleFrameworks);
        }

        public static void ValidateAddIn(AddIn addIn)
        {
            var errorContext = $"Attempting to validate an {nameof(AddIn)} instance.";

            if (addIn == null)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AddIn)} cannot be null.",
                    $"Supply a valid {nameof(AddIn)} instance."));
            }

            if (addIn.Uri == null)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AddIn.Uri)} cannot be null.",
                    $"Supply a valid {nameof(AddIn.Uri)}."));
            }

            if (addIn.DeploymentUri == null)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AddIn.DeploymentUri)} cannot be null.",
                    $"Supply a valid {nameof(AddIn.DeploymentUri)}."));
            }

            ValidateAddInIdentity(addIn.Identity);

            if (addIn?.Dependencies?.Any() == true)
            {
                ValidateDependencies(addIn.Dependencies);
            }

            if (addIn?.AssetFiles?.Any() == true)
            {
                ValidateAssetFiles(addIn.AssetFiles);
            }
        }

        #region Deployment
        public static void ValidateDescription(Description description)
        {
            var errorContext = $"Attempting to validate a {nameof(Description)} instance.";

            if (description == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"A {nameof(Description)} cannot be null.",
                    $"Supply a valid {nameof(Description)} instance."));
            }

            if (String.IsNullOrEmpty(description.Publisher) || String.IsNullOrWhiteSpace(description.Publisher))
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(Description.Publisher)} is either null, empty, or whitespace.",
                    $"Supply a valid value for {nameof(Description.Publisher)}."));
            }

            if (String.IsNullOrEmpty(description.Manufacturer) || String.IsNullOrWhiteSpace(description.Manufacturer))
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(Description.Manufacturer)} is either null, empty, or whitespace.",
                    $"Supply a valid value for {nameof(Description.Manufacturer)}."));
            }

            if (String.IsNullOrEmpty(description.Product) || String.IsNullOrWhiteSpace(description.Product))
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(Description.Product)} is either null, empty, or whitespace.",
                    $"Supply a valid value for {nameof(Description.Product)}."));
            }
        }

        public static void ValidateDeploymentSettings(DeploymentSettings deploymentSettings)
        {
            var errorContext = $"Attempting to validate a {nameof(DeploymentSettings)} instance.";

            if (deploymentSettings == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(DeploymentSettings)} cannot be null.",
                    $"Supply a valid {nameof(DeploymentSettings)} instance."));
            }

            if (deploymentSettings?.DeploymentBasis == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(DeploymentSettings.DeploymentBasis)} is null.",
                    $"Supply a value for {nameof(DeploymentSettings.DeploymentBasis)}."));
            }

            if (deploymentSettings.MinimumRequiredVersion == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(DeploymentSettings.MinimumRequiredVersion)} is null.",
                    $"Supply a value for {nameof(DeploymentSettings.MinimumRequiredVersion)}."));
            }

            if (deploymentSettings?.MapFileExtensions == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(DeploymentSettings.MapFileExtensions)} is null.",
                    $"Supply a value for {nameof(DeploymentSettings.MinimumRequiredVersion)}."));
            }

            ValidateLoadBehavior(deploymentSettings.LoadBehavior);
            ValidateUpdateBehavior(deploymentSettings.UpdateBehavior);
        }

        public static void ValidateLoadBehavior(LoadBehavior loadBehavior)
        {
            var errorContext = $"Attempting to validate a {nameof(LoadBehavior)} instance.";

            if (loadBehavior == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(LoadBehavior)} cannot be null.",
                    $"Supply a valid {nameof(LoadBehavior)} instance."));
            }

            if (loadBehavior?.Install == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(LoadBehavior.Install)} is null.",
                    $"Supply a valid value for {nameof(LoadBehavior.Install)}."));
            }
        }

        public static void ValidateUpdateBehavior(UpdateBehavior updateBehavior)
        {
            var errorContext = $"Attempting to validate a {nameof(UpdateBehavior)} instance.";

            if (updateBehavior == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(UpdateBehavior)} cannot be null.",
                    $"Supply a valid {nameof(UpdateBehavior)} instance."));
            }

            if (updateBehavior?.Mode == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(UpdateBehavior.Mode)} is null.",
                    $"Supply a valid value for {nameof(UpdateBehavior.Mode)}."));
            }

            if (updateBehavior?.RequiresRestart == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(UpdateBehavior.RequiresRestart)} is null.",
                    $"Supply a valid value for {nameof(UpdateBehavior.RequiresRestart)}."));
            }

            if (updateBehavior?.RemoveDeprecatedVersion == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(UpdateBehavior.RemoveDeprecatedVersion)} is null.",
                    $"Supply a valid value for {nameof(UpdateBehavior.RemoveDeprecatedVersion)}."));
            }

            if (updateBehavior?.NotifyClient == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(UpdateBehavior.NotifyClient)} is null.",
                    $"Supply a valid value for {nameof(UpdateBehavior.NotifyClient)}."));
            }

            if (updateBehavior.RequiresRestart & updateBehavior.NotifyClient)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"Both {nameof(UpdateBehavior.RequiresRestart)} and {nameof(UpdateBehavior.NotifyClient)} cannot be true.",
                    $"Set either {nameof(UpdateBehavior.RequiresRestart)} or {nameof(UpdateBehavior.NotifyClient)} to true, but not both."));
            }

            if (updateBehavior.Expiration != null)
            {
                ValidateUpdateExpiration(updateBehavior.Expiration);
            }
        }

        public static void ValidateUpdateExpiration(UpdateExpiration updateExpiration)
        {
            var errorContext = $"Attempting to validate a {nameof(UpdateExpiration)} instance.";

            if (updateExpiration == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(UpdateExpiration)} cannot be null.",
                    $"Supply a valid {nameof(UpdateExpiration)} instance."));
            }

            if (updateExpiration?.MaximumAge == null || updateExpiration.MaximumAge == 0)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(UpdateExpiration.MaximumAge)} is null or 0.",
                    $"Supply a valid value for {nameof(UpdateExpiration.MaximumAge)}."));
            }

            if (updateExpiration?.UnitOfTime == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(UpdateExpiration.UnitOfTime)} is null.",
                    $"Supply a valid value for {nameof(UpdateExpiration.UnitOfTime)}."));
            }
        }

        public static void ValidateRequiredOperatingSystem(RequiredOperatingSystem requiredOperatingSystem)
        {
            var errorContext = $"Attempting to validate a {nameof(RequiredOperatingSystem)} instance.";

            if (requiredOperatingSystem == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(RequiredOperatingSystem)} cannot be null.",
                    $"Supply a valid {nameof(RequiredOperatingSystem)} instance."));
            }

            if (String.IsNullOrEmpty(requiredOperatingSystem.SupportUrl) || String.IsNullOrWhiteSpace(requiredOperatingSystem.SupportUrl))
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(RequiredOperatingSystem.SupportUrl)} is either null, empty, or whitespace.",
                    $"Supply a valid value for {nameof(RequiredOperatingSystem.SupportUrl)}."));
            }

            ValidateUrl(requiredOperatingSystem.SupportUrl); 

            if (requiredOperatingSystem.MinimumVersion == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(RequiredOperatingSystem.MinimumVersion)} is null.",
                    $"Supply a valid value for {nameof(RequiredOperatingSystem.MinimumVersion)}."));
            }

            if (requiredOperatingSystem?.Bitness == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(RequiredOperatingSystem.Bitness)} is null.",
                    $"Supply a valid value for {nameof(RequiredOperatingSystem.Bitness)}."));
            }
        }

        public static void ValidateCompatibleFrameworks(IEnumerable<CompatibleFramework> compatibleFrameworks)
        {
            if (compatibleFrameworks?.Any() == false)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage($"Attempting to validate an {nameof(IEnumerable<CompatibleFramework>)} instance.",
                    $"The {nameof(IEnumerable<CompatibleFramework>)} cannot be null.",
                    $"Supply a valid {nameof(IEnumerable<CompatibleFramework>)} instance."));
            }

            foreach (var framework in compatibleFrameworks)
            {
                ValidateCompatibleFramework(framework);
            }
        }

        public static void ValidateCompatibleFramework(CompatibleFramework compatibleFramework)
        {
            var errorContext = $"Attempting to validate a {nameof(CompatibleFramework)} instance.";

            if (compatibleFramework == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(CompatibleFramework)} cannot be null.",
                    $"Supply a valid {nameof(CompatibleFramework)} instance."));
            }

            if (String.IsNullOrEmpty(compatibleFramework.SupportUrl) || String.IsNullOrWhiteSpace(compatibleFramework.SupportUrl))
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(CompatibleFramework.SupportUrl)} is either null, empty, or whitespace.",
                    $"Supply a valid value for {nameof(CompatibleFramework.SupportUrl)}."));
            }

            ValidateUrl(compatibleFramework.SupportUrl);

            if (compatibleFramework?.SupportedRuntime == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(CompatibleFramework.SupportedRuntime)} is null.",
                    $"Supply a valid value for {nameof(CompatibleFramework.SupportedRuntime)}."));
            }

            if (compatibleFramework.TargetVersion == null)
            {
                throw new InvalidDeploymentException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(CompatibleFramework.TargetVersion)} is null.",
                    $"Supply a valid value for {nameof(CompatibleFramework.TargetVersion)}."));
            }
        }
        #endregion

        #region AddIn
        public static void ValidateAddInIdentity(AddInIdentity addInIdentity)
        {
            var errorContext = $"Attempting to validate an {nameof(AddInIdentity)} instance.";

            if (addInIdentity == null)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AddInIdentity)} cannot be null.",
                    $"Supply a valid {nameof(AddInIdentity)} instance."));
            }

            if (addInIdentity?.AddInType == null)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AddInIdentity.AddInType)} cannot be null.",
                    $"Supply a valid value for {nameof(AddInIdentity.AddInType)}."));
            }

            if (String.IsNullOrEmpty(addInIdentity.Title) || String.IsNullOrWhiteSpace(addInIdentity.Title))
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AddInIdentity.Title)} is either null, empty, or whitespace.",
                    $"Supply a valid value for {nameof(AddInIdentity.Title)}."));
            }

            if (String.IsNullOrEmpty(addInIdentity.Name) || String.IsNullOrWhiteSpace(addInIdentity.Name))
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AddInIdentity.Name)} is either null, empty, or whitespace.",
                    $"Supply a valid value for {nameof(AddInIdentity.Name)}."));
            }

            if (addInIdentity.Name.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AddInIdentity.Name)} contains one or more invalid file characters.",
                    $"Supply a valid value for {nameof(AddInIdentity.Name)}."));
            }

            if (addInIdentity?.FileExtension == null)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AddInIdentity.FileExtension)} cannot be null.",
                    $"Supply a valid value for {nameof(AddInIdentity.FileExtension)}."));
            }

            if (addInIdentity.Version == null)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AddInIdentity.Version)} cannot be null.",
                    $"Supply a valid {nameof(AddInIdentity.Version)}."));
            }

            if ((addInIdentity.AddInType == AddInType.Vba & addInIdentity.FileExtension == AddInFileExtensionType.Dll)
                || (addInIdentity.AddInType == AddInType.Vba & addInIdentity.FileExtension == AddInFileExtensionType.Xll))
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AddInIdentity.FileExtension)} for an add-in of type {nameof(AddInType.Vba)} must be either {nameof(AddInFileExtensionType.Xlam)} or {nameof(AddInFileExtensionType.Xla)}.",
                    $"Supply a valid {nameof(AddInIdentity.FileExtension)}."));
            }

            if ((addInIdentity.AddInType == AddInType.ExcelDna & addInIdentity.FileExtension == AddInFileExtensionType.Xlam)
                || (addInIdentity.AddInType == AddInType.ExcelDna & addInIdentity.FileExtension == AddInFileExtensionType.Xla))
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AddInIdentity.FileExtension)} for an add-in of type {nameof(AddInType.ExcelDna)} must be either {nameof(AddInFileExtensionType.Dll)} or {nameof(AddInFileExtensionType.Xll)}.",
                    $"Supply a valid {nameof(AddInIdentity.FileExtension)}."));
            }

            if ((addInIdentity.AddInType == AddInType.Automation & addInIdentity.FileExtension == AddInFileExtensionType.Xlam)
                || (addInIdentity.AddInType == AddInType.Automation & addInIdentity.FileExtension == AddInFileExtensionType.Xla))
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AddInIdentity.FileExtension)} for an add-in of type {nameof(AddInType.Automation)} must be either {nameof(AddInFileExtensionType.Dll)} or {nameof(AddInFileExtensionType.Xll)}.",
                    $"Supply a valid {nameof(AddInIdentity.FileExtension)}."));
            }

            if ((addInIdentity.AddInType == AddInType.Com & addInIdentity.FileExtension == AddInFileExtensionType.Xlam)
                || (addInIdentity.AddInType == AddInType.Com & addInIdentity.FileExtension == AddInFileExtensionType.Xla))
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AddInIdentity.FileExtension)} for an add-in of type {nameof(AddInType.Com)} must be either {nameof(AddInFileExtensionType.Dll)} or {nameof(AddInFileExtensionType.Xll)}.",
                    $"Supply a valid {nameof(AddInIdentity.FileExtension)}."));
            }

        }

        public static void ValidateDependencies(IEnumerable<Dependency> dependencies)
        {
            if (dependencies?.Any() == false)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage($"Attempting to validate an {nameof(IEnumerable<Dependency>)} instance.",
                    $"The {nameof(IEnumerable<Dependency>)} cannot be null.",
                    $"Supply a valid {nameof(IEnumerable<Dependency>)} instance."));
            }

            foreach (var dependency in dependencies)
            {
                ValidateDependency(dependency);
            }
        }

        public static void ValidateDependency(Dependency dependency)
        {
            var errorContext = $"Attempting to validate an {nameof(Dependency)} instance.";

            if (dependency == null)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(Dependency)} cannot be null.",
                    $"Supply a valid {nameof(Dependency)} instance."));
            }

            if (dependency.Uri == null)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(Dependency.Uri)} cannot be null.",
                    $"Supply a valid value for {nameof(Dependency.Uri)}."));
            }

            if (dependency?.Type == null)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(Dependency.Type)} cannot be null.",
                    $"Supply a valid value for {nameof(Dependency.Type)}."));
            }

            if (dependency?.Size == null || dependency.Size == 0)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(Dependency.Size)} cannot be null or 0.",
                    $"Supply a valid value for {nameof(Dependency.Size)}."));
            }

            ValidateAssemblyIdentity(dependency.AssemblyIdentity);

            if (dependency?.ManagedAssembly == null)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(Dependency.ManagedAssembly)} cannot be null.",
                    $"Supply a valid value for {nameof(Dependency.ManagedAssembly)}."));
            }

            ValidateFilePlacement(dependency.FilePlacement);

            if (dependency?.AssetFiles?.Any() == true)
            {
                ValidateAssetFiles(dependency.AssetFiles);
            }
        }


        public static void ValidateAssemblyIdentity(AssemblyIdentity assemblyId)
        {
            var errorContext = $"Attempting to validate an {nameof(AssemblyIdentity)} instance.";

            if (assemblyId == null)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AssemblyIdentity)} cannot be null.",
                    $"Supply a valid {nameof(AssemblyIdentity)} instance."));
            }

            if (String.IsNullOrEmpty(assemblyId.Name) || String.IsNullOrWhiteSpace(assemblyId.Name))
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AssemblyIdentity.Name)} is either null, empty, or whitespace.",
                    $"Supply a valid value for {nameof(AssemblyIdentity.Name)}."));
            }

            if (assemblyId.Name.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AssemblyIdentity.Name)} contains one or more invalid file characters.",
                    $"Supply a valid value for {nameof(AssemblyIdentity.Name)}."));
            }

            if (assemblyId.Version == null)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AssemblyIdentity.Version)} cannot be null.",
                    $"Supply a valid {nameof(AssemblyIdentity.Version)}."));
            }

            if (String.IsNullOrEmpty(assemblyId.ProcessorArchitecture) || String.IsNullOrWhiteSpace(assemblyId.ProcessorArchitecture))
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AssemblyIdentity.ProcessorArchitecture)} is either null, empty, or whitespace.",
                    $"Supply a valid value for {nameof(AssemblyIdentity.ProcessorArchitecture)}."));
            }

            if (!ProcessorArchitectureValues.Contains(assemblyId.ProcessorArchitecture))
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
    $"The {nameof(AssemblyIdentity.ProcessorArchitecture)} value {assemblyId.ProcessorArchitecture} is not defined.",
    $"Supply a valid value from the following list for {nameof(AssemblyIdentity.ProcessorArchitecture)}: {String.Join(", ", ProcessorArchitectureValues)}."));
            }
        }


        public static void ValidateAssetFiles(IEnumerable<AssetFile> assetFiles)
        {
            if (assetFiles?.Any() == false)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage($"Attempting to validate an {nameof(IEnumerable<AssetFile>)} instance.",
                    $"The {nameof(IEnumerable<AssetFile>)} cannot be null.",
                    $"Supply a valid {nameof(IEnumerable<AssetFile>)} instance."));
            }

            foreach (var file in assetFiles)
            {
                ValidateAssetFile(file);
            }
        }

        public static void ValidateAssetFile(AssetFile assetFile)
        {
            var errorContext = $"Attempting to validate an {nameof(AssetFile)} instance.";

            if (assetFile == null)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AssetFile)} cannot be null.",
                    $"Supply a valid {nameof(AssetFile)} instance."));
            }

            if (assetFile.Uri == null)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AssetFile.Uri)} cannot be null.",
                    $"Supply a valid value for the {nameof(AssetFile.Uri)}."));
            }

            if (String.IsNullOrEmpty(assetFile.Name) || String.IsNullOrWhiteSpace(assetFile.Name))
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AssetFile.Name)} is either null, empty, or whitespace.",
                    $"Supply a valid value for {nameof(AssetFile.Name)}."));
            }

            if (assetFile.Name.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AssetFile.Name)} contains one or more invalid file characters.",
                    $"Supply a valid value for {nameof(AssetFile.Name)}."));
            }

            if (assetFile?.Size == null || assetFile?.Size == 0)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(AssetFile.Size)} cannot be null or 0.",
                    $"Supply a valid value for {nameof(AssetFile.Size)}."));
            }

            ValidateFilePlacement(assetFile.FilePlacement);
        }

        public static void ValidateFilePlacement(FilePlacement filePlacement)
        {
            var errorContext = $"Attempting to validate an {nameof(FilePlacement)} instance.";

            if (filePlacement == null)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(FilePlacement)} cannot be null.",
                    $"Supply a valid {nameof(FilePlacement)} instance."));
            }

            if (filePlacement?.NextToAddIn == null)
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(FilePlacement.NextToAddIn)} value cannot be null.",
                    $"Supply a valid {nameof(FilePlacement.NextToAddIn)}."));
            }

            if (!filePlacement.NextToAddIn & (String.IsNullOrEmpty(filePlacement.SubDirectory) || String.IsNullOrWhiteSpace(filePlacement.SubDirectory)))
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                    $"The {nameof(FilePlacement.SubDirectory)} must have a non-null value if {nameof(FilePlacement.NextToAddIn)} is set to true.",
                    $"Supply a valid {nameof(FilePlacement.SubDirectory)}."));
            }

            if (filePlacement.SubDirectory != null)
            {
                if (filePlacement.SubDirectory == String.Empty || String.IsNullOrWhiteSpace(filePlacement.SubDirectory))
                {
                    throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                        $"The {nameof(FilePlacement.SubDirectory)} is either empty, or whitespace.",
                        $"Supply a valid value for {nameof(FilePlacement.SubDirectory)}."));
                }

                if (filePlacement.SubDirectory.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0)
                {
                    throw new InvalidAddInException(Errors.GetFormatedErrorMessage(errorContext,
                        $"The {nameof(FilePlacement.SubDirectory)} contains one or more invalid file characters.",
                        $"Supply a valid value for {nameof(FilePlacement.SubDirectory)}."));
                }
            }
        }
        #endregion

        public static void ValidateUrl(string url)
        {
            if (String.IsNullOrEmpty(url) || String.IsNullOrWhiteSpace(url))
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage($"Attempting to validate a {nameof(url)}",
                    $"The {nameof(url)} is either null, empty, or whitespace.",
                    $"Supply a valid value for {nameof(url)}."));
            }
            string tempUrl = url;
            if (!Regex.IsMatch(url, @"^https?:\/\/", RegexOptions.IgnoreCase))
            {
                tempUrl = "http://" + url;
            }

            if (!Uri.TryCreate(tempUrl, UriKind.Absolute, out Uri uri))
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage($"Attempting to validate a {nameof(url)}",
                    $"The {nameof(url)} is not a valid URI.",
                    $"Supply a valid value for {nameof(url)}."));
            }

            if (!((uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps) && tempUrl.Contains('.')))
            {
                throw new InvalidAddInException(Errors.GetFormatedErrorMessage($"Attempting to validate a {nameof(url)}",
                    $"The {nameof(url)} is not a valid URI.",
                    $"Supply a valid value for {nameof(url)}."));
            }
        }
    }
}

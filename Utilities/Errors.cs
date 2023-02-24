using System;

namespace XLAutoDeploy.Manifests.Utilities
{
    internal static class Errors
    {
        public static string GetFormatedErrorMessage(string context, string problem, string solution)
        {
            return String.Concat(new string[] { "Context: ", context, System.Environment.NewLine, "Problem: ", problem, System.Environment.NewLine, "Solution: ", solution });
        }
    }
}

using System;
using System.IO;

// ReSharper disable InconsistentNaming
namespace BuildMonitorPackage
{
    public static class OptionKey
    {
        public const string SolutionId = "bm_solution_id";
    }

    public static class Settings
    {
        public static string RepositoryFilename
        {
            get =>
                string.IsNullOrWhiteSpace(BuildMonitorRepositoryFilename)
                    ? DefaultRepositoryFilename
                    : BuildMonitorRepositoryFilename;
        }

        public static void CreateRepositoryPathIfNotExist()
        {
            if (!Directory.Exists(RepositoryPath))
            {
                Directory.CreateDirectory(RepositoryPath);
            }
            if (!File.Exists(RepositoryFilename))
            {
                File.Create(RepositoryFilename).Dispose();
            }
        }

        static string RepositoryPath =>
            Path.GetDirectoryName(RepositoryFilename);

        static string BuildMonitorRepositoryFilename =>
            Environment.GetEnvironmentVariable("BuildMonitorRepositoryFilename");

        static string DefaultRepositoryFilename =>
            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Build Monitor\\buildtimes.json";

    }
}
// ReSharper restore InconsistentNaming
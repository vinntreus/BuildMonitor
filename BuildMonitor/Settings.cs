using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
// ReSharper disable InconsistentNaming
namespace BuildMonitor
{
    public static class OptionKey
    {
        public const string SolutionId = "bm_solution_id";
    }

    public static class Settings
    {
        public static string RepositoryPath = string.Format("{0}\\{1}\\{2}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationFolderName, JsonFileName);

        private static string ApplicationFolder = string.Format("{0}\\{1}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationFolderName);

        private const string ApplicationFolderName = "Build Monitor";
        private const string JsonFileName = "buildtimes.json";
        

        public static void CreateApplicationFolderIfNotExist()
        {
            if(!Directory.Exists(ApplicationFolder))
            {
                Directory.CreateDirectory(ApplicationFolder);
            }
        }
    }
}
// ReSharper restore InconsistentNaming
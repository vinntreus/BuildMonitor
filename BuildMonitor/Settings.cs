using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace BuildMonitor
{
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
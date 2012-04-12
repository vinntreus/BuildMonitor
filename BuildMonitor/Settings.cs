using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

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
            if(!File.Exists(RepositoryPath))
            {
                using(var f = File.Create(RepositoryPath)){}
            }
        }

        public delegate void LogAction(string s, params object[] args);

        public static void VerifyJsonData(LogAction log)
        {
            var jsonText = File.ReadAllText(RepositoryPath);

            if (string.IsNullOrEmpty(jsonText)) return;

            if(jsonText.StartsWith("{") && jsonText.EndsWith("}"))
            {
                log("-- Found invalid json-data in file: ", RepositoryPath);
                jsonText = string.Format("[{0}]", jsonText.Replace("}{", "},{"));

                try
                {
                    JsonConvert.DeserializeObject<IEnumerable<dynamic>>(jsonText);
                    File.WriteAllText(RepositoryPath, jsonText);
                    log("-- Successfully converted invalid json data to valid");
                }
                catch(Exception e)
                {
                    log("-- Could not convert json-data : ", e.Message);
                }
            }
        }
    }
}
// ReSharper restore InconsistentNaming
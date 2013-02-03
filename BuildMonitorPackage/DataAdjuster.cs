using System;
using System.Collections.Generic;
using System.IO;
using BuildMonitor;

namespace BuildMonitorPackage
{
    /// <summary>
    /// This class is created to adjust json data due to a earlier bugg
    /// </summary>
    public static class DataAdjuster
    {
        public delegate void LogAction(string s, params object[] args);

        public static void VerifyJsonData(LogAction log)
        {
            var jsonText = File.ReadAllText(Settings.RepositoryPath);

            if (!DataVerifyer.IsValidData(jsonText))
            {
                log("-- Found invalid json-data in file: ", Settings.RepositoryPath);
                jsonText = String.Format("[{0}]", jsonText.Replace("}{", "},{"));

                try
                {
                    DataVerifyer.Dezerialize<IEnumerable<object>>(jsonText);
                    File.WriteAllText(Settings.RepositoryPath, jsonText);
                    log("-- Successfully converted invalid json data to valid");
                }
                catch (Exception e)
                {
                    log("-- Could not convert json-data : ", e.Message);
                }
            }
        }
    }
}
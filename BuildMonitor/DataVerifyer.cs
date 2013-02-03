using System;
using Newtonsoft.Json;

namespace BuildMonitor
{
    public static class DataVerifyer
    {
        /// <summary>
        /// JsonData is stored as [{}], that is an array of objects (builds)
        /// </summary>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public static bool IsValidData(string jsonText)
        {
            if (string.IsNullOrEmpty(jsonText)) return true;

            jsonText = jsonText.Replace(" ", "");

            return (jsonText.StartsWith("[{") && jsonText.EndsWith("}]") && jsonText.IndexOf("}{", StringComparison.InvariantCulture) == -1);
        }

        public static void Dezerialize<T>(string jsonText)
        {
            JsonConvert.DeserializeObject<T>(jsonText);
        }
    }
}

using System;

namespace BuildMonitor.LocalData
{
    public class RawBuildData
    {
        private readonly string rawData;

        public RawBuildData(string rawData)
        {
            this.rawData = rawData;
        }

        public bool IsValidData
        {
            get
            {
                if (string.IsNullOrEmpty(rawData)) return true;

                var jsonText = rawData.Replace(" ", "");

                return jsonText.StartsWith("[{") 
                        && jsonText.EndsWith("}]") 
                        && jsonText.IndexOf("}{", StringComparison.InvariantCulture) == -1
                        && jsonText.IndexOf(":newDate(", StringComparison.InvariantCulture) == -1;
            }
        }

        public string Data
        {
            get { return rawData; }
        }

        public string Fix()
        {
            var format = GetFormat();
            return string.Format(format, rawData.Replace("}{", "},{"));
        }

        private string GetFormat()
        {
            var format = "";
            if (!rawData.StartsWith("["))
            {
                format += "[";
            }
            format += "{0}";
            if (!rawData.EndsWith("]"))
            {
                format += "]";
            }
            return format;
        }
    }
}
using BuildMonitor.Domain;
using BuildMonitor.LocalData;

namespace BuildMonitorPackage
{
    public class DataAdjusterWithLogging : DataAdjuster
    {
        public delegate void LogAction(string s, params object[] args);

        public DataAdjusterWithLogging(IBuildRepository repository, LogAction log) : base(repository)
        {
            OnFoundInvalidData = data => log("-- Found invalid json-data in file: {0}", data.Source);
            OnFixedInvalidData = () => log("-- Successfully converted invalid json data to valid");
            OnCouldNotConvertData = e => log("-- Could not convert json-data : {0}", e.Message);
        }
    }
}
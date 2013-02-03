using System;
using BuildMonitor.Domain;

namespace BuildMonitor.LocalData
{
    public class DataAdjuster
    {
        private readonly IBuildRepository buildRepository;

        public DataAdjuster(IBuildRepository buildRepository)
        {
            this.buildRepository = buildRepository;
        }

        public Action<InvalidData> OnFoundInvalidData { get; set; }
        public Action OnFixedInvalidData { get; set; }
        public Action<Exception> OnCouldNotConvertData { get; set; }

        public void Adjust()
        {
            var rawData = buildRepository.GetRawData();

            var raw = new RawBuildData(rawData);
            if (!raw.IsValidData)
            {
                RaiseFoundInvalidData(raw.Data);
                try
                {
                    var fixedRawData = raw.Fix();
                    buildRepository.Save(fixedRawData);
                    RaiseFixedInvalidData();
                }
                catch (Exception e)
                {
                    RaiseCouldNotConvertData(e);
                }
            }
        }

        private void RaiseCouldNotConvertData(Exception e)
        {
            if (OnCouldNotConvertData != null)
                OnCouldNotConvertData(e);
        }

        private void RaiseFixedInvalidData()
        {
            if (OnFixedInvalidData != null)
            {
                OnFixedInvalidData();
            }
        }

        private void RaiseFoundInvalidData(string raw)
        {
            if (OnFoundInvalidData != null)
            {
                OnFoundInvalidData(new InvalidData {Data = raw, Source = buildRepository.Source});
            }
        }
    }
}
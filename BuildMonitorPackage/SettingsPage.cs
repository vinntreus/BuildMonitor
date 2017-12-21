using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace BuildMonitorPackage
{
    public class SettingsPage : DialogPage
    {
        [Category("Build Monitor")]
        [DisplayName("Output File Path")]
        [Description("Specifies the path to the file to which data is persisted. Can contain special folders, see https://msdn.microsoft.com/en-us/library/system.environment.specialfolder(v=vs.110).aspx, enclosed in %. Example: %ApplicationData%\\Build Monitor\\buildtimes.json (default)")]
        public string RepositoryPath { get; set; }

        public SettingsPage()
        {
            RepositoryPath = Settings.Instance.RawRepositoryPath;
        }

        protected override void OnApply(PageApplyEventArgs args)
        {
            base.OnApply(args);
            Settings.Instance.RawRepositoryPath = RepositoryPath;

            var output = new OutputWindowWrapper(ServiceProvider.GlobalProvider);
            output.WriteLine("New path to persist data: {0}", Settings.Instance.RepositoryPath);
        }
    }
}

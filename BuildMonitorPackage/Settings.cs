using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.Settings;

// ReSharper disable InconsistentNaming
namespace BuildMonitorPackage
{
    public static class OptionKey
    {
        public const string SolutionId = "bm_solution_id";
    }

    public class Settings {

        private static readonly string DefaultRepositoryPath = Path.Combine("%ApplicationData%", ApplicationFolderName, JsonFileName);

        private string rawRepositoryPath = DefaultRepositoryPath;

        private const string ApplicationFolderName = "Build Monitor";

        private const string JsonFileName = "buildtimes.json";

        private readonly WritableSettingsStore settingsStore;

        private const string CollectionPath = "BuildMonitor";

        public static Settings Instance { get; set; }

        public Settings(WritableSettingsStore settingsStore) {
            this.settingsStore = settingsStore;
            LoadSettings();
            CreateApplicationFolderIfNotExist();
        }

        public string RawRepositoryPath
        {
            get => rawRepositoryPath;
            set
            {
                if (rawRepositoryPath != value)
                {
                    rawRepositoryPath = value;
                    SaveSettings();
                }
            }
        }

        public string RepositoryPath => ExpandPath(rawRepositoryPath);

        private void CreateApplicationFolderIfNotExist()
        {
            string folder = Path.GetDirectoryName(RepositoryPath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (!File.Exists(RepositoryPath))
            {
                using (var f = File.Create(RepositoryPath)){}
            }
        }

        private void LoadSettings() {
            try
            {
                RawRepositoryPath = settingsStore.GetString(CollectionPath, "RepositoryPath", DefaultRepositoryPath);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }

        private void SaveSettings() {
            try
            {
                if (!settingsStore.CollectionExists(CollectionPath))
                {
                    settingsStore.CreateCollection(CollectionPath);
                }

                settingsStore.SetString(CollectionPath, "RepositoryPath", rawRepositoryPath);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Expands a path possibly starting with a <see cref="Environment.SpecialFolder"/>
        /// to a full path.
        /// </summary>
        private static string ExpandPath(string path)
        {
            if (!path.StartsWith("%"))
            {
                return path;
            }

            int splitIndex = path.IndexOf("%", 1, StringComparison.InvariantCulture) + 1;
            string maybeSpecialFolder = path.Substring(0, splitIndex).Trim('%');
            string rest = path.Substring(splitIndex);
            while (rest.StartsWith(Path.DirectorySeparatorChar.ToString()))
            {
                // The remaining path cannot start with a rooted path as that
                // will "break" Path.Combine.
                rest = rest.Substring(1);
            }

            foreach (var @enum in Enum.GetNames(typeof(Environment.SpecialFolder)))
            {
                if (@enum.Equals(maybeSpecialFolder, StringComparison.InvariantCultureIgnoreCase))
                {
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), rest);
                }
            }

            return path;
        }
    }
}
// ReSharper restore InconsistentNaming
using System;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;

namespace BuildMonitorPackage {
    internal class OutputWindowWrapper {
        private readonly OutputWindowPane outputWindowPane;

        public OutputWindowWrapper(IServiceProvider serviceContainer)
        {
            var dte = serviceContainer.GetService(typeof(SDTE)) as DTE;
            var outputWindow = (OutputWindow)dte.Windows.Item(EnvDTEConstants.vsWindowKindOutput).Object;

            foreach (OutputWindowPane pane in outputWindow.OutputWindowPanes)
            {
                if (pane.Name == "Build monitor")
                {
                    outputWindowPane = pane;
                }
            }

            if (outputWindowPane == null)
            {
                outputWindowPane = outputWindow.OutputWindowPanes.Add("Build monitor");
            }
        }

        public void Write(string text) {
            outputWindowPane.OutputString(text);
        }

        public void Write(string format, params object[] args) {
            outputWindowPane.OutputString(string.Format(format, args));
        }

        public void WriteLine(string text) {
            Write(text + Environment.NewLine);
        }

        public void WriteLine(string format, params object[] args)
        {
            Write(format + Environment.NewLine, args);
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using BuildMonitor.Domain;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Constants = EnvDTE.Constants;

namespace BuildMonitor
{
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.guidBuildMonitorPkgString)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [ProvideAutoLoad("{f1536ef8-92ec-443c-9ed7-fdadf150da82}")]
    public class BuildMonitorPackage : Package, IVsUpdateSolutionEvents2
    {
        private DTE dte;
        private readonly Monitor monitor;
        private Domain.Solution solution;

        private IVsSolutionBuildManager2 sbm;
        private uint updateSolutionEventsCookie;
        private OutputWindowPane outputWindowPane;
        private SolutionEvents events;
        private IVsSolution2 vsSolution;

        public BuildMonitorPackage()
        {
            AddOptionKey("bm_solution_id");
            Settings.CreateApplicationFolderIfNotExist();

            var factory = new BuildFactory();
            var repository = new BuildRepository(Settings.RepositoryPath);

            monitor = new Monitor(factory, repository);
        }

        protected override void Initialize()
        {
            base.Initialize();

            // Get solution build manager
            sbm = ServiceProvider.GlobalProvider.GetService(typeof(SVsSolutionBuildManager)) as IVsSolutionBuildManager2;
            if (sbm != null)
            {
                sbm.AdviseUpdateSolutionEvents(this, out updateSolutionEventsCookie);
            }
            var serviceContainer = this as IServiceContainer;
            dte = serviceContainer.GetService(typeof(SDTE)) as DTE;
            if (dte == null)
                return;

            // Must hold a reference to the solution events object or the events wont fire, garbage collection related
            events = dte.Events.SolutionEvents;
            events.Opened += Solution_Opened;

            CreateOutputWindowPane();

            outputWindowPane.OutputString("Build monitor initialized\n");
            outputWindowPane.OutputString(string.Format("Path to persist data: {0}\n", Settings.RepositoryPath));

            monitor.SolutionBuildFinished = b =>
            {
                outputWindowPane.OutputString(string.Format("[{0}] Time Elapsed: {1}ms  \t\t", b.SessionBuildCount, b.SolutionBuildTime));
                outputWindowPane.OutputString(string.Format("Session build time: {0}ms\n", b.SessionMillisecondsElapsed));
            };
        }

        private void CreateOutputWindowPane()
        {
            var outputWindow = (OutputWindow)dte.Windows.Item(Constants.vsWindowKindOutput).Object;
            outputWindowPane = outputWindow.OutputWindowPanes.Add("Build monitor");
        }

        private void Solution_Opened()
        {
            vsSolution = ServiceProvider.GlobalProvider.GetService(typeof(SVsSolution)) as IVsSolution2;
            solution = new Domain.Solution(Guid.NewGuid(), GetSolutionName());
            outputWindowPane.OutputString(string.Format("\nSolution loaded:  \t{0}\n", solution.Name));
            outputWindowPane.OutputString(string.Format("{0}\n", 60.Times("-")));
        }

        private string GetSolutionName()
        {
            object solutionName;
            vsSolution.GetProperty((int)__VSPROPID.VSPROPID_SolutionBaseName, out solutionName);
            return (string)solutionName;
        }

        int IVsUpdateSolutionEvents.UpdateSolution_Begin(ref int pfCancelUpdate)
        {
            // This method is called when the entire solution starts to build.
            monitor.SolutionBuildStart(solution);

            return VSConstants.S_OK;
        }


        int IVsUpdateSolutionEvents2.UpdateProjectCfg_Begin(IVsHierarchy pHierProj, IVsCfg pCfgProj, IVsCfg pCfgSln, uint dwAction, ref int pfCancel)
        {
            // This method is called when a specific project begins building.
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents2.UpdateProjectCfg_Done(IVsHierarchy pHierProj, IVsCfg pCfgProj, IVsCfg pCfgSln, uint dwAction, int fSuccess, int fCancel)
        {
            // This method is called when a specific project finishes building.
            return VSConstants.S_OK;
        }


        int IVsUpdateSolutionEvents.UpdateSolution_Done(int fSucceeded, int fModified, int fCancelCommand)
        {
            // This method is called when the entire solution is done building.
            monitor.SolutionBuildStop();
            return VSConstants.S_OK;
        }


        int IVsUpdateSolutionEvents2.UpdateSolution_StartUpdate(ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents2.UpdateSolution_Cancel()
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents2.OnActiveProjectCfgChange(IVsHierarchy pIVsHierarchy)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents2.UpdateSolution_Begin(ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents2.UpdateSolution_Done(int fSucceeded, int fModified, int fCancelCommand)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents.UpdateSolution_StartUpdate(ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents.UpdateSolution_Cancel()
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents.OnActiveProjectCfgChange(IVsHierarchy pIVsHierarchy)
        {
            return VSConstants.S_OK;
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            // Unadvise all events
            if (sbm != null && updateSolutionEventsCookie != 0)
                sbm.UnadviseUpdateSolutionEvents(updateSolutionEventsCookie);
        }
    }

    public static class IntExtensions
    {
        public static string Times(this int i, string s)
        {
            return string.Join("", Enumerable.Range(0, i).Select(d => s));
        }
    }
}

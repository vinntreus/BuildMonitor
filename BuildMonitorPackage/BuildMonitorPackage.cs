using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using BuildMonitor;
using BuildMonitor.Domain;
using BuildMonitor.UI;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using System.Data.SqlClient;
using System.Data;
using System.Security.Principal;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;

namespace BuildMonitorPackage
{
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.guidBuildMonitorPackagePkgString)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(SettingsPage), "Build Monitor", "General", 0, 0, true)]
    sealed class BuildMonitorPackage : Package, IVsUpdateSolutionEvents2
    {
        private DTE dte;
        private Monitor monitor;
        private DataAdjusterWithLogging dataAdjuster;
        private BuildMonitor.Domain.Solution solution;

        private IVsSolutionBuildManager2 sbm;
        private uint updateSolutionEventsCookie;
        private SolutionEvents events;
        private IVsSolution2 vsSolution;
        private OutputWindowWrapper output;

        protected override void Initialize()
        {
            base.Initialize();

            output = new OutputWindowWrapper(this);

            SettingsManager settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            WritableSettingsStore settingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            Settings.Instance = new Settings(settingsStore);

            var factory = new BuildFactory();
            var repository = new BuildRepository(Settings.Instance.RepositoryPath);

            monitor = new Monitor(factory, repository);
            dataAdjuster = new DataAdjusterWithLogging(repository, output.WriteLine);

            //if invalid data, adjust it
            dataAdjuster.Adjust();


            // Get solution build manager
            sbm = ServiceProvider.GlobalProvider.GetService(typeof(SVsSolutionBuildManager)) as IVsSolutionBuildManager2;
            if (sbm != null)
            {
                sbm.AdviseUpdateSolutionEvents(this, out updateSolutionEventsCookie);
            }

            // Must hold a reference to the solution events object or the events wont fire, garbage collection related
            events = GetDTE().Events.SolutionEvents;
            events.Opened += Solution_Opened;
            GetDTE().Events.BuildEvents.OnBuildBegin += Build_Begin;

            output.WriteLine("Build monitor initialized");
            output.WriteLine("Path to persist data: {0}", Settings.Instance.RepositoryPath);

            monitor.SolutionBuildFinished = b =>
            {
                output.Write("[{0}] Time Elapsed: {1} \t\t", b.SessionBuildCount, b.SolutionBuildTime.ToTime());
                output.WriteLine("Session build time: {0}\n", b.SessionMillisecondsElapsed.ToTime());
                output.WriteLine("Rebuild All: {0}\n", b.SolutionBuild.IsRebuildAll);
                System.Threading.Tasks.Task.Factory.StartNew(() => SaveToDatabase(b));
            };

            monitor.ProjectBuildFinished = b => output.WriteLine(" - {0}\t-- {1} --", b.MillisecondsElapsed.ToTime(), b.ProjectName);
        	AnalyseBuildTimesCommand.Initialize(this);
		}

        private void SaveToDatabase(SolutionBuildData b)
        {
            try
            {
                var conn = new SqlConnection("Server=kl-sql-005;DataBase=RESSoftware;Integrated Security=SSPI");
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.AddBuildTime", conn);
                cmd.Parameters.AddWithValue("IsRebuildAll", b.SolutionBuild.IsRebuildAll ? 1 : 0);
                cmd.Parameters.AddWithValue("SolutionName", b.SolutionName);
                cmd.Parameters.AddWithValue("BuildDateTime", DateTime.Now);
                cmd.Parameters.AddWithValue("TimeInMilliseconds", b.SolutionBuildTime);
                cmd.Parameters.AddWithValue("NT4Name", WindowsIdentity.GetCurrent().Name);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                if (conn != null) conn.Close();
            }
            catch // ignore exceptions, its not a big problem if we can't log the build time
            { }
        }

        private void Solution_Opened()
        {
            solution = new BuildMonitor.Domain.Solution { Name = GetSolutionName() };
            output.WriteLine("\nSolution loaded:  \t{0}", solution.Name);
            output.WriteLine(new string('-', 60));
        }

        #region Get objects from vs

        private DTE GetDTE()
        {
            if (dte == null)
            {
                var serviceContainer = this as IServiceContainer;
                dte = serviceContainer.GetService(typeof(SDTE)) as DTE;
            }
            return dte;
        }

        private void SetVsSolution()
        {
            if (vsSolution == null)
                vsSolution = ServiceProvider.GlobalProvider.GetService(typeof(SVsSolution)) as IVsSolution2;
        }

        private string GetSolutionName()
        {
            SetVsSolution();
            object solutionName;
            vsSolution.GetProperty((int)__VSPROPID.VSPROPID_SolutionBaseName, out solutionName);
            return (string)solutionName;
        }

        private IProject GetProject(IVsHierarchy pHierProj)
        {
            object n;
            pHierProj.GetProperty((uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_Name, out n);
            var name = n as string;

            Guid id;
            vsSolution.GetGuidOfProject(pHierProj, out id);

            return new BuildMonitor.Domain.Project { Name = name, Id = id };
        }

        #endregion

        // this event is called on build begin and let's us find out whether it is a full rebuild or a partial
        private void Build_Begin(vsBuildScope scope, vsBuildAction action)
        {
            monitor.SetIsRebuildAll(action == vsBuildAction.vsBuildActionRebuildAll);
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
            var project = GetProject(pHierProj);
            monitor.ProjectBuildStart(project);

            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents2.UpdateProjectCfg_Done(IVsHierarchy pHierProj, IVsCfg pCfgProj, IVsCfg pCfgSln, uint dwAction, int fSuccess, int fCancel)
        {
            // This method is called when a specific project finishes building.
            var project = GetProject(pHierProj);
            monitor.ProjectBuildStop(project);

            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents.UpdateSolution_Done(int fSucceeded, int fModified, int fCancelCommand)
        {
            // This method is called when the entire solution is done building.
            monitor.SolutionBuildStop();

            return VSConstants.S_OK;
        }

        #region empty impl. of solution events interface

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

        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            // Unadvise all events
            if (sbm != null && updateSolutionEventsCookie != 0)
                sbm.UnadviseUpdateSolutionEvents(updateSolutionEventsCookie);
        }
    }
}
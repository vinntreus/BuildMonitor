using System;
using System.Linq;
using System.Collections.Generic;

namespace BuildMonitor.Domain
{
    public class Monitor
    {
        private readonly IBuildFactory buildFactory;
        private readonly IBuildRepository buildRepository;
        private ISolutionBuild solutionBuild;
        private int buildCount;
        private long sessionMillisecondsElapsed;

        private readonly ISet<IProjectBuild> runningProjects;

        public Monitor(IBuildFactory buildFactory, IBuildRepository buildRepository)
        {
            this.buildFactory = buildFactory;
            this.buildRepository = buildRepository;

            runningProjects = new HashSet<IProjectBuild>(new ProjectBuildComparer());

            SolutionBuildFinished = d => { };
            ProjectBuildFinished = b => { };
        }

        public Action<SolutionBuildData> SolutionBuildFinished;
        public Action<ProjectBuildData> ProjectBuildFinished;
        private bool _isRebuildAll = false;

        public void SetIsRebuildAll(bool isRebuildAll)
        {
            _isRebuildAll = isRebuildAll;
        }

        public void SolutionBuildStart(ISolution solution)
        {
            if(solutionBuild != null && solutionBuild.IsRunning)
                throw new InvalidOperationException("There is already a build running");

            solutionBuild = buildFactory.CreateSolutionBuild(solution);

            solutionBuild.Start();
        }

        public void SolutionBuildStop()
        {
            if (solutionBuild != null && solutionBuild.IsRunning)
            {
                solutionBuild.Stop();
                solutionBuild.IsRebuildAll = _isRebuildAll;
                buildRepository.Save(solutionBuild);

                SolutionBuildFinished(new SolutionBuildData(solutionBuild, ++buildCount, sessionMillisecondsElapsed += solutionBuild.MillisecondsElapsed));
            }
        }

        public void ProjectBuildStart(IProject project)
        {
            var projectBuild = buildFactory.CreateProjectBuild(project);

            if (runningProjects.Add(projectBuild))
            {
                projectBuild.Start();
            }
        }

        public void ProjectBuildStop(IProject project)
        {
            var projectBuild = runningProjects.First(b => b.Project.Id == project.Id);

            projectBuild.Stop();
            runningProjects.Remove(projectBuild);
            solutionBuild.AddProject(projectBuild);

            ProjectBuildFinished(new ProjectBuildData(projectBuild.Project.Name, projectBuild.MillisecondsElapsed));
        }
    }

    public class ProjectBuildComparer : IEqualityComparer<IProjectBuild>
    {
        public bool Equals(IProjectBuild x, IProjectBuild y)
        {
            return x.Project.Id == y.Project.Id;
        }

        public int GetHashCode(IProjectBuild obj)
        {
            return obj.Project.Id.GetHashCode();
        }
    }

    public class ProjectBuildData
    {
        public ProjectBuildData(string projectName, long millisecondsElapsed)
        {
            ProjectName = projectName;
            MillisecondsElapsed = millisecondsElapsed;
        }

        public string ProjectName { get; private set; }
        public long MillisecondsElapsed { get; private set; }
    }
}
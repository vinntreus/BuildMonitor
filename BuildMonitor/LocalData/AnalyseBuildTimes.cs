using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildMonitor.LocalData
{
    public class AnalyseBuildTimes
    {
        public IBuildTimes Calculate(string json)
        {
            var solutionMonths = new Dictionary<SolutionMonth, TimeSpan>();
            var solutions = new Dictionary<string, TimeSpan>();
            int total = 0;

            foreach (var jsonSolutionBuildTime in ReadJSON(json))
            {
                UpdateSolutionMonths(jsonSolutionBuildTime, solutionMonths);
                UpdateSolutions(jsonSolutionBuildTime, solutions);
                total += jsonSolutionBuildTime.Time; 
            }

            return new BuildTimes(TimeSpan.FromMilliseconds(total), solutions, solutionMonths);
        }

        private static IEnumerable<JSONSolutionTimes> ReadJSON(string json)
        {
            var jsonSerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            jsonSerializerSettings.Converters.Add(new IsoDateTimeConverter());
            var jsonSolutionBuildTimes = JsonConvert.DeserializeObject<IEnumerable<JSONSolutionTimes>>(json, jsonSerializerSettings);
            return jsonSolutionBuildTimes.Where(b => b.Name != null);
        }

        private void UpdateSolutions(JSONSolutionTimes jsonSolutionBuildTime, Dictionary<string, TimeSpan> solutions)
        {
            if (solutions.ContainsKey(jsonSolutionBuildTime.Name))
                solutions[jsonSolutionBuildTime.Name] = solutions[jsonSolutionBuildTime.Name] + TimeSpan.FromMilliseconds(jsonSolutionBuildTime.Time);
            else
                solutions.Add(jsonSolutionBuildTime.Name, TimeSpan.FromMilliseconds(jsonSolutionBuildTime.Time));
        }

        private void UpdateSolutionMonths(JSONSolutionTimes jsonSolutionBuildTime, Dictionary<SolutionMonth, TimeSpan> solutionMonths)
        {
            var solutionMonth = new SolutionMonth( solution: jsonSolutionBuildTime.Name, month: jsonSolutionBuildTime.Start.Month, year: jsonSolutionBuildTime.Start.Year );

            if (solutionMonths.ContainsKey(solutionMonth))
                solutionMonths[solutionMonth] = solutionMonths[solutionMonth] + TimeSpan.FromMilliseconds(jsonSolutionBuildTime.Time);
            else
                solutionMonths.Add(solutionMonth, TimeSpan.FromMilliseconds(jsonSolutionBuildTime.Time));
        }
    }
}
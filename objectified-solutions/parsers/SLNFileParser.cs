using System.Collections.Generic;
using System.IO;
using objectified_solutions.project;
using objectified_solutions.solution;

namespace objectified_solutions.parsers {
    public class SLNFileParser {
        public static SolutionObject Parse(string slnFile) {
            List<string> lines = new List<string>(File.ReadAllLines(slnFile));
            SolutionObject slnObj = new SolutionObject(slnFile, lines);
            
            List<string> projectlines = ApplyFilter(lines, "Project", null);
            List<string> csprojlines = ApplyFilter(projectlines, null, "csproj");

            List<ProjectObject> projects = new List<ProjectObject>();
            foreach(string line in csprojlines) {
                CSProjLine csprojline = new CSProjLine(line);
                ProjectObject project = new ProjectObject { Name = csprojline.Name, RelativePath = csprojline.RelativePath };
                projects.Add(project);
            }

            slnObj.Projects = projects;
            return slnObj;
        }

        private static List<string> ApplyFilter(List<string> lines, string startsWith, string contains) {
            List<string> filteredLines = new List<string>();
            if(startsWith == null) {
                foreach(string line in lines) {
                    if(line.Contains(contains)) {
                        filteredLines.Add(line);
                    }
                }
            } else {
                foreach(string line in lines) {
                    if(line.StartsWith(startsWith)) {
                        filteredLines.Add(line);
                    }
                }
            }

            return filteredLines;
        }
    }
}
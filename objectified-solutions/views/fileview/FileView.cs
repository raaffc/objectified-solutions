#region Licence
/*
 * The MIT License
 *
 * Copyright (c) 2008-2013, Andrew Gray
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion
using System.Collections.Generic;
using objectified_solutions.parsers;
using objectified_solutions.views.fileview.project;

namespace objectified_solutions.views.fileview {
    public class FileView {
        public List<ProjectObject> Projects { get; set; }

        public FileView(List<string> lines, string rootPath) {
            List<string> projectLines = Common.ApplyFilter(lines, Constants.PROJECT, null);
            List<string> csprojLines = Common.ApplyFilter(projectLines, null, Constants.CSPROJ);
            List<string> wixprojLines = Common.ApplyFilter(projectLines, null, Constants.WIXPROJ);
            
            Projects = new List<ProjectObject>();
            foreach (string line in csprojLines) {
                ProjectObject project = AddProject(line);
                ProjFileParser.Parse(rootPath + project.RelativePath, project);
            }
            foreach (string line in wixprojLines) {
                ProjectObject project = AddProject(line);
                ProjFileParser.Parse(rootPath + project.RelativePath, project);
            }
        }

        private ProjectObject AddProject(string line) {
            ProjectLine projectLine = new ProjectLine(line);
            ProjectObject project = new ProjectObject { Name = projectLine.Name,
                                                        RelativePath = projectLine.RelativePath,
                                                        ProjectGuid = projectLine.ProjectGuid };
            Projects.Add(project);
            return project;
        }

        public string GetProjectName(string projectGuid) {
            string name = string.Empty;
            foreach(ProjectObject project in Projects) {
                if(project.ProjectGuid.Equals(projectGuid)) {
                    name = project.Name;
                    break;
                }
            }
            return name;
        }
    }
}
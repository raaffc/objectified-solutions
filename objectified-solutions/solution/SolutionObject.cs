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
using System.IO;
using objectified_solutions.parsers;
using objectified_solutions.project;

namespace objectified_solutions.solution {
    public class SolutionObject {
        public string FormatVersion { get; set; }
        public string VSVersion { get; set; }
        public string Name { get; set; }
        public string RootPath { get; set; }
        public List<ProjectObject> Projects { get; set; }
        public SolutionView SolutionView { get; set; }

        public SolutionObject(string slnFile) {
            List<string> lines = new List<string>(File.ReadAllLines(slnFile));
            RootPath = GetRootPath(slnFile);
            Name = GetName(slnFile);
            FormatVersion = GetFormatVersion(lines[0]);
            VSVersion = lines[1].Substring(2);

            List<string> projectlines = ApplyFilter(lines, Constants.PROJECT, null);
            List<string> csprojlines = ApplyFilter(projectlines, null, Constants.CSPROJ);

            Projects = new List<ProjectObject>();
            foreach(string line in csprojlines) {
                CSProjLine csprojline = new CSProjLine(line);
                ProjectObject project = new ProjectObject { Name = csprojline.Name, RelativePath = csprojline.RelativePath };
                CSProjFileParser.Parse(RootPath + project.RelativePath, project);
                Projects.Add(project);
            }
        }

        private string GetRootPath(string slnFile) {
            int lastSlash = slnFile.LastIndexOf(Constants.BACKSLASH);
            return slnFile.Remove(lastSlash + 1);
        }

        private string GetName(string slnFile) {
            int lastSlash = slnFile.LastIndexOf(Constants.BACKSLASH);
            return slnFile.Substring(lastSlash + 1);
        }

        private string GetFormatVersion(string firstLine) {
            string[] tokens = firstLine.Split(Constants.SPACE_CHAR);
            return tokens[tokens.Length - 1];
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
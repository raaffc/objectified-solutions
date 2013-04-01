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
using objectified_solutions.views.solutionview.solution;

namespace objectified_solutions.views.solutionview {
    public class SolutionView {
        public List<SolutionFolderObject> SolutionFolders { get; set; }
        public List<string> ProjectsNotInASolutionFolder { get; set; }

        public SolutionView(List<string> lines, string rootPath) {
            List<string> allProjectLines = Common.ApplyFilter(lines, Constants.PROJECT, null);
            List<string> solutionFolderLines = Common.GetSolutionFolders(allProjectLines);
            List<string> csprojLines = Common.ApplyFilter(allProjectLines, null, Constants.CSPROJ);
            List<string> nestedProjectsSection = Common.GetNestedProjects(lines);

            SolutionFolders = new List<SolutionFolderObject>();
            foreach(string line in solutionFolderLines) {
                SolutionFolderLine solutionFolderLine = new SolutionFolderLine(line);
                SolutionFolderObject solutionFolder = new SolutionFolderObject(solutionFolderLine, 
                                                                               nestedProjectsSection,
                                                                               csprojLines,
                                                                               solutionFolderLines);
                SolutionFolders.Add(solutionFolder);
            }

            ProjectsNotInASolutionFolder = BuildListOfProjectsNotInASolutionFolder(csprojLines, SolutionFolders);
        }

        private List<string> BuildListOfProjectsNotInASolutionFolder(List<string> csprojLines, List<SolutionFolderObject> solutionFolders) {
            List<string> unNestedProjects = new List<string>();
            List<string> allNestedProjects = GetAllNestedProjectGuids(solutionFolders);

            foreach (string line in csprojLines) {
                CSProjLine csprojLine = new CSProjLine(line);
                if(!IsNested(allNestedProjects, csprojLine.ProjectGuid)) {
                    unNestedProjects.Add(csprojLine.ProjectGuid);
                }
            }
            return unNestedProjects;
        }

        private bool IsNested(List<string> allNestedProjects, string projectGuid) {
            foreach(string nestedProjectGuid in allNestedProjects) {
                if(projectGuid.Equals(nestedProjectGuid)) {
                    return true;
                }
            }
            return false;
        }

        private List<string>  GetAllNestedProjectGuids(List<SolutionFolderObject> solutionFolders) {
            List<string> list = new List<string>();
            foreach(SolutionFolderObject folder in solutionFolders) {
                list.AddRange(GetAllNestedProjectGuids(folder.NestedFolders));
                foreach(string projectGuid in folder.NestedProjects) {
                    list.Add(projectGuid);
                }
            }
            return list;
        }
    }
}
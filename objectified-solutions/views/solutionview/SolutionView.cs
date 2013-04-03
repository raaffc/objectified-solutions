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
using objectified_solutions.views.solutionview.project;
using objectified_solutions.views.solutionview.solution;

namespace objectified_solutions.views.solutionview {
    public class SolutionView {
        public List<SolutionFolderObject> SolutionFolders { get; set; }
        public List<string> ProjectsNotInASolutionFolder { get; set; }

        public SolutionView(List<string> lines, string rootPath) {
            List<string> allProjectLines = Common.ApplyFilter(lines, Constants.PROJECT, null);
            List<string> csprojLines = Common.ApplyFilter(allProjectLines, null, Constants.CSPROJ);

            List<string> nestedProjectLines = Common.GetNestedProjectsSectionAsLines(lines);
            NestedProjectCollection nestedProjectCollection = new NestedProjectCollection(nestedProjectLines);
            SolutionFolders = BuildSolutionFoldersSkeleton(nestedProjectCollection.RootParents);

            //Fill out SolutionFolders
            foreach(NestedProject nestedProject in nestedProjectCollection.AllLines) {
                if (nestedProjectCollection.IsRootFolder(nestedProject)) { //Check the parent
                    SolutionFolderObject sfo = GetRootFolder(nestedProject.Parent);
                    if(nestedProjectCollection.IsNestedProject(nestedProject.Child)) {
                        if (sfo.NestedProjects == null) {
                            sfo.NestedProjects = new List<string>();
                        }
                        sfo.NestedProjects.Add(nestedProject.Child);
                    } else {
                        if (sfo.NestedFolders == null) {
                            sfo.NestedFolders = new List<SolutionFolderObject>();
                        }
                        sfo.NestedFolders.Add(new SolutionFolderObject { FolderGuid = nestedProject.Child });
                    }
                } else {
                    //Parent is not a Root Project
                    SolutionFolderObject sfo2 = FindFolder(SolutionFolders, nestedProject.Parent);
                    if (nestedProjectCollection.IsNestedProject(nestedProject.Child)) {
                        if (sfo2.NestedProjects == null) {
                            sfo2.NestedProjects = new List<string>();
                        }
                        sfo2.NestedProjects.Add(nestedProject.Child);
                    } else {
                        if (sfo2.NestedFolders == null) {
                            sfo2.NestedFolders = new List<SolutionFolderObject>();
                        }
                        sfo2.NestedFolders.Add(new SolutionFolderObject { FolderGuid = nestedProject.Child });
                    }
                }
            }

            ProjectsNotInASolutionFolder = BuildListOfProjectsNotInASolutionFolder(csprojLines, nestedProjectCollection.NestedProjects);
        }
        
        private SolutionFolderObject FindFolder(List<SolutionFolderObject> folders, string item) {
            foreach (SolutionFolderObject sfo in folders) {
                if (sfo.NestedFolders != null) {
                    return FindFolder(sfo.NestedFolders, item);
                }
                if(item.Equals(sfo.FolderGuid)) {
                    return sfo;
                }
            }
            return null;
        }

        private SolutionFolderObject GetRootFolder(string item) {
            foreach(SolutionFolderObject sfo in SolutionFolders) {
                if(item.Equals(sfo.FolderGuid)) {
                    return sfo;
                }
            }
            return null;
        }

        private List<SolutionFolderObject> BuildSolutionFoldersSkeleton(List<string> rootParents) {
            List<SolutionFolderObject> temp = new List<SolutionFolderObject>();
            foreach(string rootParent in rootParents) {
                SolutionFolderObject sfo = new SolutionFolderObject {FolderGuid = rootParent};
                temp.Add(sfo); 
            }
            return temp;
        }

        private List<string> BuildListOfProjectsNotInASolutionFolder(List<string> csprojLines, List<string> nestedProjects) {
            List<string> unNestedProjects = new List<string>();
            foreach (string line in csprojLines) {
                CSProjLine csprojLine = new CSProjLine(line);
                if(!nestedProjects.Contains(csprojLine.ProjectGuid)) {
                    unNestedProjects.Add(csprojLine.ProjectGuid);
                }
            }
            return unNestedProjects;
        }
    }
}
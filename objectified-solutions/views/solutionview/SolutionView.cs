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

        public SolutionView(List<string> lines) {
            var allProjectLines = Common.ApplyFilter(lines, Constants.PROJECT, null);
            var csprojLines = Common.ApplyFilter(allProjectLines, null, Constants.CSPROJ);

            var nestedProjectLines = Common.GetNestedProjectsSectionAsLines(lines);
            NestedProjectCollection.PopulateCollection(nestedProjectLines);
            SolutionFolders = BuildSolutionFoldersSkeleton(NestedProjectCollection.RootParents, allProjectLines);

            //Fill out SolutionFolders
            foreach(var nestedProject in NestedProjectCollection.AllLines) {
                var sfo = NestedProjectCollection.IsRootFolder(nestedProject)
                              ? GetRootFolder(nestedProject.Parent)
                              : FindFolder(SolutionFolders, nestedProject.Parent);
                if(sfo != null) {
                    AddChildIntoStructure(sfo, nestedProject.Child, allProjectLines);
                }
            }

            ProjectsNotInASolutionFolder = BuildListOfProjectsNotInASolutionFolder(csprojLines);
        }

        private static List<SolutionFolderObject> BuildSolutionFoldersSkeleton(IEnumerable<string> rootParents, List<string> allProjectLines) {
            //This needs to create all nested folders not just the rootParents. Utilise NestedProjectCollection.NestedFolders
            Common.CheckNull(rootParents, "rootParents");
            Common.CheckNull(allProjectLines, "allProjectLines");
            //int foldersToBePlaced = NestedProjectCollection.NestedFolders.Count;
            //int foldersPlaced = 0;

            var sfos = new List<SolutionFolderObject>();
            foreach (var rootParent in rootParents) {
                var sfo = new SolutionFolderObject { FolderGuid = rootParent, Name = GetName(allProjectLines, rootParent) };
                sfos.Add(sfo);
            }

            //var tempCopyOfNestedFolders = new List<string>(NestedProjectCollection.NestedFolders);

            //while (foldersPlaced != foldersToBePlaced) {
            //    foreach(var nestedFolder in tempCopyOfNestedFolders) {
            //        PlaceNestedFolderInStructure(nestedFolder, sfos);
            //    }
            //}
            return sfos;
        }

        private void AddChildIntoStructure(SolutionFolderObject sfo, string child, IEnumerable<string> allProjectLines) {
            if(NestedProjectCollection.IsNestedProject(child)) {
                if(sfo.NestedProjects == null) {
                    sfo.NestedProjects = new List<string>();
                }
                sfo.NestedProjects.Add(child);
            } else {
                if(sfo.NestedFolders == null) {
                    sfo.NestedFolders = new List<SolutionFolderObject>();
                }
                sfo.NestedFolders.Add(new SolutionFolderObject {FolderGuid = child, Name = GetName(allProjectLines, child)});
            }
        }

        private static string GetName(IEnumerable<string> allProjectLines, string folderGuid) {
            string name = null;
            foreach(var line in allProjectLines) {
                name = ExtractName(line);
                if(ExtractGuid(line).Equals(folderGuid)) {
                    break;
                }
            }
            return name;
        }

        private static string ExtractName(string line) {
            var tokens = Common.Split(line);
            return Common.TrimToken(tokens[2], 1, 3);
        }

        private static string ExtractGuid(string line) {
            var tokens = Common.Split(line);
            return Common.TrimToken(tokens[4], 2, 4);
        }

        private static SolutionFolderObject FindFolder(IEnumerable<SolutionFolderObject> folders, string item) {
            SolutionFolderObject result = null;
            foreach(var sfo in folders) {
                if (item.Equals(sfo.FolderGuid)) {
                    return sfo;
                }
                if(sfo.NestedFolders != null) {
                    result = FindFolder(sfo.NestedFolders, item);
                    if(result != null) {
                        break;
                    }
                }
            }
            return result;
        }

        private SolutionFolderObject GetRootFolder(string item) {
            foreach(var sfo in SolutionFolders) {
                if(item.Equals(sfo.FolderGuid)) {
                    return sfo;
                }
            }
            return null;
        }

        private List<string> BuildListOfProjectsNotInASolutionFolder(IEnumerable<string> csprojLines) {
            var unNestedProjects = new List<string>();
            foreach(var line in csprojLines) {
                var csprojLine = new SolutionFileProjectEntry(line);
                if(!NestedProjectCollection.NestedProjects.Contains(csprojLine.ProjectGuid)) {
                    unNestedProjects.Add(csprojLine.ProjectGuid);
                }
            }
            return unNestedProjects;
        }
    }
}

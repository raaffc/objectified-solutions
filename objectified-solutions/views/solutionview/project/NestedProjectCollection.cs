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

namespace objectified_solutions.views.solutionview.project {
    public class NestedProjectCollection {
        public List<NestedProject> AllLines { get; set; }
        public List<string> Children { get; set; }
        public List<string> Parents { get; set; }
        public List<string> RootParents { get; set; }
        public List<string> NestedProjects { get; set; }
        public List<string> NestedFolders { get; set; }

        public NestedProjectCollection(List<string> nestedProjects) {
            AllLines = new List<NestedProject>();
            Children = new List<string>();
            Parents = new List<string>();
            foreach (string line in nestedProjects) {
                string trimmedLine = line.Trim();
                string[] tokens = trimmedLine.Split(Constants.SPACE_CHAR);
                NestedProject nestedProject = new NestedProject { Parent = TrimProjectGuid(tokens[2]), Child = TrimProjectGuid(tokens[0]) };
                AllLines.Add(nestedProject);
                if(!Children.Contains(nestedProject.Child)) {
                    Children.Add(nestedProject.Child);
                }
                if(!Parents.Contains(nestedProject.Parent)) {
                    Parents.Add(nestedProject.Parent);
                }
            }

            RootParents = new List<string>();
            foreach(string parent in Parents) {
                if(!Children.Contains(parent)) {
                    RootParents.Add(parent);
                }
            }

            NestedProjects = new List<string>();
            NestedFolders = new List<string>();
            foreach(string child in Children) {
                if(!Parents.Contains(child)) {
                    NestedProjects.Add(child);
                } else {
                    NestedFolders.Add(child);
                }
            }
        }

        public bool IsChild(string item) {
            return Children.Contains(item);
        }

        public bool IsParent(string item) {
            return Parents.Contains(item);
        }

        private string TrimProjectGuid(string s) {
            return s.Substring(1, s.Length - 2);
        }        
    }
}
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

namespace objectified_solutions {
    public class Common {
        public static List<string> ApplyFilter(List<string> lines, string startsWith, string contains) {
            List<string> filteredLines = new List<string>();
            if (startsWith == null) {
                foreach (string line in lines) {
                    if (line.Contains(contains)) {
                        filteredLines.Add(line);
                    }
                }
            } else {
                foreach (string line in lines) {
                    if (line.StartsWith(startsWith)) {
                        filteredLines.Add(line);
                    }
                }
            }
            return filteredLines;
        }

        public static List<string> GetSolutionFolders(List<string> lines) {
            List<string> solutionFolders = new List<string>();
            foreach (string line in lines) {
                if (!line.Contains(Constants.CSPROJ) && !line.Contains(Constants.WIXPROJ) && !line.Contains(Constants.DTPROJ)) {
                    solutionFolders.Add(line);
                }
            }
            return solutionFolders;
        }

        public static List<string> GetNestedProjects(List<string> lines) {
            List<string> nestedProjects = new List<string>();
            bool sectionFound = false;
            foreach (string line in lines) {
                if(line.Contains(Constants.NESTED_PROJECTS)) {
                    sectionFound = true;
                    continue;
                }
                if(sectionFound) {
                    if (line.Contains("EndGlobalSection")) {
                        sectionFound = false;
                    } else {
                        nestedProjects.Add(line);
                    }
                }
            }
            return nestedProjects;
        } 
    }
}
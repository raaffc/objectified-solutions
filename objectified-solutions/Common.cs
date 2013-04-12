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

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace objectified_solutions {
    public class Common {
        public static void CheckNull(object var, string varName) {
            if (var == null) {
                throw new ArgumentNullException(varName);
            }
        }

        public static string GetVariableName<T>(Expression<Func<T>> expr) {
            var body = ((MemberExpression)expr.Body);
            return body.Member.Name;
        }

        public static List<string> ApplyFilter(List<string> lines, string startsWith, string contains) {
            var filteredLines = new List<string>();
            if (startsWith == null) {
                foreach(string line in lines) {
                    if(line.Contains(contains)) {
                        filteredLines.Add(line);
                    }
                }
            } else {
                foreach(string line in lines) {
                    if (line.StartsWith(startsWith)) {
                        filteredLines.Add(line);
                    }
                }
            }
            return filteredLines;
        }

        public static List<string> GetSolutionFolders(List<string> lines) {
            var solutionFolders = new List<string>();
            foreach(string line in lines) {
                if(!line.Contains(Constants.CSPROJ) && !line.Contains(Constants.WIXPROJ) && !line.Contains(Constants.DTPROJ)) {
                    solutionFolders.Add(line);
                }
            }
            return solutionFolders;
        }

        public static List<string> GetNestedProjectsSectionAsLines(List<string> lines) {
            var nestedProjects = new List<string>();
            bool sectionFound = false;
            foreach (string line in lines) {
                if(line.Contains(Constants.NESTED_PROJECTS)) {
                    sectionFound = true;
                    continue;
                }
                if(sectionFound) {
                    if(line.Contains("EndGlobalSection")) {
                        sectionFound = false;
                    } else {
                        nestedProjects.Add(line);
                    }
                }
            }
            return nestedProjects;
        }

        public static string[] Split(string line) {
            return line.Split(Constants.SPACE_CHAR);
        }

        public static string TrimToken(string s, int start, int lengthSubtractor) {
            return s.Substring(start, s.Length - lengthSubtractor);
        }

        public static string Tabs(int numTabs) {
            var sb = new StringBuilder();
            for(int i = 0; i < numTabs; i++) {
                sb.Append(Constants.FOURSPACES);
            }
            return sb.ToString();
        }
    }
}
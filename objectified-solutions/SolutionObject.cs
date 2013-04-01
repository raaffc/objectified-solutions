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
using objectified_solutions.views.fileview;
using objectified_solutions.views.solutionview;

namespace objectified_solutions {
    public class SolutionObject {
        public string FormatVersion { get; set; }
        public string VSVersion { get; set; }
        public string Name { get; set; }
        public string RootPath { get; set; }
        public FileView FileView { get; set; }
        public SolutionView SolutionView { get; set; }

        public SolutionObject(string slnFile) {
            List<string> lines = new List<string>(File.ReadAllLines(slnFile));
            RootPath = GetRootPath(slnFile);
            Name = GetName(slnFile);
            FormatVersion = GetFormatVersion(lines[0]);
            VSVersion = lines[1].Substring(2);

            FileView = new FileView(lines, RootPath);
            SolutionView = new SolutionView(lines, RootPath);
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
    }
}
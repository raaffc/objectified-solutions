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
namespace objectified_solutions.parsers {
    //Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "Installation", "Installation", "{1BC9248A-7F32-4816-95A4-2D3DB14CA300}"
    public class SolutionFolderLine {        
        public string Name { get; set; }
        public string FolderGuid { get; set; }

        public SolutionFolderLine(string solutionFolderLine) {
            var tokens = solutionFolderLine.Split(Constants.SPACE_CHAR);
            Name = Common.TrimToken(tokens[2], 1, 3);
            FolderGuid = Common.TrimToken(tokens[4], 1, 3);
        }
    }
}
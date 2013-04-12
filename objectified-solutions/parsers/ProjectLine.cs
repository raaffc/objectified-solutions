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
    public class ProjectLine {
        //Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "mycsproject", "path\to\mycsproject.csproj", "{95B7703A-54F2-43FC-8664-E648E51B86E6}"
        //Project("{930C7802-8A8C-48F9-8165-68863BCCD9DD}") = "mywixproject", "path\to\mywixproject.wixproj", "{8FE4FFF6-205F-4DCD-9823-6DB6904D63CA}"
        //Project("{D183A3D8-5FD8-494B-B014-37F57B35E655}") = "mydtproject", "path\to\mydtproject.dtproj", "{10951775-2AA3-438D-99AA-1F8B9239E288}"
        public string Name { get; set; }
        public string RelativePath { get; set; }
        public string ProjectGuid { get; set; } //unique id
        
        public ProjectLine(string line) {
            var tokens = Common.Split(line);
            Name = Common.TrimToken(tokens[2], 1, 3);
            RelativePath = Common.TrimToken(tokens[3], 1, 3);
            ProjectGuid = Common.TrimToken(tokens[4], 2, 4);
        }
    }
}

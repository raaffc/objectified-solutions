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
using objectified_solutions;
using objectified_solutions.views.fileview.project;

namespace Tester {
    class Program {
        static void Main(string[] args) {
            if(args.Length != 1) {
                Console.WriteLine("Usage: Tester <path to .sln file>.");
            } else {
                SolutionObject slnObj = new SolutionObject(args[0]);

                Console.WriteLine("Solution Format Version: {0}", slnObj.FormatVersion);
                Console.WriteLine("Solution will open in {0}", slnObj.VSVersion);
                Console.WriteLine("Solution location: {0}", slnObj.RootPath);
                Console.WriteLine("Projects in {0} solution:", slnObj.Name);
                foreach(ProjectObject project in slnObj.FileView.Projects) {
                    Console.WriteLine(project.Name);
                    Console.WriteLine("RelativePath: {0}", project.RelativePath);
                    Console.WriteLine("Number of Source Files: {0}", project.SourceFiles.Count);
                    Console.WriteLine("Number of System References: {0}", project.References.Count);
                    Console.WriteLine("Number of Project References: {0}", project.ProjectReferences.Count);
                    Console.WriteLine("-------------");
                }
                Console.WriteLine("End Program");
            }
        }
    }
}
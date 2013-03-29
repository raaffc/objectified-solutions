using System;
using objectified_solutions.parsers;
using objectified_solutions.project;
using objectified_solutions.solution;

namespace Tester {
    class Program {
        static void Main(string[] args) {
            if(args.Length != 1) {
                Console.WriteLine("Usage: Tester <path to .sln file>.");
            } else {
                SolutionObject slnObj = SLNFileParser.Parse(args[0]);
                
                Console.WriteLine("Solution Format Version: {0}", slnObj.FormatVersion);
                Console.WriteLine("Solution will open in {0}", slnObj.VSVersion);
                Console.WriteLine("Solution location: {0}", slnObj.RootPath);
                Console.WriteLine("Projects in {0} solution:", slnObj.Name);
                foreach(ProjectObject project in slnObj.Projects) {
                    Console.WriteLine("Name: {0}", project.Name);
                    Console.WriteLine("RelativePath: {0}", project.RelativePath);
                }
                Console.WriteLine("End Program");
            }
        }
    }
}
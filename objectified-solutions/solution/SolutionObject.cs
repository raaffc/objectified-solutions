using System.Collections.Generic;
using objectified_solutions.project;

namespace objectified_solutions.solution {
    public class SolutionObject {
        public string FormatVersion { get; set; }
        public string VSVersion { get; set; }
        public string Name { get; set; }
        public string RootPath { get; set; }
        public List<ProjectObject> Projects { get; set; }

        public SolutionObject(string slnFile, List<string> lines) {
            RootPath = GetRootPath(slnFile);
            Name = GetName(slnFile);
            FormatVersion = GetFormatVersion(lines[0]);
            VSVersion = lines[1].Substring(2);
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
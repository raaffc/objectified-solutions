using System.Collections.Generic;
using objectified_solutions.project;

namespace objectified_solutions.solution {
    public class SolutionObject {
        public string Version { get; set; }
        public List<ProjectObject> Projects { get; set; }
    }
}

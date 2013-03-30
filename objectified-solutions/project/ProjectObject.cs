using System.Collections.Generic;
using objectified_solutions.source;

namespace objectified_solutions.project {
    public class ProjectObject {
        public string Name { get; set; }
        public string ProjectGuid { get; set; } //unique id
        public string RelativePath { get; set; }
        public string OutputType { get; set; }
        public string Configuration { get; set; }
        public string Platform { get; set; }
        public string ProductVersion { get; set; }
        public string RootNamespace { get; set; }
        public string TargetFrameworkVersion { get; set; }
        public string SchemaVersion { get; set; }
                
        public List<SourceCodeFile> SourceFiles { get; set; }
        public List<Reference> References { get; set; }
        public List<ProjectReference> ProjectReferences { get; set; }
    }
}
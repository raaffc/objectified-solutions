namespace objectified_solutions.source {
    public class SourceCodeFile {
        public string FileName { get; set; }
        public bool IsCompiled { get; set; }
        public string RelativePath { get; set; }
        public string DependentUpon { get; set; }
        public string SubType { get; set; }
        public string CopyToOutputDirectory { get; set; }
    }
}
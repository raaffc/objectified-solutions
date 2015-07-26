using objectified_solutions.views.fileview.project;

namespace objectified_solutions.parsers{
    public interface IProjectFileParser{
        void Parse(string projFileFullPath, Project project);
    }
}
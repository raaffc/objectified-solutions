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
using System.Collections.Generic;
using objectified_solutions.parsers;
using objectified_solutions.views.fileview.project;

namespace objectified_solutions.views.fileview{
    public class FileView{
        public List<Project> Projects { get; set; }

        public FileView(List<string> lines, string rootPath){
            var projectLines = Common.ApplyFilter(lines, Constants.PROJECT, null);
            var csprojLines = Common.ApplyFilter(projectLines, null, Constants.CSPROJ);
            var cppprojLines = Common.ApplyFilter(projectLines, null, Constants.CPPPROJ);
            var wixprojLines = Common.ApplyFilter(projectLines, null, Constants.WIXPROJ);

            Projects = new List<Project>();
            ProcessProjectEntryLines(rootPath, csprojLines);
            ProcessProjectEntryLines(rootPath, cppprojLines);
            ProcessProjectEntryLines(rootPath, wixprojLines);
        }

        private void ProcessProjectEntryLines(string rootPath, IEnumerable<string> lines){
            foreach (var line in lines){
                var projectLine = new SolutionFileProjectEntry(line);
                var fullPath = rootPath + projectLine.RelativePath;
                var lang = MapExtensionToLanguage(projectLine.ProjectFileExtension);

                IProjectFileParser parser;
                switch (lang){
                    case ProjectLanguageTypes.Cs:
                        parser = new CsProjFileParser();
                        break;
                    case ProjectLanguageTypes.Cpp:
                        parser = new CppProjFileParser();
                        break;
                    case ProjectLanguageTypes.Unknown:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                var project = new Project{
                    Name = projectLine.Name,
                    RelativePath = projectLine.RelativePath,
                    FileExtension = projectLine.ProjectFileExtension,
                    ProjectGuid = projectLine.ProjectGuid,
                    ProjectLanguage = lang,
                };
                Projects.Add(project);
                project.FullPath = fullPath;

                parser.Parse(project.FullPath, project);
            }
        }

        private static ProjectLanguageTypes MapExtensionToLanguage(string projectFileExtension){
            switch (projectFileExtension.Substring(1)){
                case Constants.CSPROJ:
                case Constants.WIXPROJ:
                    return ProjectLanguageTypes.Cs;
                case Constants.CPPPROJ:
                    return ProjectLanguageTypes.Cpp;
                default:
                    return ProjectLanguageTypes.Unknown;
            }
        }

        public string GetProjectName(string projectGuid){
            var name = string.Empty;
            foreach (var project in Projects){
                if (project.ProjectGuid.Equals(projectGuid)){
                    name = project.Name;
                    break;
                }
            }
            return name;
        }
    }
}
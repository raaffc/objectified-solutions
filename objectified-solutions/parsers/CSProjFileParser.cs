using System.Collections.Generic;
using System.IO;
using System.Xml;
using objectified_solutions.project;

namespace objectified_solutions.parsers {
    public class CSProjFileParser {
        public static void Parse(string csprojFile, ProjectObject project) {
            XmlDocument doc = new XmlDocument();
            doc.Load(csprojFile);
            XmlNodeList propertyGroupList = doc.SelectNodes("/Project/PropertyGroup");
            
            List<string> lines = new List<string>(File.ReadAllLines(csprojFile));
            project.OutputType = GetProperty(lines, "OutputType");
            project.Configuration = GetProperty(lines, "Configuration");
            project.Platform  = GetProperty(lines, "Platform");
            project.ProductVersion = GetProperty(lines, "ProductVersion");
            project.RootNamespace = GetProperty(lines, "RootNamespace");
            project.TargetFrameworkVersion = GetProperty(lines, "TargetFrameworkVersion");
            project.SchemaVersion = GetProperty(lines, "SchemaVersion");
            project.ProjectGuid = GetProperty(lines, "ProjectGuid");



        
            //SourceCodeFiles //List<SourceCodeFile>
            //ReferencedProjects //List<ProjectObject>

        }

        private static string GetProperty(List<string> lines, string property){
            string value = string.Empty;
            XmlDocument doc = new XmlDocument();
            foreach(string line in lines){
                if(line.Contains(property)){
                    doc.LoadXml(line);
                    value = doc.FirstChild.InnerText;
                    break;
                }
            }
            return value;
        }
    }
}
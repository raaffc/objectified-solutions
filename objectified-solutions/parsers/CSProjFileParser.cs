using System.Collections.Generic;
using System.Xml;
using objectified_solutions.project;
using objectified_solutions.source;

namespace objectified_solutions.parsers {
    public class CSProjFileParser {
        public static void Parse(string csprojFile, ProjectObject project) {
            XmlDocument doc = new XmlDocument();
            XmlNamespaceManager nsmgr = GetNsMgr(csprojFile);
            doc.Load(csprojFile);

            XmlNodeList propertyGroupList = doc.SelectNodes("//msbuild:Project/msbuild:PropertyGroup", nsmgr);
            XmlNode properties = propertyGroupList[0];
            project.OutputType = GetProperty(properties, Constants.PROPERTY_OUTPUTTYPE);
            project.Configuration = GetProperty(properties, Constants.PROPERTY_CONFIGURATION);
            project.Platform = GetProperty(properties, Constants.PROPERTY_PLATFORM);
            project.ProductVersion = GetProperty(properties, Constants.PROPERTY_PRODUCTVERSION);
            project.RootNamespace = GetProperty(properties, Constants.PROPERTY_ROOTNAMESPACE);
            project.TargetFrameworkVersion = GetProperty(properties, Constants.PROPERTY_TARGETFRAMEWORKVERSION);
            project.SchemaVersion = GetProperty(properties, Constants.PROPERTY_SCHEMAVERSION);
            project.ProjectGuid = GetProperty(properties, Constants.PROPERTY_PROJECTGUID);

            XmlNodeList itemGroups = doc.SelectNodes("//msbuild:Project/msbuild:ItemGroup", nsmgr);
            ProcessItemGroups(itemGroups, project);
        }

        private static void ProcessItemGroups(XmlNodeList itemGroups, ProjectObject project) {
            project.References = new List<Reference>();
            project.SourceFiles = new List<SourceCodeFile>();
            project.ProjectReferences = new List<ProjectReference>();
            foreach(XmlNode itemGroup in itemGroups) {
                XmlNodeList childNodes = itemGroup.ChildNodes;
                if(childNodes.Count > 0) {
                    switch(childNodes[0].Name) {
                        case Constants.ITEM_GROUP_REFERENCE:
                            ProcessReferenceItems(childNodes, project);
                            break;
                        case Constants.ITEM_GROUP_COMPILE:
                            ProcessSourceItems(childNodes, project, Constants.ITEM_GROUP_COMPILE);
                            break;
                        case Constants.ITEM_GROUP_CONTENT:
                            ProcessSourceItems(childNodes, project, Constants.ITEM_GROUP_CONTENT);
                            break;
                        case Constants.ITEM_GROUP_PROJECTREFERENCE:
                            ProcessProjectReferenceItems(childNodes, project);
                            break;
                        case Constants.ITEM_GROUP_BOOTSTRAPPERPACKAGE:
                            break;
                    }
                }
            }
        }

        private static void ProcessReferenceItems(XmlNodeList nodes, ProjectObject project) {
            foreach (XmlNode node in nodes) {
                string name = GetName(node.Attributes[0]);
                string specificVersionString = GetProperty(node, Constants.PROPERTY_SPECIFICVERSION);
                bool specificVersion = specificVersionString != null && bool.Parse(specificVersionString);
                string hintPath = GetProperty(node, Constants.PROPERTY_HINTPATH);
                Reference reference = new Reference { Name = name, SpecificVersion = specificVersion, HintPath = hintPath };
                project.References.Add(reference);
            }
        }

        private static void ProcessSourceItems(XmlNodeList nodes, ProjectObject project, string itemType) {
            foreach (XmlNode node in nodes) {
                SourceCodeFile sourceFile;
                string filename = GetName(node.Attributes[0]);
                string relativePath = GetRelativePath(node.Attributes[0]);
                string subType = GetProperty(node, Constants.PROPERTY_SUBTYPE);
                if(itemType.Equals(Constants.ITEM_GROUP_COMPILE)) {
                    string dependentUpon = GetProperty(node, Constants.PROPERTY_DEPENDENTUPON);
                    sourceFile = new SourceCodeFile { FileName = filename, RelativePath = relativePath, IsCompiled = true, SubType = subType, DependentUpon = dependentUpon };
                } else {
                    string copyToOutputDirectory = GetProperty(node, Constants.PROPERTY_COPYTOOUTPUTDIRECTORY);
                    sourceFile = new SourceCodeFile { FileName = filename, RelativePath = relativePath, IsCompiled = false, SubType = subType, CopyToOutputDirectory = copyToOutputDirectory };
                }
                project.SourceFiles.Add(sourceFile);
            }
        } 

        private static void ProcessProjectReferenceItems(XmlNodeList nodes, ProjectObject project) {
            foreach (XmlNode node in nodes) {
                string name = GetProperty(node, Constants.PROPERTY_NAME);
                string projectGuid = GetProperty(node, Constants.PROPERTY_PROJECT);
                string relativePath = GetRelativePath(node.Attributes[0]);
                ProjectReference projectReference = new ProjectReference { Name = name, ProjectGuid = projectGuid, RelativePath = relativePath };
                project.ProjectReferences.Add(projectReference);
            }
        }

        private static string GetName(XmlAttribute attribute) {
            string name = attribute.Value;
            if(!name.Contains(Constants.COMMA)) {
                return name;
            }
            int firstComma = name.IndexOf(Constants.COMMA);
            return name.Remove(firstComma);
        }

        private static string GetRelativePath(XmlAttribute attribute) {
            string path = attribute.Value;
            if(!path.Contains(Constants.BACKSLASH)) {
                return null;
            }
            int lastSlash = path.LastIndexOf(Constants.BACKSLASH);
            return path.Substring(0, lastSlash + 1);
        }

        private static XmlNamespaceManager GetNsMgr(string csprojFile) {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(new XmlTextReader(csprojFile).NameTable);
            nsmgr.AddNamespace(Constants.MSBUILD, "http://schemas.microsoft.com/developer/msbuild/2003");
            nsmgr.PushScope();
            return nsmgr;
        }

        private static string GetProperty(XmlNode properties, string property){
            string value = null;
            foreach(XmlNode childNode in properties.ChildNodes){
                if(childNode.Name.Equals(property)) {
                    value = childNode.InnerText;
                    break;
                }
            }
            return value;
        }
    }
}
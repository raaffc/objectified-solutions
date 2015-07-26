using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using objectified_solutions.views.fileview.project;
using objectified_solutions.views.fileview.source;

namespace objectified_solutions.parsers{
    public class CppProjFileParser : IProjectFileParser{
        public void Parse(string projFileFullPath, Project project){
            var doc = new XmlDocument();
            var nsmgr = ProjectParserHelper.GetNsMgr(projFileFullPath);
            doc.Load(projFileFullPath);

            var propertyGroupList = doc.SelectNodes("//msbuild:Project/msbuild:PropertyGroup", nsmgr);
            var properties = propertyGroupList?.Cast<XmlNode>().FirstOrDefault(n => n.Attributes?.GetNamedItem("Label")?.Value == "Globals");
            if (properties != null){
                project.RootNamespace = ProjectParserHelper.GetProperty(properties, Constants.PROPERTY_ROOTNAMESPACE);
            }

            var itemGroups = doc.SelectNodes("//msbuild:Project/msbuild:ItemGroup", nsmgr);
            ProcessItemGroups(itemGroups, project);
        }

        private static void ProcessItemGroups(XmlNodeList itemGroups, Project project){
            project.References = new List<Reference>();
            project.SourceFiles = new List<SourceCodeFile>();
            project.ProjectReferences = new List<ProjectReference>();
            foreach (XmlNode itemGroup in itemGroups){
                var childNodes = itemGroup.ChildNodes;
                if (childNodes.Count > 0){
                    switch (childNodes[0].Name){
                        case Constants.ITEM_GROUP_CPPINCLUDE:
                            ProcessSourceItems(childNodes, project, Constants.ITEM_GROUP_CPPINCLUDE);
                            break;
                        case Constants.ITEM_GROUP_CPPCOMPILE:
                            ProcessSourceItems(childNodes, project, Constants.ITEM_GROUP_CPPCOMPILE);
                            break;
                        case Constants.ITEM_GROUP_RESOURCECOMPILE:
                            ProcessSourceItems(childNodes, project, Constants.ITEM_GROUP_RESOURCECOMPILE);
                            break;

                        //case Constants.ITEM_GROUP_PROJECTREFERENCE:
                        //    ProcessProjectReferenceItems(childNodes, project);
                        //    break;
                        //case Constants.ITEM_GROUP_BOOTSTRAPPERPACKAGE:
                        //    break;
                        //case Constants.ITEM_GROUP_REFERENCE:
                        //    ProcessReferenceItems(childNodes, project);
                        //    break;
                    }
                }
            }
        }

        //private static void ProcessReferenceItems(XmlNodeList nodes, Project project){
        //    foreach (XmlNode node in nodes){
        //        if (node.Attributes != null){
        //            var name = GetName(node.Attributes[0]);
        //            var specificVersionString = ProjectParserHelper.GetProperty(node, Constants.PROPERTY_SPECIFICVERSION);
        //            var specificVersion = specificVersionString != null && bool.Parse(specificVersionString);
        //            var hintPath = ProjectParserHelper.GetProperty(node, Constants.PROPERTY_HINTPATH);
        //            var reference = new Reference{
        //                Name = name,
        //                SpecificVersion = specificVersion,
        //                HintPath = hintPath
        //            };
        //            project.References.Add(reference);
        //        }
        //    }
        //}

        private static void ProcessSourceItems(XmlNodeList nodes, Project project, string itemType){
            foreach (XmlNode node in nodes){
                SourceCodeFile sourceFile = null;
                if (node.Attributes != null){
                    var filename = GetName(node.Attributes[0]);
                    var relativePath = GetRelativePath(node.Attributes[0]);
                    var subType = ProjectParserHelper.GetProperty(node, Constants.PROPERTY_SUBTYPE);
                    var dependentUpon = ProjectParserHelper.GetProperty(node, Constants.PROPERTY_DEPENDENTUPON);
                    sourceFile = new SourceCodeFile{
                        FileName = filename,
                        RelativePath = relativePath,
                        IsCompiled = true,
                        SubType = subType,
                        DependentUpon = dependentUpon
                    };
                }
                project.SourceFiles.Add(sourceFile);
            }
        }

        //private static void ProcessProjectReferenceItems(XmlNodeList nodes, Project project){
        //    foreach (XmlNode node in nodes){
        //        var name = ProjectParserHelper.GetProperty(node, Constants.PROPERTY_NAME);
        //        var projectGuid = ProjectParserHelper.GetProperty(node, Constants.PROPERTY_PROJECT);
        //        if (node.Attributes != null){
        //            var relativePath = GetRelativePath(node.Attributes[0]);
        //            var projectReference = new ProjectReference{
        //                Name = name,
        //                ProjectGuid = projectGuid,
        //                RelativePath = relativePath
        //            };
        //            project.ProjectReferences.Add(projectReference);
        //        }
        //    }
        //}

        private static string GetName(XmlAttribute attribute){
            var name = attribute.Value;
            if (!name.Contains(Constants.COMMA)){
                return name;
            }
            var firstComma = name.IndexOf(Constants.COMMA, StringComparison.Ordinal);
            return name.Remove(firstComma);
        }

        private static string GetRelativePath(XmlAttribute attribute){
            var path = attribute.Value;
            if (!path.Contains(Constants.BACKSLASH)){
                return null;
            }
            var lastSlash = path.LastIndexOf(Constants.BACKSLASH, StringComparison.Ordinal);
            return path.Substring(0, lastSlash + 1);
        }
    }
}
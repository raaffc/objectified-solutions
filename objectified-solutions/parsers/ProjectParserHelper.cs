using System.Xml;

namespace objectified_solutions.parsers{
    internal class ProjectParserHelper{
        public static XmlNamespaceManager GetNsMgr(string csprojFile){
            var nsmgr = new XmlNamespaceManager(new XmlTextReader(csprojFile).NameTable);
            nsmgr.AddNamespace(Constants.MSBUILD, "http://schemas.microsoft.com/developer/msbuild/2003");
            nsmgr.PushScope();
            return nsmgr;
        }

        public static string GetProperty(XmlNode properties, string property){
            string value = null;
            foreach (XmlNode childNode in properties.ChildNodes){
                if (childNode.Name.Equals(property)){
                    value = childNode.InnerText;
                    break;
                }
            }
            return value;
        }
    }
}
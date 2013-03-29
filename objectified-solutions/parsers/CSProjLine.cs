namespace objectified_solutions.parsers {
    public class CSProjLine {
        public string Name { get; set; }
        public string RelativePath { get; set; }

        //Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "eStoreAdminBLL", "eStoreAdminBLL\eStoreAdminBLL.csproj", "{95B7703A-54F2-43FC-8664-E648E51B86E6}"
        public CSProjLine(string line) {
            string[] tokens = line.Split(' ');
            Name = RemoveQuotes(RemoveTrailingComma(tokens[2]));
            RelativePath = RemoveQuotes(RemoveTrailingComma(tokens[3]));
        }

        private string RemoveQuotes(string s) {
            return s.Substring(1, s.Length - 2);
        }

        private string RemoveTrailingComma(string s){
            return s.Remove(s.Length - 1);
        }
    }
}
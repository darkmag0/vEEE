using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace vEEE
{
    class Program
    {
        static void Main(string[] args)
        {
            vServer vS;
            vClient vC;
            vIniResource vIni = new vIniResource();
            vIni.load();

            if(vIniResource.vIniParameters["mod"] == "server")
                vS = new vServer();
            if(vIniResource.vIniParameters["mod"] == "client") 
                vC = new vClient();

            while (vConsole.readCommand());
            
            return;
        }
    }

    class vServer
    {
        static vCatalog vRepositoryCatalog;
        static vCatalog vFinalCatalog;

        static List<vUser> vUserList = new List<vUser>();

        public vServer()
        {
            vConsoleDebug.debugMessage("server run");

            if (vIniResource.vIniParameters.ContainsKey("vIniServerRepositoryPath"))
                vRepositoryCatalog = new vCatalog(vIniResource.vIniParameters["vIniServerRepositoryPath"]);
            if (vIniResource.vIniParameters.ContainsKey("vIniServerFinalPath"))
                vFinalCatalog = new vCatalog(vIniResource.vIniParameters["vIniServerFinalPath"]);
        }

        public vServer(string r, string f)
        {
            vConsoleDebug.debugMessage("server run");

            vRepositoryCatalog = new vCatalog(r);
            vFinalCatalog = new vCatalog(f);
        }

        static public bool setRepositoryPath()
        {
            Regex re = new Regex("conf.rep (?<rep>.*)");
            Match match;

            match = re.Match(vConsole.vLastCommand);

            if (match.Success)
            {
                if (Directory.Exists(match.Groups["rep"].Value))
                {
                    vRepositoryCatalog = new vCatalog(match.Groups["rep"].Value);
                    vConsoleDebug.debugMessage("repository catalog was changed on " + match.Groups["rep"].Value);
                    return true;
                }
                else
                {
                    vConsoleDebug.debugMessage("path \"" + match.Groups["rep"].Value + "\" does not exist");
                }
            }

            return false; ;
        }

        public override string ToString()
        {
            string tmp = "Repository catalog: " + vRepositoryCatalog.ToString() + "\nFinal catalog: " + vFinalCatalog.ToString()+"\n";
            if (vUserList.Count() != 0)
            {
                tmp += "USERLIST:";
                for (int i = 0; i < vUserList.Count(); i++)
                {
                    tmp += "\n" + (i + 1) + " - " + vUserList.ElementAt(i).ToString();
                }
            }

            return tmp;
        }

        static public bool addUser()
        {
            Regex re = new Regex("addu (?<login>.*?)@(?<password>.*)");
            Match match;
            GroupCollection gc;

            match = re.Match(vConsole.vLastCommand);

            if (match.Success)
            {
                gc = match.Groups;
                vUser u = new vUser(gc["login"].Value, gc["password"].Value);
                vServer.vUserList.Add(u);
                vConsoleDebug.debugMessage("user " + u.ToString() + " added");
                return true;
            }

            return false;
        }

        public bool addUser(vUser u)
        {
            vUserList.Add(u);
            vConsoleDebug.debugMessage("user " + u.ToString() + " added");

            return true;
        }

        public bool addUser(string l, string p)
        {
            vUser u = new vUser(l, p); 
            vUserList.Add(u);
            vConsoleDebug.debugMessage("user " + u.ToString() + " added");

            return true;
        }

        static public bool removeUser()
        {
            Regex re = new Regex("remu (?<login>.*)");
            Match match;

            if (vUserList.Count() == 0)
            {
                vConsoleDebug.debugMessage("user list is empty");
                return false;
            }

            match = re.Match(vConsole.vLastCommand);

            if (match.Success)
            {
                string l = match.Groups["login"].Value;
                
                int i;
                for (i = 0; i < vUserList.Count() && vUserList.ElementAt(i).getLogin() != l; i++) ;
                if (i == vUserList.Count() && vUserList.ElementAt(i - 1).getLogin() != l)
                {
                    vConsoleDebug.debugMessage("user " + l + " has no exist");
                    return false;
                }
                vUserList.RemoveAt(i);
                vConsoleDebug.debugMessage("user " + l + " has been deleted");

                return true;
            }

            return false;
        }

        public void removeUser(string l)
        {
            int i;
            for (i = 0; i < vUserList.Count() && vUserList.ElementAt(i).getLogin() != l; i++);
            if (i == vUserList.Count() && vUserList.ElementAt(i - 1).getLogin() != l)
            {
                vConsoleDebug.debugMessage("user " + l + " has no exist");
                return;
            }
            vUserList.RemoveAt(i);
            vConsoleDebug.debugMessage("user " + l + " has been deleted");
        }
    }
    
    class vClient
    {
        vCatalog vWorkCatalog;

        public vClient()
        {
            vConsoleDebug.debugMessage("client run");

            this.vWorkCatalog = new vCatalog("C:\\vEEE\\WC");
        }

        public vClient(string w)
        {
            vConsoleDebug.debugMessage("client run");

            this.vWorkCatalog = new vCatalog(w);
        }

        public override string ToString()
        {
            return "Work Catalog: "+this.vWorkCatalog+"\n";
        }
    }
    
    class vUser
    {
        string vLogin;
        string vPassword;

        public vUser(string l, string p)
        {
            this.vLogin = l;
            this.vPassword = p;
        }

        public override string ToString()
        {
            return this.vLogin+"@"+this.vPassword;
        }

        public string getLogin()
        {
            return this.vLogin;
        }
        public string getPassword()
        {
            return this.vPassword;
        }
    }
    
    class vCatalog
    {
        string vPath;
        List<vAccess> vAccessList;
        List<vFile> vFileList;

        public vCatalog(string vP)
        {
            vPath = vP;
        }
        public override string ToString()
        {
            return this.vPath;
        }
    }
    
    class vFile
    {
        string vName;
        List<vAccess> vAccessList;
    }
    
    unsafe class vAccess
    {
        void* user;
        bool vReadAccess;
        bool vWriteAccess;
    }
}

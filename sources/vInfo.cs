using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vEEE
{
    static class vInfo
    {
        static Dictionary<string, string> vServerCommandInfo;
        static Dictionary<string, string> vClientCommandInfo;

        static vInfo()
        {
            if (vIniResource.vIniParameters["mod"] == "server")
            {
                vServerCommandInfo = new Dictionary<string, string>();

                vServerCommandInfo.Add("help", "show command list");
                vServerCommandInfo.Add("info", "show application info");
                vServerCommandInfo.Add("dbg", "reset debug mode");
                vServerCommandInfo.Add("conf.rep PATH", "set repository path");
                vServerCommandInfo.Add("addu LOGIN@PASSWORD", "add new user at server");
                vServerCommandInfo.Add("remu LOGIN", "remove existing user");
                vServerCommandInfo.Add("exit", "close program");
            }
            if (vIniResource.vIniParameters["mod"] == "client")
            {
                vClientCommandInfo = new Dictionary<string, string>();

                vClientCommandInfo.Add("help", "show command list");
                vClientCommandInfo.Add("info", "show application info");
                vClientCommandInfo.Add("dbg", "reset debug mode");
                vClientCommandInfo.Add("exit", "close program");
            }
        }

        static public bool showHelp()
        {
            if (vIniResource.vIniParameters["mod"] == "server")
            {
                for (int i = 0; i < vServerCommandInfo.Count(); i++)
                {
                    vConsole.printLine(vServerCommandInfo.ElementAt(i).Key + " - " + vServerCommandInfo.ElementAt(i).Value);
                }
            }
            if (vIniResource.vIniParameters["mod"] == "client")
            {
                for (int i = 0; i < vClientCommandInfo.Count(); i++)
                {
                    vConsole.printLine(vClientCommandInfo.ElementAt(i).Key + " - " + vClientCommandInfo.ElementAt(i).Value);
                }
            }
            return true;
        }

        static public bool showInfo()
        {
            vConsole.printLine("vEEE v0.1");
            if(vIniResource.vIniParameters["mod"] == "server")
                vConsole.printLine("Server mod");
            if(vIniResource.vIniParameters["mod"] == "client")
                vConsole.printLine("Client mod");

            return true;
        }
    }
}

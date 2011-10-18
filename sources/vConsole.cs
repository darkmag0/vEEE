using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vEEE
{
    static class vConsole
    {
        static Dictionary<string, Func<bool>> vServerCommandList;
        static Dictionary<string, Func<bool>> vClientCommandList;
        static public string vLastCommand;

        static vConsole()
        {
            if (vIniResource.vIniParameters["mod"] == "server")
            {
                vServerCommandList = new Dictionary<string, Func<bool>>();

                vServerCommandList.Add("help", vInfo.showHelp);
                vServerCommandList.Add("info", vInfo.showInfo);
                vServerCommandList.Add("dbg", vConsoleDebug.resetDebugMode);
                vServerCommandList.Add("addu", vServer.addUser);
                vServerCommandList.Add("remu", vServer.removeUser);
                vServerCommandList.Add("conf.rep", vServer.setRepositoryPath);
            }
            if (vIniResource.vIniParameters["mod"] == "client")
            {
                vClientCommandList = new Dictionary<string, Func<bool>>();

                vClientCommandList.Add("help", vInfo.showHelp);
                vClientCommandList.Add("info", vInfo.showInfo);
                vClientCommandList.Add("dbg", vConsoleDebug.resetDebugMode);
            }
        }

        static public void printLine(string s)
        {
            Console.WriteLine("   "+s);
        }

        static public bool readCommand()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("$: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            string comm = Console.ReadLine();
            Console.ResetColor();

            if (comm == "exit")
                return false;

            vLastCommand = comm;

            if (vIniResource.vIniParameters["mod"] == "server")
            {
                if (vServerCommandList.ContainsKey(comm))
                {
                    vServerCommandList[comm]();
                }
                else if (vServerCommandList.ContainsKey(comm.Split(' ').First()))
                {
                    vServerCommandList[comm.Split(' ').First()]();
                }
            }
            else if (vIniResource.vIniParameters["mod"] == "client")
            {
                if (vClientCommandList.ContainsKey(comm))
                {
                    vClientCommandList[comm]();
                }
                else if (vClientCommandList.ContainsKey(comm.Split(' ').First()))
                {
                    vClientCommandList[comm.Split(' ').First()]();
                }
            }
            else if (comm == "")
            { }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                vConsole.printLine("unknown command: " + comm);
                Console.ResetColor();
            }

            return true;
        }
    }
   
    static class vConsoleDebug
    {
        static bool debugMode = true;
        static bool DebugMode
        {
            set
            {
                debugMode = value;
            }
            get
            {
                return debugMode;
            }
        }
        static Stack<string> messagesStack = new Stack<string>();

        static public bool resetDebugMode()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            debugMode = !debugMode;
            Console.Write("debug mode ");
            if (debugMode)
                Console.Write("on\n");
            else
                Console.Write("off\n");
            Console.ResetColor();
            return true;
        }

        static public void debugObject(Object o)
        {
            if (debugMode)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(o.GetType().ToString());
                Console.WriteLine(o.ToString());
                Console.ResetColor();
            }
        }
    
        static public void debugMessage(string s)
        {
            if (debugMode)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(s);
                Console.ResetColor();
            }
        }
    }
}

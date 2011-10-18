using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace vEEE
{
    abstract class vResource
    {
        protected string vIniPath;
        protected string vIniFile;

        abstract public bool load();
        abstract protected bool parse(StreamReader sr);
    }

    class vIniResource : vResource
    {
        static public Dictionary<string, string> vIniParameters = new Dictionary<string, string>();

        public vIniResource()
        {
            this.vIniPath = "";
            this.vIniFile = "vEEE.ini";
        }

        override public bool load()
        {
            if (System.IO.File.Exists(this.vIniFile))
            {
                vConsoleDebug.debugMessage(this.vIniPath + "\\" + this.vIniFile + " loaded.");
                if (parse(File.OpenText(this.vIniFile)))
                    return true;
                return false;
            }
            else
            {
                vConsoleDebug.debugMessage(this.vIniPath + "\\" + this.vIniFile + " is not exists.");
                return false;
            }
        }
        override protected bool parse(StreamReader sr)
        {
            string line = "";
            char commentChar = '#';
            
            Regex re_attr = new Regex("(?<attr>.*?) = (?<val>.*)");
            Match match;
            GroupCollection gc;

            vConsoleDebug.debugMessage("starting parse vEEE.ini");

            while ((line = sr.ReadLine()) != null)
            {
                if (line.Length != 0 && line[0] != commentChar)
                {
                    if (line == ".server")
                    {
                        vIniParameters.Add("mod", "server");
                        vConsoleDebug.debugMessage("programm mod is server");
                        continue;
                    }
                    if(line == ".client")
                    {
                        vIniParameters.Add("mod", "client");
                        vConsoleDebug.debugMessage("programm mod is client");
                        continue;
                    }

                    match = re_attr.Match(line);
                    gc = match.Groups;

                    vIniParameters.Add(gc["attr"].Value, gc["val"].Value);

                    vConsoleDebug.debugMessage(gc["attr"].Value + " = " + vIniParameters[gc["attr"].Value]);
                }
            }

            vConsoleDebug.debugMessage("ending parse vEEE.ini");
            sr.Close();
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vEEE
{
    abstract class vException : System.Exception
    {
        protected bool vCritical;
        protected string vMessage;

        //abstract public void showDebugMessage();
        //abstract public void showMessage();
    }

    class vIniException : vException
    {
        public vIniException()
        {
            this.vMessage = "some error at the .ini file processing";
            this.vCritical = false;
        }
    }
}

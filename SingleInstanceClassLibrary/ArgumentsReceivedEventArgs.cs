using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleInstanceClassLibrary
{
    /// <summary>
    /// Holds a list of arguments given to an application at startup.
    /// </summary>
    public class ArgumentsReceivedEventArgs : EventArgs
    {
        public String[] Args { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace CharSheet
{
    class BackStuff
    {
        public static bool CheckForQuit(string entry)
        {
            var quitCommands = new List<string> { "stop", "exit", "quit", "q", "return" };
            if (quitCommands.Any(str => str.Contains(entry)) && entry != "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

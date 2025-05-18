using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatBuster
{
    internal class Checker
    {
        public void CheckForCheats()
        {
            string windowsUserName = Environment.UserName;
            string folderPath = @"C:\Users\{windowsUserName}\AppData\Roaming\.minecraft\versions";
            

        }
    }
}

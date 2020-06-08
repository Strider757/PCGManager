using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCGManager
{
    public class Global
    {
        public static string ThisMachineName { get { return Environment.MachineName; } }
        public static string ThisUserName => Environment.UserName;
        
        //public static string ThisMachineName = Environment.MachineName;
    }
}

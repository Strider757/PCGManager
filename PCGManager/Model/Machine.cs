using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCGManager
{
    public class Machine
    {
        public int id { get; set; }
        public string machineName { get; set; }
        public string userName { get; set; }
        public string backupDir { get; set; }

        public ICollection<InstalledGame> InstalledGames { get; set; }

        public Machine()
        {
            InstalledGames = new List<InstalledGame>();
        }

        public override string ToString()
        {
            return machineName;
        }
    }
}

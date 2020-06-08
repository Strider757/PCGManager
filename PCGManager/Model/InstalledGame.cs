using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCGManager
{
    //[Table("MachineGames")]
    public class InstalledGame
    {
        public int id { get; set; }
        public string InstallPath { get; set; }
        public string GameSavePath{ get; set; }

        //public int Game_id { get; set; }
        public Game Game { get; set; }
        public Machine Machine { get; set; }
        //public int Machine_id { get; set; }
        ////public Machine Machine { get; set; }
        //public int Game_id { get; set; }
        ////public Game Game { get; set; }


        //public virtual Game Game { get; set; }
        //public virtual Machine Machine { get; set; }
    }
}

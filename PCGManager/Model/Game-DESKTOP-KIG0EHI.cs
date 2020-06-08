using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCGManager
{
    public class Game
    {
        public int id { get; set; }
        public string name { get; set; }

        public ICollection<InstalledGame> InstalledGames{ get; set; }

        public Game()
        {
            InstalledGames = new List<InstalledGame>();
        }

        public override string ToString()
        {
            return name;
        }
    }
}

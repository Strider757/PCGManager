using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PCGManager
{
    public class Game : INotifyPropertyChanged
    {
        public int id { get; set; }

        private string _name;
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }

        public ICollection<InstalledGame> InstalledGames { get; set; }

        public Game()
        {
            InstalledGames = new List<InstalledGame>();
        }

        public override string ToString()
        {
            return name;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}


//namespace PCGManager
//{
//    public class Game
//    {
//        public int id { get; set; }
//        public string name { get; set; }

//        public ICollection<InstalledGame> InstalledGames { get; set; }

//        public Game()
//        {
//            InstalledGames = new List<InstalledGame>();
//        }

//        public override string ToString()
//        {
//            return name;
//        }
//    }
//}

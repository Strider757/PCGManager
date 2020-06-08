using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PCGManager.ViewModel
{
    public class MachinesTableViewModel : INotifyPropertyChanged
    {
        //pcgmDBContext pcgmDBc;

        SQLiteContext sqlDBc;
        GameDataBase gDB = new GameDataBase();
        IEnumerable<Machine> machines;
        public IEnumerable<Machine> Machines
        {
            get { return machines; }
            set
            {
                machines = value;
                OnPropertyChanged("Machines");
            }
        }

        IEnumerable<InstalledGame> _machineGames;
        public IEnumerable<InstalledGame> MachineGames
        {
            get { return _machineGames; }
            set
            {
                _machineGames = value;
                OnPropertyChanged("MachineGames");
            }
        }



        //IEnumerable<MachineGame> machineGames;
        //public IEnumerable<MachineGame> MachineGames
        //{
        //    get { return machineGames; }
        //    set
        //    {
        //        machineGames = value;
        //        //OnPropertyChanged("Games");
        //    }
        //}
        //IEnumerable locGames;
        //public IEnumerable LocGames
        //{
        //    get { return locGames; }
        //    set
        //    {
        //        locGames = value;
        //        //    OnPropertyChanged("Games");
        //    }
        //}

        //private RelayCommand setDataToDb;
        //public RelayCommand SetDataToDb
        //{
        //    get
        //    {
        //        return setDataToDb ??
        //            (setDataToDb = new RelayCommand(obj =>
        //            {
        //                Machine mach1 = new Machine { machineName = "NN-016", userName = "d.usov", id = 1 };
        //                Machine mach2 = new Machine { machineName = "Desktop", userName = "dmitry", id = 2 };
        //                pcgmDBc.Machines.Add(mach1);
        //                pcgmDBc.Machines.Add(mach2);
        //                Game g1 = new Game { name = "NFS", id = 1 };
        //                Game g2 = new Game { name = "The Witcher 3", id = 2 };
        //                Game g3 = new Game { name = "Fallout 4", id = 3 };


        //                pcgmDBc.Games.Add(g1);
        //                pcgmDBc.Games.Add(g2);
        //                pcgmDBc.Games.Add(g3);
        //                //mach1.Games.Add(g3);
        //                //mach1.Games.Add(g2);
        //                //mach2.Games.Add(g1);

        //                InstalledGame ig1 = new InstalledGame { id = 1, InstallPath = @"C:\Games\NeedForSpeed", Game = g1, Machine = mach1};
        //                InstalledGame ig2 = new InstalledGame { id = 2, InstallPath = @"D:\Games\NeedForSpeed", Game = g1 , Machine = mach2};
        //                InstalledGame ig3 = new InstalledGame { id = 1, InstallPath = @"D:\Games\TheWitcher3", Game = g2, Machine = mach2};

        //                pcgmDBc.InstalledGames.Add(ig1);
        //                pcgmDBc.InstalledGames.Add(ig2);
        //                pcgmDBc.InstalledGames.Add(ig3);

        //                pcgmDBc.SaveChanges();
        //            }));
        //    }
        //}

        

        public event PropertyChangedEventHandler PropertyChanged;

        private RelayCommand setDataToDb;
        public RelayCommand SetDataToDb
        {
            get
            {
                return setDataToDb ??
                    (setDataToDb = new RelayCommand(obj =>
                    {
                        Machine mach1 = new Machine { machineName = "NN-016", userName = "d.usov", id = 1 };
                        Machine mach2 = new Machine { machineName = "Desktop", userName = "dmitry", id = 2 };
                        sqlDBc.Machines.Add(mach1);
                        sqlDBc.Machines.Add(mach2);
                        Game g1 = new Game { name = "NFS", id = 1 };
                        Game g2 = new Game { name = "The Witcher 3", id = 2 };
                        Game g3 = new Game { name = "Fallout 4", id = 3 };


                        sqlDBc.Games.Add(g1);
                        sqlDBc.Games.Add(g2);
                        sqlDBc.Games.Add(g3);
                        //mach1.Games.Add(g3);
                        //mach1.Games.Add(g2);
                        //mach2.Games.Add(g1);

                        InstalledGame ig1 = new InstalledGame { id = 1, InstallPath = @"C:\Games\NeedForSpeed", Game = g1, Machine = mach1 };
                        InstalledGame ig2 = new InstalledGame { id = 2, InstallPath = @"D:\Games\NeedForSpeed", Game = g1, Machine = mach2 };
                        InstalledGame ig3 = new InstalledGame { id = 1, InstallPath = @"D:\Games\TheWitcher3", Game = g2, Machine = mach2 };

                        sqlDBc.InstalledGames.Add(ig1);
                        sqlDBc.InstalledGames.Add(ig2);
                        sqlDBc.InstalledGames.Add(ig3);

                        sqlDBc.SaveChanges();
                    }));
            }
        }

        public void fillList()
        {
          //Machines = sqlDBc.Machines.Local.ToBindingList();
        }

        public MachinesTableViewModel()
        {

            sqlDBc = new SQLiteContext();
            sqlDBc.Machines.Load();
            Machines = sqlDBc.Machines.Local.ToBindingList();

            //MachineGames = sqlDBc.InstalledGames.Local.ToBindingList();


            //pcgmDBc = new pcgmDBContext();

            //pcgmDBc.Machines.Load();
            //Machines = pcgmDBc.Machines.Local.ToBindingList();
            //pcgmDBc.InstalledGames.Load();
            //MachineGames = pcgmDBc.InstalledGames.Local.ToBindingList();
            //MachineGames = pcgmDBc.Machines.Where(m => m.machineName == global.ThisMachineName).SelectMany(i => i.InstalledGames).ToList();

            //games = sqldbc.machines.where(c => c.machinename == "nn-016").selectmany(c => c.games).tolist(); // все игры на одном пк
            //pcgmDBc.MachineGames.Load();
            //
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }


}

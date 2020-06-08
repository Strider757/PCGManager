using PCGManager.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCGManager.ViewModel
{
    public class DBeditViewModel : INotifyPropertyChanged
    {
        #region InfsPropChange
        //Реализация интрефейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        SQLiteContext sqlDBc;

        #region Properties
        IEnumerable<Game> _gamesTable;
        public IEnumerable<Game> GamesTable
        {
            get { return _gamesTable; }
            set
            {
                _gamesTable = value;
                OnPropertyChanged("GamesTable");
            }
        }

        IEnumerable<Machine> _machinesTable;
        public IEnumerable<Machine> MachinesTable
        {
            get { return _machinesTable; }
            set
            {
                _machinesTable = value;
                OnPropertyChanged("MachinesTable");
            }
        }

        IEnumerable<InstalledGame> _installedGamesTable;
        public IEnumerable<InstalledGame> InstalledGamesTable
        {
            get { return _installedGamesTable; }
            set
            {
                _installedGamesTable = value;
                OnPropertyChanged("InstalledGamesTable");
            }
        }
        #endregion

        private RelayCommand updateDbCommand;
        public RelayCommand UpdateDbCommand
        {
            get
            {
                return updateDbCommand ??
                   (updateDbCommand = new RelayCommand(obj =>
                   {
                       //MessageBox.Show("kek");
                       GetData();
                   }));
            }
        }

        private void GetData()
        {
            sqlDBc = new SQLiteContext();
            sqlDBc.Machines.Load();
            sqlDBc.Games.Load();
            sqlDBc.InstalledGames.Load();

            GamesTable = sqlDBc.Games.Local.ToBindingList();
            MachinesTable = sqlDBc.Machines.Local.ToBindingList();
            InstalledGamesTable = sqlDBc.InstalledGames.Local.ToBindingList();
           
        }

        public static void test()
        {

        }

        public DBeditViewModel()
        {
            GetData();
        }
    }
}

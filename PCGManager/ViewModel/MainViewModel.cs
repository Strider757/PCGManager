using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using PCGManager.Views;

namespace PCGManager.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        GameDataBase gDB = new GameDataBase();
        SQLiteContext sqlDBc;

        private Page AllGamesTableView;
        private Page LocalGamesView;
        public static Machine LocalMachine;

        #region Properties

        public string ThisMachineName { get; set; }

        public string ThisUserName { get; set; }

        private Page _currentPage;
        public Page CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        private string _pageTitle;
        public string PageTitle
        {
            get
            {
                return _pageTitle;
            }
            set
            {
                _pageTitle = value;
                OnPropertyChanged("PageTitle");
            }
        }

        private bool _thisMachineIsRegistred;
        public bool ThisMachineIsRegistred
        {
            get
            {
                return _thisMachineIsRegistred;
            }
            set
            {
                _thisMachineIsRegistred = value;
                OnPropertyChanged("ThisMachineIsRegistred");
            }
        }

        #endregion

        #region Commands

        private RelayCommand _createDbCommand;
        public RelayCommand CreateDbCommand
        {
            get
            {
                return _createDbCommand ??
                  (_createDbCommand = new RelayCommand(obj =>
                  {
                      gDB.DataBaseCreate();
                  }));
            }
        }

        private RelayCommand _registerMachine;
        public RelayCommand RegisterMachine
        {
            get
            {
                return _registerMachine ??
                    (_registerMachine = new RelayCommand(obj =>
                    {
                        sqlDBc.Machines.Add(LocalMachine);
                        sqlDBc.SaveChanges();
                        ThisMachineIsRegistred = true;
                    }));
            }
        }

        private RelayCommand _setViewLocalGamesCommand;
        public RelayCommand SetViewLocalGamesCommand
        {
            get
            {
                return _setViewLocalGamesCommand ??
                    (_setViewLocalGamesCommand = new RelayCommand(obj =>
                    {
                        LocalGamesViewModel.cancelButton.Execute(null);
                        LocalGamesViewModel.updateButton.Execute(null);
                        LocalGamesView = new LocalGamesView();
                        CurrentPage = LocalGamesView;
                        PageTitle = "Local Games";
                    }));
            }
        }

        private RelayCommand _setViewAllGamesCommand;
        public RelayCommand SetViewAllGamesCommand
        {
            get
            {
                return _setViewAllGamesCommand ??
                    (_setViewAllGamesCommand = new RelayCommand(obj =>
                    {
                        CurrentPage = AllGamesTableView;
                        PageTitle = "All games";

                    }));
            }
        }

        private RelayCommand _setViewSettingsCommand;
        public RelayCommand SetViewSettingsCommand
        {
            get
            {
                return _setViewSettingsCommand ??
                    (_setViewSettingsCommand = new RelayCommand(obj =>
                    {
                        CurrentPage = SettingsView;
                        PageTitle = "Settings";
                    }));
            }
        }

        #endregion

        public MainViewModel()
        {
            try
            {
                if (!gDB.DataBaseExist)
                {
                    if( !gDB.DataBaseCreate())
                    {
                        throw new Exception("Data Base cannot create!");
                    }
                }
            }
            catch (Exception ex)
            {
                Message.Error(ex.Message);
            }
            sqlDBc = new SQLiteContext();
            ThisMachineIsRegistred = sqlDBc.Machines.Any(m => m.machineName == Global.ThisMachineName && m.userName == Global.ThisUserName);
            if (ThisMachineIsRegistred)
            {
                LocalMachine = sqlDBc.Machines.First(m => m.machineName == Global.ThisMachineName && m.userName == Global.ThisUserName);
            }
            else
            {
                LocalMachine = new Machine { machineName = Global.ThisMachineName, userName = Global.ThisUserName };
            }
            ThisMachineName = Global.ThisMachineName;
            ThisUserName = Global.ThisUserName;
            AllGamesTableView = new AllGamesTableView();
            LocalGamesView = new LocalGamesView();
            AllMachinesTable = new MachinesTableView();
            DbEditView = new DBeditView();
            SettingsView = new SettingsView();
            CurrentPage = LocalGamesView;
            PageTitle = "Local Games";
        }

        public static RelayCommand command;


        private Page AllMachinesTable;
        private Page DbEditView;
        
        private Page SettingsView;



        #region trash

        private RelayCommand updateDbTestCommand;
        public RelayCommand UpdateDbTestCommand
        {
            get
            {
                return updateDbTestCommand ??
                    (updateDbTestCommand = new RelayCommand(obj =>
                    {
                        command.Execute(null);
                    }));
            }
        }



        private RelayCommand setViewMachinesCommand;
        public RelayCommand SetViewMachinesCommand
        {
            get
            {
                return setViewMachinesCommand ??
                    (setViewMachinesCommand = new RelayCommand(obj =>
                    {
                        CurrentPage = AllMachinesTable;
                        PageTitle = "";
                    }));
            }
        }

        //SetViewSettingsCommand



        private RelayCommand setViewDbEditCommand;
        public RelayCommand SetViewDbEditCommand
        {
            get
            {
                return setViewDbEditCommand ??
                    (setViewDbEditCommand = new RelayCommand(obj =>
                    {
                        CurrentPage = DbEditView;
                        PageTitle = "DB";
                    }));
            }
        }






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
        IEnumerable<Game> games;
        public IEnumerable<Game> Games
        {
            get { return games; }
            set
            {
                games = value;
                OnPropertyChanged("Games");
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }


    


}

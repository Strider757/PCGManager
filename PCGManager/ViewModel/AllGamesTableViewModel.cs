using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PCGManager.ViewModel
{
    public class AllGamesTableViewModel : INotifyPropertyChanged
    {

        SQLiteContext sqlDBc;

        #region propertyies

        private Game _selectedGame;
        public Game SelectedGame
        {
            get
            {
                return _selectedGame;
            }
            set
            {
                _selectedGame = value;
                //OnPropertyChanged(nameof(SelectedGame));
                OnPropertyChanged("SelectedGame");
            }
        }

        private Machine _selectedMachine;
        public Machine SelectedMachine
        {
            get
            {
                return _selectedMachine;
            }
            set
            {
                _selectedMachine = value;
                //OnPropertyChanged(nameof(SelectedGame));
                OnPropertyChanged("SelectedMachine");
            }
        }

        private string _gameName;
        public string GameNameProp
        {
            get
            {
                    return _gameName;
            }
            set
            {
                _gameName = value;
                OnPropertyChanged("GameNameProp");
            }
        }

        private bool _tableChanged;
        public bool TableChangedProp
        {
            get
            {
                return _tableChanged;
            }
            set
            {
                _tableChanged = value;
                OnPropertyChanged("TableChangedProp");
            }
        }
        private bool _editTextEnabled;
        public bool EditTextEnabled
        {
            get
            {
                return _editTextEnabled;
            }
            set
            {
                _editTextEnabled = value;
                OnPropertyChanged("EditTextEnabled");
            }
        }

        private string _instalPath;
        public string InstalPathProp
        {
            get
            {
                return _instalPath;
            }
            set
            {
                _instalPath = value;
                OnPropertyChanged("InstalPathProp");
            }
        }

        #endregion

        #region commands

        private RelayCommand _addNewGameComman;
        public RelayCommand AddNewGameComman
        {
            get
            {
                return _addNewGameComman ??
                   (_addNewGameComman = new RelayCommand(obj =>
                   {
                       Game NewGame = new Game { name = "New Game" };
                       Games.Insert(0, NewGame);
                       sqlDBc.Games.Add(NewGame);
                       SelectedGame = NewGame;
                       EditCommand.Execute(SelectedGame);
                       TableChangedProp = true;
                   }));
            }
        }

        private RelayCommand _editCommand;
        public RelayCommand EditCommand
        {
            get
            {
                return _editCommand ??
                   (_editCommand = new RelayCommand(obj =>
                   {
                       TableChangedProp = true;
                       EditTextEnabled = true;
                   }));
            }
        }
        private RelayCommand _doneEditCommand;
        public RelayCommand DoneEditCommand
        {
            get
            {
                return _doneEditCommand ??
                   (_doneEditCommand = new RelayCommand(obj =>
                   {
                       //Games.
                       EditTextEnabled = false;       
                   }));
            }
        }


        //chooseGameNameCommand
        //private RelayCommand chooseGameNameCommand;
        //public RelayCommand ChooseGameNameCommand
        //{
        //    get
        //    {
        //        return chooseGameNameCommand ??
        //           (chooseGameNameCommand = new RelayCommand(obj =>
        //           {
        //               OpenFileDialog myDialog = new OpenFileDialog();
        //               //myDialog.Filter = "Картинки(*.JPG;*.GIF)|*.JPG;*.GIF" + "|Все файлы (*.*)|*.* ";
        //               myDialog.CheckFileExists = true;
        //               myDialog.Multiselect = true;
        //               if (myDialog.ShowDialog() == true)
        //               {
        //                   SelectedGame.name = myDialog.SafeFileName;

        //               }
        //           }));
        //    }
        //}

        private RelayCommand removeComman;
        public RelayCommand RemoveComman
        {
            get
            {
                return removeComman ??
                   (removeComman = new RelayCommand(obj =>
                   {
                       if (SelectedGame != null)
                       { 
                           IEnumerable<InstalledGame> instalationsToRemove;

                           instalationsToRemove = sqlDBc.Games.Where(g => g.name == SelectedGame.name).SelectMany(i => i.InstalledGames).ToList();

                           foreach(InstalledGame i in instalationsToRemove)
                           {
                               sqlDBc.InstalledGames.Remove(i);
                           }

                           sqlDBc.Games.Remove(SelectedGame);
                           Games.Remove(SelectedGame);;
                           SelectedGame = null;
                           TableChangedProp = true;
                       }

                           //sqlDBc.SaveChanges();
                           //Games.Append(new Game { name = "newGame" });
                   }));
            }
        }



        public RelayCommand ReturnKeyPressed
        {
            get; set;
        }

        private RelayCommand saveButEnabCommand;
        public RelayCommand SaveButEnabCommand
        {
            get
            {
                return saveButEnabCommand ??
                   (saveButEnabCommand = new RelayCommand(obj =>
                   {
                       TableChangedProp = true;
                   }));
            }
        }


        private RelayCommand saveDbComman;
        public RelayCommand SaveDbComman
        {
            get
            {
                return saveDbComman ??
                   (saveDbComman = new RelayCommand(obj =>
                   {
                       sqlDBc.SaveChanges();
                       TableChangedProp = false;
                       EditTextEnabled = false;
                   }));
            }
        }

        #endregion



        IEnumerable<Game> games;
        public ObservableCollection<Game> Games { get; set; }
        //public IEnumerable<Game> Games
        //{
        //    get { return games; }
        //    set
        //    {
        //        games = value;
        //        OnPropertyChanged("Games");
        //    }
        //}

        IEnumerable<Machine> _onMachines;
        public IEnumerable<Machine> OnMachines
        {
            get { return _onMachines; }
            set
            {
                _onMachines = value;
                OnPropertyChanged("OnMachines");
            }
        }

        IEnumerable<InstalledGame> _istPaths;
        public IEnumerable<InstalledGame> IstPaths
        {
            get { return _istPaths; }
            set
            {
                _istPaths = value;
                OnPropertyChanged("IstPaths");
            }
        }

        private void GetData()
        {
           
            sqlDBc.Games.Load();
            games = sqlDBc.Games.Local.ToBindingList();
            Games = new ObservableCollection<Game>(games);
        }


        public AllGamesTableViewModel()
        {
            sqlDBc = new SQLiteContext();
            GetData();
            EditTextEnabled = true;
            TableChangedProp = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            if(prop == "SelectedGame")
            {
                if(SelectedGame != null)
                {
                    InstalPathProp = "";
                    SelectedMachine = null; 
                    EditTextEnabled = false;
                    GameNameProp = SelectedGame.name;
                    OnMachines = sqlDBc.Games.Where(g => g.name == SelectedGame.name).SelectMany(i => i.InstalledGames).Select(m => m.Machine).ToList();
                }
            }

            if (prop == "SelectedMachine")
            {
                if (SelectedMachine != null)
                {
                    InstalPathProp = sqlDBc.Machines.Where(m => m.machineName == SelectedMachine.machineName && m.userName == SelectedMachine.userName).SelectMany(i => i.InstalledGames).Where(g => g.Game.name == SelectedGame.name).FirstOrDefault().InstallPath;
                }
            }
        }
    }
}

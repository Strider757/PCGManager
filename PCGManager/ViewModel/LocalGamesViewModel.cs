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
using System.Windows.Controls;
using System.IO;
using System.Windows;
using PCGManager.Views;

namespace PCGManager.ViewModel
{
    public class LocalGamesViewModel : INotifyPropertyChanged
    {
        SQLiteContext sqlDBc;
        private Page addGameFrame;
        private Page editGameFrame;
        private Page gameInfoPage;

        public static RelayCommand cancelButton;
        public static RelayCommand updateButton;
        public static ObservableCollection<Game> LocalGameRef;

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
                OnPropertyChanged("SelectedGame");
            }
        }

        private Page _frame;
        public Page Frame
        {
            get
            {
                return _frame;
            }
            set
            {
                _frame = value;
                OnPropertyChanged("Frame");
            }
        }

        //ShowFrameEditGameCommand

        private RelayCommand _showFrameEditGameCommand;
        public RelayCommand ShowFrameEditGameCommand
        {
            get
            {
                return _showFrameEditGameCommand ??
                    (_showFrameEditGameCommand = new RelayCommand(obj =>
                    {
                        editLocalGameFrameViewModel.GetGame.Execute(SelectedGame);
                        Frame = editGameFrame;
                    }));
            }
        }

        private RelayCommand _showFrameAddGameCommand;
        public RelayCommand ShowFrameAddGameCommand
        {
            get
            {
                return _showFrameAddGameCommand ??
                    (_showFrameAddGameCommand = new RelayCommand(obj =>
                    {
                        Frame = addGameFrame;
                        addGameFrameViewModel.updateCommandRef.Execute(null);
                    }));
            }
        }

        
        private RelayCommand _hideFormCommand;
        public RelayCommand HideFormCommand
        {
            get
            {
                return _hideFormCommand ??
                    (_hideFormCommand = new RelayCommand(obj =>
                    {
                        Frame = null;
                    }));
            }
        }

        //private RelayCommand _editCommand;
        //public RelayCommand EditCommand
        //{
        //    get
        //    {
        //        return _editCommand ??
        //            (_editCommand = new RelayCommand(obj =>
        //            {
                        
        //            }));
        //    }
        //}

        private RelayCommand _removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return _removeCommand ??
                    (_removeCommand = new RelayCommand(obj =>
                    {
                        InstalledGame InsGmToRemove;
                        InsGmToRemove = sqlDBc.Games.Where(g => g.name == SelectedGame.name).SelectMany(i => i.InstalledGames).Where(m => m.Machine.machineName == Global.ThisMachineName && m.Machine.userName == Global.ThisUserName).FirstOrDefault();
                        sqlDBc.InstalledGames.Remove(InsGmToRemove);
                        LocalGames.Remove(SelectedGame);
                        sqlDBc.SaveChanges();
                    }));
            }
        }

        private RelayCommand _updateCommand;
        public RelayCommand UpdateCommand
        {
            get
            {
                return _updateCommand ??
                    (_updateCommand = new RelayCommand(obj =>
                    {
                        GetData();
                    }));
            }
        }

        private string _linkState;
        public string LinkStateProp
        {
            get
            {
                return _linkState;
            }
            set
            {
                _linkState = value;
                OnPropertyChanged("LinkStateProp");
            }
        }

        public bool _linked;
        public bool LinkedProp
        {
            get
            {
                return _linked;
            }
            set
            {
                _linked = value;
                OnPropertyChanged("LinkedProp");
            }
        }

        public bool _backUpShow;
        public bool BackUpShow
        {
            get
            {
                return _backUpShow;
            }
            set
            {
                _backUpShow = value;
                OnPropertyChanged("BackUpShow");
            }
        }

        public bool _incloud;
        public bool InCloudProp
        {
            get
            {
                return _incloud;
            }
            set
            {
                _incloud = value;
                OnPropertyChanged("InCloudProp");
            }
        }

        public bool _linkedRev;
        public bool LinkedRevProp
        {
            get
            {
                return _linkedRev;
            }
            set
            {
                _linkedRev = value;
                OnPropertyChanged("LinkedRevProp");
            }
        }

        private RelayCommand _checkLinkCommand;
        public RelayCommand CheckLinkCommand
        {
            get
            {
                return _checkLinkCommand ??
                    (_checkLinkCommand = new RelayCommand(obj =>
                    {
                        InCloudProp = SaveManager.ChekCloud(MainViewModel.LocalMachine.backupDir + @"\" + SelectedGame.name);
                        LinkedProp = SaveManager.ChekLink(SelectedGameSavePathProp);
                        LinkedRevProp = !LinkedProp;
                        BackUpShow = InCloudProp && LinkedRevProp;
                        if (LinkedProp)
                        {
                            LinkStateProp = "Linked";
                        }
                        else
                        {
                            LinkStateProp = "NOT Linked";
                        }
                    }));
            }
        }

        private RelayCommand _createLinkCommand;
        public RelayCommand CreateLinkCommand
        {
            get
            {
                return _createLinkCommand ??
                    (_createLinkCommand = new RelayCommand(obj =>
                    {
                        SaveManager.Mklink(SelectedGameSavePathProp, MainViewModel.LocalMachine.backupDir + @"\" + SelectedGame.name);
                        CheckLinkCommand.Execute(null);
                    }));
            }
        }

        private RelayCommand _getSaveFromCloudCommand;
        public RelayCommand GetSaveFromCloudCommand
        {
            get
            {
                return _getSaveFromCloudCommand ??
                    (_getSaveFromCloudCommand = new RelayCommand(obj =>
                    {
                        SaveManager.GetSave(SelectedGameSavePathProp, MainViewModel.LocalMachine.backupDir + @"\" + SelectedGame.name);
                        CheckLinkCommand.Execute(null);
                        //SaveManager.Mklink(SelectedGameSavePathProp, MainViewModel.LocalMachine.backupDir + @"\" + SelectedGame.name);
                    }));
            }
        }


        private string _launchOptions;
        public string LaunchOptionsProp
        {
            get
            {
                return _launchOptions;
            }
            set
            {
                _launchOptions = value;
                OnPropertyChanged("LaunchOptionsProp");
            }
                }



        //CreateLinkCommand
        private void GetData()
        {
            sqlDBc = new SQLiteContext();
            sqlDBc.Games.Load();

            _localGames = sqlDBc.Machines.Where(m => m.machineName == Global.ThisMachineName).SelectMany(i => i.InstalledGames).Select(g => g.Game).ToList();
            LocalGames = new ObservableCollection<Game>(_localGames);
        }

        private string _selectedGameInstallPathProp;
        public string SelectedGameInstallPathProp
        {
            get
            {
                return _selectedGameInstallPathProp;
            }
            set
            {
                _selectedGameInstallPathProp = value;
                OnPropertyChanged("SelectedGameInstallPathProp");
            }
        }

        private string _selectedGameSavePathProp;
        public string SelectedGameSavePathProp
        {
            get
            {
                return _selectedGameSavePathProp;
            }
            set
            {
                _selectedGameSavePathProp = value;
                OnPropertyChanged("SelectedGameSavePathProp");
            }
        }
        //SelectedGameSavePathProp

        IEnumerable<Game> _localGames;
        public ObservableCollection<Game> LocalGames { get; set; }
        //public IEnumerable<Game> LocalGames
        //{
        //    get { return _localGames; }
        //    set
        //    {
        //        _localGames = value;
        //        OnPropertyChanged("LocalGame");
        //    }
        //}

        //public ObservableCollection<Game> LocalGames { get; set; }


        public LocalGamesViewModel()
        {
            InCloudProp = true;
           
            addGameFrame = new Views.addGameFrameView();
            editGameFrame = new Views.editLocalGameFrameView();
            //LocalGames = new ObservableCollection<Game>();
            GetData();
            LocalGameRef = LocalGames;
            cancelButton = HideFormCommand;
            updateButton = UpdateCommand;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            if (prop == "SelectedGame")
            {
                HideFormCommand.Execute(null);
                if (SelectedGame != null)
                {
                    
                    SelectedGameInstallPathProp = sqlDBc.InstalledGames.Where(i => i.Game.name == SelectedGame.name && i.Machine.machineName == Global.ThisMachineName && i.Machine.userName == Global.ThisUserName).FirstOrDefault().InstallPath;
                    SelectedGameSavePathProp = sqlDBc.InstalledGames.Where(i => i.Game.name == SelectedGame.name && i.Machine.machineName == Global.ThisMachineName && i.Machine.userName == Global.ThisUserName).FirstOrDefault().GameSavePath;
                    CheckLinkCommand.Execute(null);
                }
            }
        }
    }
}

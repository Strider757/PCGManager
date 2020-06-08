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

        public bool LinkStatusProp { get; set; }

        private RelayCommand _checkLinkCommand;
        public RelayCommand CheckLinkCommand
        {
            get
            {
                return _checkLinkCommand ??
                    (_checkLinkCommand = new RelayCommand(obj =>
                    {
                        //SelectedGameSavePathProp
                        LinkStatusProp = SaveManager.ChekLink(SelectedGameSavePathProp);
                        if (LinkStatusProp)
                        {
                            MessageBox.Show("збс");
                        }
                        else
                        {
                            MessageBox.Show("итс бэд");
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
                    }));
            }
        }

        //CreateLinkCommand
        private void GetData()
        {
            
            sqlDBc.Games.Load();

            _localGames = sqlDBc.Machines.Where(m => m.machineName == Global.ThisMachineName).SelectMany(i => i.InstalledGames).Select(g => g.Game).ToList();
            //  LocalGames.Clear();
            //LocalGames = _localGames as ObservableCollection<Game>;
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
        public static ObservableCollection<Game> LocalGames { get; set; }
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
            sqlDBc = new SQLiteContext();
            addGameFrame = new Views.addGameFrameView();
            editGameFrame = new Views.editLocalGameFrameView();
            //LocalGames = new ObservableCollection<Game>();
            GetData();
            cancelButton = HideFormCommand;
            //updateButton = UpdateCommand;
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
                    SelectedGameInstallPathProp = sqlDBc.InstalledGames.Where(i => i.Game.name == SelectedGame.name).FirstOrDefault().InstallPath;
                    SelectedGameSavePathProp = sqlDBc.InstalledGames.Where(i => i.Game.name == SelectedGame.name).FirstOrDefault().GameSavePath;
                }
            }
        }
    }
}

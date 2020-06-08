//using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace PCGManager.ViewModel
{
    public class addGameFrameViewModel : INotifyPropertyChanged
    {
        SQLiteContext sqlDBc;
        public static RelayCommand updateCommandRef;
        IEnumerable<Game> _games;
        public IEnumerable<Game> Games
        {
            get { return _games; }
            set
            {
                _games = value;
                OnPropertyChanged("Games");
            }
        }

        //IEnumerable<Game> _notInstalledGames;
        //public IEnumerable<Game> NotInstalledGames
        //{
        //    get { return _notInstalledGames; }
        //    set
        //    {
        //        _notInstalledGames = value;
        //        OnPropertyChanged("NotInstalledGames");
        //    }
        //}

        //IEnumerable<Game> _notLocalGame;
        //public IEnumerable<Game> NotLocalGame
        //{
        //    get { return _notLocalGame; }
        //    set
        //    {
        //        _notLocalGame = value;
        //        OnPropertyChanged("NotLocalGame");
        //    }
        //}

        //IEnumerable<Machine> _tm;
        //public IEnumerable<Machine> tm
        //{
        //    get { return _tm; }
        //    set
        //    {
        //        _tm = value;
        //        OnPropertyChanged("tm");
        //    }
        //}


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


        private string _installPath;
        public string InstallPathProp
        {
            get
            {
                return _installPath;
            }
            set
            {
                _installPath = value;
                OnPropertyChanged("InstallPathProp");
            }
        }

        private string _gameSavePath;
        public string GameSavePathProp
        {
            get
            {
                return _gameSavePath;
            }
            set
            {
                _gameSavePath = value;
                OnPropertyChanged("GameSavePathProp");
            }
        }

        private RelayCommand _addComman;
        public RelayCommand AddComman
        {
            get
            {
                return _addComman ??
                   (_addComman = new RelayCommand(obj =>
                   {
                       Machine locMachine = new Machine();
                       locMachine = sqlDBc.Machines.Where(m => m.machineName == Global.ThisMachineName && m.userName == Global.ThisUserName).Single();
                       Game existGame;
                       existGame = sqlDBc.Machines.SelectMany(i => i.InstalledGames).Where(g => g.Game.name == SelectedGame.name && g.Machine.machineName == locMachine.machineName && g.Machine.userName == locMachine.userName ).Select(g => g.Game).FirstOrDefault(); // locMachine.InstalledGames.Select(i => i.Game).Where(g => g.name == SelectedGame.name).FirstOrDefault();
                       if (existGame  != null)
                       {
                           System.Windows.MessageBox.Show("Game is allready added on this machine!");
                       }
                       else
                       {
                           InstalledGame newInst = new InstalledGame { InstallPath = InstallPathProp, GameSavePath = GameSavePathProp, Game = SelectedGame, Machine = locMachine };
                           sqlDBc.InstalledGames.Add(newInst);
                           sqlDBc.SaveChanges();
                         
                           LocalGamesViewModel.LocalGameRef.Add(SelectedGame);
                           //LocalGamesViewModel.LocalGames.Add(SelectedGame);
                           LocalGamesViewModel.cancelButton.Execute(null);
                       }
                   }));
            }
        }

        private RelayCommand _chooseComman;
        public RelayCommand ChooseComman
        {
            get
            {
                return _chooseComman ??
                    (_chooseComman = new RelayCommand(obj =>
                    {
                        OpenFileDialog myDialog = new OpenFileDialog();
                        //myDialog.Filter = "Картинки(*.JPG;*.GIF)|*.JPG;*.GIF" + "|Все файлы (*.*)|*.* ";
                        myDialog.CheckFileExists = true;
                        myDialog.Multiselect = true;
                        
                        if (myDialog.ShowDialog() != null)
                        {
                            InstallPathProp = myDialog.FileName;
                        }
                    }));
            }
        }

        //ChooseGameSaveFolderComman
        private RelayCommand _chooseGameSaveFolderComman;
        public RelayCommand ChooseGameSaveFolderComman
        {
            get
            {
                return _chooseGameSaveFolderComman ??
                    (_chooseGameSaveFolderComman = new RelayCommand(obj =>
                    {
                        //Microsoft.Win32.
                        //System.Windows.Forms
                        FolderBrowserDialog myDialog = new FolderBrowserDialog();
                        //OpenFileDialog myDialog = new OpenFileDialog();
                        //myDialog.
                        //myDialog.Filter = "Картинки(*.JPG;*.GIF)|*.JPG;*.GIF" + "|Все файлы (*.*)|*.* ";
                        //myDialog.CheckFileExists = true;
                        //myDialog.Multiselect = true;
                        if (myDialog.ShowDialog() != null)
                        {
                            GameSavePathProp = myDialog.SelectedPath;
                        }
                    }));
            }
        }


        private RelayCommand _cancelComman;
        public RelayCommand CancelComman
        {
            get
            {
                return _cancelComman ??
                   (_cancelComman = new RelayCommand(obj =>
                   {
                       LocalGamesViewModel.cancelButton.Execute(null);
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
                       Games = sqlDBc.Games.ToList();
                   }));
            }
        }

        public static void getdata()
        {
            
        }

        public addGameFrameViewModel()
        {
            sqlDBc = new SQLiteContext();
            updateCommandRef = UpdateCommand;
            Games = sqlDBc.Games.ToList();

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            if (prop == "SelectedGame")
            {
                InstallPathProp = "";
                GameSavePathProp = "";
            }
        }
    }
}

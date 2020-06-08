using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PCGManager.ViewModel
{
   public class editLocalGameFrameViewModel : INotifyPropertyChanged
    {
        public static RelayCommand GetGame;

        private RelayCommand _getGame;
        public RelayCommand GetGameCommand
        {
            get
            {
                return _getGame ??
                    (_getGame = new RelayCommand(obj =>
                    {
                        CurGameProp = obj as Game;
                    }));
            }
        }

        private Game _curGame;
        public Game CurGameProp
        {
            get
            {
                return _curGame;
            }
            set
            {
                _curGame = value;
                OnPropertyChanged("CurGameProp");
            }
        }


        public editLocalGameFrameViewModel()
        {
            GetGame = GetGameCommand;
            //_curGame = new Game { name = "pizda" };
        }
        public editLocalGameFrameViewModel(Game game)
        {
            //_curGame = game;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));

        }

    }
}

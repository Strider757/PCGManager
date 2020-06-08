using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCGManager.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {

        SQLiteContext sqlDBc;
        private string _backupDirProp;
        public string BackupDirProp
        {
            get
            {
                return _backupDirProp;
            }
            set
            {
                _backupDirProp = value;
                OnPropertyChanged("BackupDirProp");
            }
        }


        private RelayCommand _selectFolderCommand;
        public RelayCommand SelectFolderCommand
        {
            get
            {
                return _selectFolderCommand ??
                    (_selectFolderCommand = new RelayCommand(obj =>
                    {
                        FolderBrowserDialog myDialog = new FolderBrowserDialog();
                        if (myDialog.ShowDialog() != null)
                        {
                            BackupDirProp = myDialog.SelectedPath;
                        }
                    }));
            }
        }

        private RelayCommand _saveSetCommand;
        public RelayCommand SaveSetCommand
        {
            get
            {
                return _saveSetCommand ??
                    (_saveSetCommand = new RelayCommand(obj =>
                    {
                        Machine locMachine = new Machine();
                        locMachine = sqlDBc.Machines.Where(m => m.machineName == Global.ThisMachineName && m.userName == Global.ThisUserName).FirstOrDefault();
                        locMachine.backupDir = BackupDirProp;
                        sqlDBc.SaveChanges();
                    }));
            }
        }

        public SettingsViewModel()
        {
            sqlDBc = new SQLiteContext();
            Machine m1 = new Machine();
            //m1 
            BackupDirProp = sqlDBc.Machines.Where(m => m.machineName == Global.ThisMachineName && m.userName == Global.ThisUserName).FirstOrDefault().backupDir;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));

        }
    }
}

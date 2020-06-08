using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCGManager
{
    public class SaveManager
    {
        [DllImport("kernel32.dll")]
        static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags);

        enum SymbolicLink
        {
            File = 0,
            Directory = 1
        }

        public static void GetSave(string saveDirectory, string backupDirectory)
        {
            DirectoryInfo saveDirectoryInfo = new DirectoryInfo(saveDirectory);
            DirectoryInfo backupDirectoryyInfo = new DirectoryInfo(backupDirectory);
            string strCmdText;
            strCmdText = "/c mklink /j " + '\u0022' + saveDirectory + '\u0022' + " " + '\u0022' + backupDirectory + '\u0022';
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "CMD.exe";
            startInfo.Arguments = strCmdText;
            if (saveDirectoryInfo.Exists)
            {
                try
                {
                    saveDirectoryInfo.Delete(true);
                }
                catch(IOException)
                {
                    MessageBox.Show("Не удалось удалить папку сохранения для создания символьной ссылки. Удали позязя пручную, ска!","Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            Process.Start(startInfo);
        }

        public static void  Mklink(string saveDirectory, string backupDirectory)
        {
            DirectoryInfo saveDirectoryInfo = new DirectoryInfo(saveDirectory);
            DirectoryInfo backupDirectoryyInfo = new DirectoryInfo(backupDirectory);

            if (saveDirectoryInfo.Exists)
            {
                //MessageBox
            }

                if (saveDirectoryInfo.Exists)
            {
                string strCmdText;
                Copy(saveDirectory, backupDirectory);
                saveDirectoryInfo.Delete(true);
                strCmdText = "/c mklink /j " + '\u0022' + saveDirectory  + '\u0022' + " " + '\u0022' + backupDirectory + '\u0022';
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "CMD.exe";
                startInfo.Arguments = strCmdText;
                Process.Start(startInfo);
            }
            else
            {

            }
        }

        public static bool ChekCloud(string saveInCloudDir)
        {
            DirectoryInfo dir = new DirectoryInfo(saveInCloudDir);

            return dir.Exists;
        }

        public static bool ChekLink(string linkDirectory)
        {
            if (linkDirectory != "")
            {
                DirectoryInfo dir = new DirectoryInfo(linkDirectory);

                if (!dir.Exists)
                    return false;

                if (((int)dir.Attributes & (int)FileAttributes.ReparsePoint) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Если директория target.FullName не существует, создать ее
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Копируем файлы из sourceDirectory в targetDirectory
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }
            //копируем поддиректории
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        //public static void Main()
        //{
        //    string sourceDirectory = @"c:\sourceDirectory";
        //    string targetDirectory = @"c:\targetDirectory";

        //    Copy(sourceDirectory, targetDirectory);
        //}
    }
}

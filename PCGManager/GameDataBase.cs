using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Data.SQLite;

namespace PCGManager
{
    
    public class GameDataBase
    {
        private string dbName = "pcgmanager.db";
        public bool DataBaseExist { get { return DataBaseExistCheck(); } }

        //SQLiteConnection db_con = new SQLiteConnection();
        SQLiteCommand db_cmd = new SQLiteCommand();


        private bool DataBaseExistCheck()
        {
            FileInfo db_fi = new FileInfo(Environment.CurrentDirectory + @"\" + dbName);
            return db_fi.Exists;
        }

        public bool DataBaseCreate()
        {
            SQLiteConnection.CreateFile(dbName);
            if (!DataBaseExist)
            {
                return false;
            }

            SQLiteConnection db_con = new SQLiteConnection("Data Source=" + dbName + ";Version=3;");
            db_con.Open();
            db_cmd.Connection = db_con;
            // "CREATE TABLE IF NOT EXISTS PCs (id INTEGER PRIMARY KEY AUTOINCREMENT, author TEXT, book TEXT)";

            db_cmd.CommandText = "CREATE TABLE IF NOT EXISTS Games (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, name TEXT)";
            db_cmd.ExecuteNonQuery();

            db_cmd.CommandText = "CREATE TABLE IF NOT EXISTS Machines (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, machineName TEXT, userName TEXT, backupDir TEXT)";
            db_cmd.ExecuteNonQuery();

            //db_cmd.CommandText = "CREATE TABLE IF NOT EXISTS MachineGames (Machine_id INTEGER, Game_id INTEGER, PRIMARY KEY (Machine_id, Game_id), FOREIGN KEY(game_id) REFERENCES games(id), FOREIGN KEY(machine_id) REFERENCES machines(id))";

            db_cmd.CommandText = "CREATE TABLE IF NOT EXISTS InstalledGames (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, GameSavePath TEXT, InstallPath TEXT, Machine_id INTEGER, Game_id INTEGER,  FOREIGN KEY(Game_id) REFERENCES Games(id), FOREIGN KEY(Machine_id) REFERENCES Machines(id))";
            db_cmd.ExecuteNonQuery();

            db_con.Close();

            //DBConnect();
            return true;
        }

        //void DBConnect()
        //{
        //    SQLiteConnection db_con = new SQLiteConnection("Data Source=" + dbName + ";Version=3;");
        //    db_con.Open();
        //    db_cmd.Connection = db_con;
        //                      // "CREATE TABLE IF NOT EXISTS PCs (id INTEGER PRIMARY KEY AUTOINCREMENT, author TEXT, book TEXT)";

        //    db_cmd.CommandText = "CREATE TABLE IF NOT EXISTS Games (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, name TEXT)";
        //    db_cmd.ExecuteNonQuery();

        //    db_cmd.CommandText = "CREATE TABLE IF NOT EXISTS Machines (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, machineName TEXT, userName TEXT, backupDir TEXT)";
        //    db_cmd.ExecuteNonQuery();

        //    //db_cmd.CommandText = "CREATE TABLE IF NOT EXISTS MachineGames (Machine_id INTEGER, Game_id INTEGER, PRIMARY KEY (Machine_id, Game_id), FOREIGN KEY(game_id) REFERENCES games(id), FOREIGN KEY(machine_id) REFERENCES machines(id))";

        //    db_cmd.CommandText = "CREATE TABLE IF NOT EXISTS InstalledGames (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, GameSavePath TEXT, InstallPath TEXT, Machine_id INTEGER, Game_id INTEGER,  FOREIGN KEY(Game_id) REFERENCES Games(id), FOREIGN KEY(Machine_id) REFERENCES Machines(id))";
        //    db_cmd.ExecuteNonQuery();

        //    db_con.Close();
        //    //  db_cmd;
        //    // m_dbConn.con
        //}

        //bool CreateTable()
        //{

        //    return true;
        //}




    }

}

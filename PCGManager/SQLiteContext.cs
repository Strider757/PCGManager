using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCGManager
{
    public class SQLiteContext : DbContext
    {
        // Имя будущей базы данных можно указать через
        // вызов конструктора базового класса
        //private string localGameTableName;
        public SQLiteContext() : base("sqliteDB")
        {
            //localGameTableName = value;
        }
        // Отражение таблиц базы данных на свойства с типом DbSet
        public DbSet<Machine> Machines {get; set;}
        public DbSet<Game> Games {get; set;}

        public DbSet<InstalledGame> InstalledGames { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{

        //    //modelBuilder.Entity<Machine>().HasMany(a => a.MachineGame).WithRequired(ac => ac.Machine).HasForeignKey(ac => ac.Machine_id);
        //    //modelBuilder.Entity<Game>().HasMany(c => c.MachineGame).WithRequired(ac => ac.Game).HasForeignKey(ac => ac.Game_id);

        //    //modelBuilder.Entity<MachineGame>().ToTable("ArticlesCompanies");
        //    //modelBuilder.Entity<MachineGame>().HasKey(ac => new { ac.Machine_id, ac.Game_id });


        //    //modelBuilder.Entity<Machine>().HasMany(g => g.Games).WithMany(m => m.Machines).Map(m =>
        //    modelBuilder.Entity<Game>().HasMany(g => g.Machines).WithMany(m => m.Games).Map(m =>
        //    {
        //          m.ToTable("InstalledGames");
        //          m.MapLeftKey("Game_id");
        //          m.MapRightKey("Machine_id");
        //      });


        //    base.OnModelCreating(modelBuilder);
        //}

    }
}

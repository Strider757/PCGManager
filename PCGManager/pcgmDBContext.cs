namespace PCGManager
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class pcgmDBContext : DbContext
    {
        // Your context has been configured to use a 'pcgmDB' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'PCGManager.pcgmDB' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'pcgmDB' 
        // connection string in the application configuration file.
        public pcgmDBContext() : base("name=pcgmDB")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public DbSet<Machine> Machines { get; set; }
        public DbSet<Game> Games { get; set; }

        public DbSet<InstalledGame> InstalledGames { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}
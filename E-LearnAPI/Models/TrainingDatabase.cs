namespace E_LearnAPI.Models
{
    using E_LearnAPI.Models.EntityConfigurations;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class TrainingDatabase : DbContext
    {
        // Your context has been configured to use a 'TrainingDatabase' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'E_LearnAPI.Models.TrainingDatabase' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'TrainingDatabase' 
        // connection string in the application configuration file.
        public TrainingDatabase()
            : base("name=TrainingDatabase")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<ELResult> ELResults { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<Requirement> Requirements { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ELResultsConfiguration());
            modelBuilder.Configurations.Add(new PersonConfiguration());
            modelBuilder.Configurations.Add(new CourseConfiguration());
            modelBuilder.Configurations.Add(new StatusConfiguration());
            modelBuilder.Configurations.Add(new RequirementConfiguration());
        }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}
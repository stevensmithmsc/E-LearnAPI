namespace E_LearnAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class apparently_needs_another_migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("train.ESRModules", "Received", c => c.DateTime(nullable: false));
            DropColumn("train.ESRModules", "Recieved");
        }
        
        public override void Down()
        {
            AddColumn("train.ESRModules", "Recieved", c => c.DateTime(nullable: false));
            DropColumn("train.ESRModules", "Received");
        }
    }
}

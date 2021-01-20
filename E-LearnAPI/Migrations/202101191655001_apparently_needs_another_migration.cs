namespace E_LearnAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class apparently_needs_another_migration : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE train.ESRModules  ADD CONSTRAINT DF_ESRMod_Rec  DEFAULT GETDATE() FOR Received");
        }
        
        public override void Down()
        {
            
        }
    }
}

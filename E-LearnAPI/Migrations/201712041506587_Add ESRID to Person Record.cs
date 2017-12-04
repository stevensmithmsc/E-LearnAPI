namespace E_LearnAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddESRIDtoPersonRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("train.Staff", "ESRID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("train.Staff", "ESRID");
        }
    }
}

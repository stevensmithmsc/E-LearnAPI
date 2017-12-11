namespace E_LearnAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeAccessLevelColumnName : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "train.Report_Access", name: "AccessLevel", newName: "ELearn");
        }
        
        public override void Down()
        {
            RenameColumn(table: "train.Report_Access", name: "ELearn", newName: "AccessLevel");
        }
    }
}

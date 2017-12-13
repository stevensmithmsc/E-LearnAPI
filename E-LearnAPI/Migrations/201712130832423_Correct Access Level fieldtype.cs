namespace E_LearnAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrectAccessLevelfieldtype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("train.Report_Access", "ELearn", c => c.Byte(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            AlterColumn("train.Report_Access", "ELearn", c => c.Short(nullable: false));
        }
    }
}

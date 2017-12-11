namespace E_LearnAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReportAccessTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "train.Report_Access",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        AccessLevel = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("train.Staff", t => t.ID)
                .Index(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("train.Report_Access", "ID", "train.Staff");
            DropIndex("train.Report_Access", new[] { "ID" });
            DropTable("train.Report_Access");
        }
    }
}

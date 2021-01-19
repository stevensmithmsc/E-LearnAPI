namespace E_LearnAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ESR_Modules_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "train.ESRModules",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Employee = c.Int(nullable: false),
                        StaffID = c.Int(),
                        ModuleName = c.String(nullable: false, maxLength: 100, unicode: false),
                        CourseID = c.Int(),
                        CompletionDate = c.DateTime(nullable: false),
                        Processed = c.Boolean(nullable: false),
                        Comments = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("train.Courses", t => t.CourseID)
                .ForeignKey("train.Staff", t => t.StaffID)
                .Index(t => t.StaffID)
                .Index(t => t.CourseID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("train.ESRModules", "StaffID", "train.Staff");
            DropForeignKey("train.ESRModules", "CourseID", "train.Courses");
            DropIndex("train.ESRModules", new[] { "CourseID" });
            DropIndex("train.ESRModules", new[] { "StaffID" });
            DropTable("train.ESRModules");
        }
    }
}

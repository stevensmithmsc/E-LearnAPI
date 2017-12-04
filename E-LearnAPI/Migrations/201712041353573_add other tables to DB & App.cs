namespace E_LearnAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addothertablestoDBApp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "train.Courses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Course = c.String(nullable: false, maxLength: 80, unicode: false),
                        Paris = c.Boolean(nullable: false),
                        Child_Health = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "train.Req",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Staff = c.Int(nullable: false),
                        Course = c.Int(nullable: false),
                        Status = c.Short(nullable: false),
                        Comments = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("train.Courses", t => t.Course, cascadeDelete: true)
                .ForeignKey("train.Staff", t => t.Staff, cascadeDelete: true)
                .ForeignKey("train.Statuses", t => t.Status, cascadeDelete: true)
                .Index(t => t.Staff)
                .Index(t => t.Course)
                .Index(t => t.Status);
            
            CreateTable(
                "train.Staff",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Fname = c.String(maxLength: 35, unicode: false),
                        Sname = c.String(nullable: false, maxLength: 35, unicode: false),
                        ADAccount = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "train.Statuses",
                c => new
                    {
                        ID = c.Short(nullable: false, identity: true),
                        Status = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ID);

            Sql("SET IDENTITY_INSERT train.Statuses ON");
            Sql("INSERT INTO train.Statuses (Id, Status) VALUES (1, 'Required')");
            Sql("INSERT INTO train.Statuses (Id, Status) VALUES (2, 'Booked')");
            Sql("INSERT INTO train.Statuses (Id, Status) VALUES (5, 'Completed')");
            Sql("INSERT INTO train.Statuses (Id, Status) VALUES (7, 'No Longer Required')");
            Sql("SET IDENTITY_INSERT train.Statuses OFF");
        }
        
        public override void Down()
        {
            DropForeignKey("train.Req", "Status", "train.Statuses");
            DropForeignKey("train.Req", "Staff", "train.Staff");
            DropForeignKey("train.Req", "Course", "train.Courses");
            DropIndex("train.Req", new[] { "Status" });
            DropIndex("train.Req", new[] { "Course" });
            DropIndex("train.Req", new[] { "Staff" });
            DropTable("train.Statuses");
            DropTable("train.Staff");
            DropTable("train.Req");
            DropTable("train.Courses");
        }
    }
}

namespace E_LearnAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_module_course_mapping_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "train.CourseModuleMap",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ESRModuleName = c.String(nullable: false, maxLength: 100, unicode: false),
                        CourseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("train.Courses", t => t.CourseID, cascadeDelete: true)
                .Index(t => t.CourseID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("train.CourseModuleMap", "CourseID", "train.Courses");
            DropIndex("train.CourseModuleMap", new[] { "CourseID" });
            DropTable("train.CourseModuleMap");
        }
    }
}

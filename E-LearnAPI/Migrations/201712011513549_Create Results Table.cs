namespace E_LearnAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateResultsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "train.ELearning",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PersonId = c.Int(nullable: false),
                        PersonName = c.String(nullable: false, maxLength: 100, unicode: false),
                        CourseId = c.Int(nullable: false),
                        CourseDesc = c.String(nullable: false, maxLength: 100, unicode: false),
                        Score = c.Int(nullable: false),
                        MaxScore = c.Int(),
                        PassFail = c.Boolean(),
                        Received = c.DateTime(nullable: false),
                        FromADAcc = c.String(maxLength: 100, unicode: false),
                        Processed = c.Boolean(nullable: false),
                        Comments = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("train.ELearning");
        }
    }
}

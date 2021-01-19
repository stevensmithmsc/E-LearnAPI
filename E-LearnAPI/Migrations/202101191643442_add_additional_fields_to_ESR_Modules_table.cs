namespace E_LearnAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_additional_fields_to_ESR_Modules_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("train.Staff", "EMail", c => c.String(maxLength: 80, unicode: false));
            AddColumn("train.ESRModules", "UserName", c => c.String(maxLength: 80, unicode: false));
            AddColumn("train.ESRModules", "Received", c => c.DateTime(nullable: true));
            AddColumn("train.ESRModules", "Source", c => c.String(nullable: true, maxLength: 100, unicode: false));
            AlterColumn("train.ESRModules", "CompletionDate", c => c.DateTime(nullable: false, storeType: "date"));

            Sql("Update train.ESRModules set Received = getdate(), Source = 'Extracted from ESR' ");

            AlterColumn("train.ESRModules", "Received", c => c.DateTime(nullable: false));
            AlterColumn("train.ESRModules", "Source", c => c.String(nullable: false, maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("train.ESRModules", "CompletionDate", c => c.DateTime(nullable: false));
            DropColumn("train.ESRModules", "Source");
            DropColumn("train.ESRModules", "Received");
            DropColumn("train.ESRModules", "UserName");
            DropColumn("train.Staff", "EMail");
        }
    }
}

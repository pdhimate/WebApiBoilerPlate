namespace Api.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedExceptionEntry : DbMigration
    {
        public override void Up()
        {
            AddColumn("Meta.ExceptionEntries", "Method", c => c.String());
            DropColumn("Meta.ExceptionEntries", "TargetSite");
        }
        
        public override void Down()
        {
            AddColumn("Meta.ExceptionEntries", "TargetSite", c => c.String());
            DropColumn("Meta.ExceptionEntries", "Method");
        }
    }
}

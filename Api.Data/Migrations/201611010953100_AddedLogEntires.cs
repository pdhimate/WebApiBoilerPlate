namespace Api.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLogEntires : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Meta.LogEntries",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        MachineName = c.String(),
                        ExceptionId = c.String(maxLength: 128),
                        HttpRequestId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Meta.ExceptionEntries", t => t.ExceptionId)
                .ForeignKey("Meta.HttpRequestMessageEntries", t => t.HttpRequestId)
                .Index(t => t.ExceptionId)
                .Index(t => t.HttpRequestId);
            
            CreateTable(
                "Meta.ExceptionEntries",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        HResult = c.Int(nullable: false),
                        Message = c.String(),
                        Source = c.String(),
                        StackTrace = c.String(),
                        TargetSite = c.String(),
                        InnnerExceptionId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Meta.ExceptionEntries", t => t.InnnerExceptionId)
                .Index(t => t.InnnerExceptionId);
            
            CreateTable(
                "Meta.HttpRequestMessageEntries",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Method = c.String(),
                        RequestOriginalUri = c.String(),
                        IdnHost = c.String(),
                        Headers = c.String(),
                        UserName = c.String(),
                        IpAddress = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Meta.LogEntries", "HttpRequestId", "Meta.HttpRequestMessageEntries");
            DropForeignKey("Meta.LogEntries", "ExceptionId", "Meta.ExceptionEntries");
            DropForeignKey("Meta.ExceptionEntries", "InnnerExceptionId", "Meta.ExceptionEntries");
            DropIndex("Meta.ExceptionEntries", new[] { "InnnerExceptionId" });
            DropIndex("Meta.LogEntries", new[] { "HttpRequestId" });
            DropIndex("Meta.LogEntries", new[] { "ExceptionId" });
            DropTable("Meta.HttpRequestMessageEntries");
            DropTable("Meta.ExceptionEntries");
            DropTable("Meta.LogEntries");
        }
    }
}

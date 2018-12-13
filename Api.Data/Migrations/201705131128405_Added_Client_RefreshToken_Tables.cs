namespace Api.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Client_RefreshToken_Tables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Security.Clients",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50),
                        Secret = c.String(),
                        Name = c.String(maxLength: 100),
                        ClientType = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        RefreshTokenLifeTime = c.Long(nullable: false),
                        AllowedOrigin = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Security.RefreshTokens",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IssuedOnUtc = c.DateTime(nullable: false),
                        ExpiresOnUtc = c.DateTime(nullable: false),
                        ProtectedTicket = c.String(nullable: false),
                        ClientId = c.String(nullable: false, maxLength: 50),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Security.Clients", t => t.ClientId, cascadeDelete: false)
                .ForeignKey("Security.AppUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.ClientId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Security.RefreshTokens", "UserId", "Security.AppUsers");
            DropForeignKey("Security.RefreshTokens", "ClientId", "Security.Clients");
            DropIndex("Security.RefreshTokens", new[] { "UserId" });
            DropIndex("Security.RefreshTokens", new[] { "ClientId" });
            DropTable("Security.RefreshTokens");
            DropTable("Security.Clients");
        }
    }
}

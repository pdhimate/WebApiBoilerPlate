namespace Api.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Security.Addresses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Street = c.String(),
                        Landmark = c.String(),
                        BuildingName = c.String(),
                        AreaName = c.String(),
                        ZipCode = c.String(maxLength: 12),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        Latitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Longitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MapLink = c.String(),
                        AddressString = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Security.AppUserAppRoleMappings",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("Security.AppRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("Security.AppUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "Security.AppRoles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "Security.AppUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CreatedOnUtc = c.DateTime(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        DefaultAddressId = c.Long(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Security.Addresses", t => t.DefaultAddressId)
                .Index(t => t.DefaultAddressId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "Security.AppUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Security.AppUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "Security.ExternalUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("Security.AppUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Security.AppUserAppRoleMappings", "UserId", "Security.AppUsers");
            DropForeignKey("Security.ExternalUserLogins", "UserId", "Security.AppUsers");
            DropForeignKey("Security.AppUserClaims", "UserId", "Security.AppUsers");
            DropForeignKey("Security.AppUsers", "DefaultAddressId", "Security.Addresses");
            DropForeignKey("Security.AppUserAppRoleMappings", "RoleId", "Security.AppRoles");
            DropIndex("Security.ExternalUserLogins", new[] { "UserId" });
            DropIndex("Security.AppUserClaims", new[] { "UserId" });
            DropIndex("Security.AppUsers", "UserNameIndex");
            DropIndex("Security.AppUsers", new[] { "DefaultAddressId" });
            DropIndex("Security.AppRoles", "RoleNameIndex");
            DropIndex("Security.AppUserAppRoleMappings", new[] { "RoleId" });
            DropIndex("Security.AppUserAppRoleMappings", new[] { "UserId" });
            DropTable("Security.ExternalUserLogins");
            DropTable("Security.AppUserClaims");
            DropTable("Security.AppUsers");
            DropTable("Security.AppRoles");
            DropTable("Security.AppUserAppRoleMappings");
            DropTable("Security.Addresses");
        }
    }
}

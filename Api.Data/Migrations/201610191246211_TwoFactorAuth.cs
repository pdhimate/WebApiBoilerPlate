namespace Api.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TwoFactorAuth : DbMigration
    {
        public override void Up()
        {
            AddColumn("Security.AppUsers", "TwoFactorPreSharedKey", c => c.String(maxLength: 16));
        }
        
        public override void Down()
        {
            DropColumn("Security.AppUsers", "TwoFactorPreSharedKey");
        }
    }
}

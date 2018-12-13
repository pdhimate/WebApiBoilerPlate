namespace Api.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Deduplicated_Address_Table_Columns : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Security.Addresses", "CountryId", "Master.Countries");
            DropForeignKey("Security.Addresses", "StateId", "Master.States");
            DropIndex("Security.Addresses", new[] { "CountryId" });
            DropIndex("Security.Addresses", new[] { "StateId" });
            DropColumn("Security.Addresses", "CountryId");
            DropColumn("Security.Addresses", "StateId");
        }
        
        public override void Down()
        {
            AddColumn("Security.Addresses", "StateId", c => c.Long(nullable: false));
            AddColumn("Security.Addresses", "CountryId", c => c.Long(nullable: false));
            CreateIndex("Security.Addresses", "StateId");
            CreateIndex("Security.Addresses", "CountryId");
            AddForeignKey("Security.Addresses", "StateId", "Master.States", "Id", cascadeDelete: false);
            AddForeignKey("Security.Addresses", "CountryId", "Master.Countries", "Id", cascadeDelete: false);
        }
    }
}

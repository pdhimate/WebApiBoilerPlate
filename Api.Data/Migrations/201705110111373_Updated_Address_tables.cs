namespace Api.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Updated_Address_tables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Master.Cities",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 400),
                    StateId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id, clustered: false)
                .ForeignKey("Master.States", t => t.StateId, cascadeDelete: false)
                .Index(t => t.StateId, clustered: true);

            CreateTable(
                "Master.States",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 400),
                    CountryId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id, clustered: false)
                .ForeignKey("Master.Countries", t => t.CountryId, cascadeDelete: false)
                .Index(t => t.CountryId, clustered: true);

            CreateTable(
                "Master.Countries",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 400),
                })
                .PrimaryKey(t => t.Id, clustered: false)
                .Index(t => t.Name, unique: true, clustered: true);

            AddColumn("Security.Addresses", "CountryId", c => c.Long(nullable: false));
            AddColumn("Security.Addresses", "StateId", c => c.Long(nullable: false));
            AddColumn("Security.Addresses", "CityId", c => c.Long(nullable: false));
            AlterColumn("Security.Addresses", "ZipCode", c => c.String(nullable: false, maxLength: 12));
            AlterColumn("Security.Addresses", "Latitude", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("Security.Addresses", "Longitude", c => c.Decimal(precision: 18, scale: 2));
            CreateIndex("Security.Addresses", "CountryId");
            CreateIndex("Security.Addresses", "StateId");
            CreateIndex("Security.Addresses", "CityId");
            AddForeignKey("Security.Addresses", "CityId", "Master.Cities", "Id", cascadeDelete: false);
            AddForeignKey("Security.Addresses", "CountryId", "Master.Countries", "Id", cascadeDelete: false);
            AddForeignKey("Security.Addresses", "StateId", "Master.States", "Id", cascadeDelete: false);
            DropColumn("Security.Addresses", "City");
            DropColumn("Security.Addresses", "State");
            DropColumn("Security.Addresses", "Country");
        }

        public override void Down()
        {
            AddColumn("Security.Addresses", "Country", c => c.String());
            AddColumn("Security.Addresses", "State", c => c.String());
            AddColumn("Security.Addresses", "City", c => c.String());
            DropForeignKey("Security.Addresses", "StateId", "Master.States");
            DropForeignKey("Security.Addresses", "CountryId", "Master.Countries");
            DropForeignKey("Security.Addresses", "CityId", "Master.Cities");
            DropForeignKey("Master.Cities", "StateId", "Master.States");
            DropForeignKey("Master.States", "CountryId", "Master.Countries");
            DropIndex("Master.Countries", new[] { "Name" });
            DropIndex("Master.States", new[] { "CountryId" });
            DropIndex("Master.Cities", new[] { "StateId" });
            DropIndex("Security.Addresses", new[] { "CityId" });
            DropIndex("Security.Addresses", new[] { "StateId" });
            DropIndex("Security.Addresses", new[] { "CountryId" });
            AlterColumn("Security.Addresses", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("Security.Addresses", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("Security.Addresses", "ZipCode", c => c.String(maxLength: 12));
            DropColumn("Security.Addresses", "CityId");
            DropColumn("Security.Addresses", "StateId");
            DropColumn("Security.Addresses", "CountryId");
            DropTable("Master.Countries");
            DropTable("Master.States");
            DropTable("Master.Cities");
        }
    }
}

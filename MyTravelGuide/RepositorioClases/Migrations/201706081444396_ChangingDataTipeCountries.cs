namespace RepositorioClases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingDataTipeCountries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        CountryName = c.String(maxLength: 100),
                        alpha2 = c.String(maxLength: 2),
                        alpha3 = c.String(maxLength: 3),
                    })
                .PrimaryKey(t => t.CountryId);
            
            DropColumn("dbo.TravelGuides", "EventState");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TravelGuides", "EventState", c => c.Int(nullable: false));
            DropTable("dbo.Countries");
        }
    }
}

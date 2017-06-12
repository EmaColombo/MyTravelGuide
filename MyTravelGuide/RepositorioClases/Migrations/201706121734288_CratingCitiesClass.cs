namespace RepositorioClases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CratingCitiesClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        CityName = c.String(nullable: false, maxLength: 200),
                        lat = c.String(nullable: false, maxLength: 50),
                        lng = c.String(nullable: false, maxLength: 50),
                        Descripcion = c.String(nullable: false, maxLength: 500),
                        IdUser = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        CountryId = c.Int(nullable: false),
                        TravelGuideId = c.Long(nullable: false),
                        CityType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TravelGuides", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cities", "Id", "dbo.TravelGuides");
            DropIndex("dbo.Cities", new[] { "Id" });
            DropTable("dbo.Cities");
        }
    }
}

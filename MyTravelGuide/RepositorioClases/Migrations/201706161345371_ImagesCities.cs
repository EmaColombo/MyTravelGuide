namespace RepositorioClases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImagesCities : DbMigration
    {
        public override void Up()
        {

            CreateTable(
                "dbo.ImagesCities",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CityId = c.Long(nullable: false),
                        RutaImagen = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.Id)
                .Index(t => t.Id);
            
          
        }
        
        public override void Down()
        { 
            DropForeignKey("dbo.ImagesCities", "Id", "dbo.Cities");
            DropIndex("dbo.ImagesCities", new[] { "Id" });         
            DropTable("dbo.ImagesCities");
            
        }
    }
}

namespace RepositorioClases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class travelguidestate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TravelGuides", "State", c => c.Int(nullable: false));
            DropColumn("dbo.TravelGuides", "Direccion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TravelGuides", "Direccion", c => c.String(nullable: false, maxLength: 200));
            DropColumn("dbo.TravelGuides", "State");
        }
    }
}

namespace RepositorioClases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CiudadPais : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Ciudad", c => c.String());
            AddColumn("dbo.Events", "Pais", c => c.String());
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Pais");
            DropColumn("dbo.Events", "Ciudad");
        }
    }
}

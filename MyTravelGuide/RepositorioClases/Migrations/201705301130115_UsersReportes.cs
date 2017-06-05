namespace RepositorioClases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersReportes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UsersReportes",
                c => new
                    {
                        ReporteId = c.Int(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                        Observacion = c.String(nullable: false, maxLength: 300),
                        Fecha = c.DateTime(nullable: false),
                        Resuelto = c.Boolean(),
                    })
                .PrimaryKey(t => t.ReporteId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UsersReportes");
        }
    }
}

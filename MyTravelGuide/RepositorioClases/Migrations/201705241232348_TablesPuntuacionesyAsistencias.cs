namespace RepositorioClases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TablesPuntuacionesyAsistencias : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AsistenciasEventos",
                c => new
                    {
                        IdAsistencia = c.Long(nullable: false, identity: true),
                        EventId = c.Long(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                        TipoAsistencia = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdAsistencia);
            
            CreateTable(
                "dbo.PuntuacionesEventos",
                c => new
                    {
                        IdPuntuacion = c.Long(nullable: false, identity: true),
                        EventId = c.Long(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                        Puntuacion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPuntuacion);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PuntuacionesEventos");
            DropTable("dbo.AsistenciasEventos");
        }
    }
}

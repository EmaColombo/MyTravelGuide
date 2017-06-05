namespace RepositorioClases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambiosenEventosMaxi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InteresesEventos",
                c => new
                    {
                        IdInteres = c.Long(nullable: false, identity: true),
                        EventId = c.Long(nullable: false),
                        UserId = c.Int(nullable: false),
                        Tipo = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Anulado = c.Boolean(nullable: false),
                        FechaAnulacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdInteres);
            
            DropTable("dbo.AsistenciasEventos");
        }
        
        public override void Down()
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
            
            DropTable("dbo.InteresesEventos");
        }
    }
}

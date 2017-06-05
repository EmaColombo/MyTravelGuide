namespace RepositorioClases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class actualizandotodo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        iDUsuario = c.Int(nullable: false),
                        EventId = c.Long(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        FechaUltimaActualizacion = c.DateTime(nullable: false),
                        ComentarioPadre = c.Long(),
                        Comentario = c.String(nullable: false, maxLength: 3000),
                        Like = c.Int(nullable: false),
                        UnLike = c.Int(nullable: false),
                        Estado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NombreEvento = c.String(nullable: false, maxLength: 200),
                        lat = c.String(nullable: false, maxLength: 50),
                        lng = c.String(nullable: false, maxLength: 50),
                        Descripcion = c.String(nullable: false, maxLength: 500),
                        FechaInicio = c.DateTime(nullable: false),
                        FechaFin = c.DateTime(nullable: false),
                        IdUser = c.Int(nullable: false),
                        IdCategoria = c.Int(nullable: false),
                        Destacado = c.Boolean(nullable: false),
                        Direccion = c.String(nullable: false, maxLength: 200),
                        RutaImagen = c.String(),
                        Estado = c.Int(nullable: false),
                        HoraInicio = c.Time(precision: 7),
                        HoraFin = c.Time(precision: 7),
                        FechaCreacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EventsReportes",
                c => new
                    {
                        ReporteId = c.Int(nullable: false, identity: true),
                        EventId = c.Long(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                        Observacion = c.String(nullable: false, maxLength: 300),
                        Fecha = c.DateTime(nullable: false),
                        Resuelto = c.Boolean(),
                    })
                .PrimaryKey(t => t.ReporteId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.IdUsuario, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "dbo.CommentsReportes",
                c => new
                    {
                        ReporteId = c.Int(nullable: false, identity: true),
                        CommentId = c.Int(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                        Observacion = c.String(nullable: false, maxLength: 300),
                        Fecha = c.DateTime(nullable: false),
                        Resuelto = c.Boolean(),
                    })
                .PrimaryKey(t => t.ReporteId)
                .ForeignKey("dbo.Comments", t => t.CommentId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.IdUsuario, cascadeDelete: true)
                .Index(t => t.CommentId)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "dbo.VotosUsersEvents",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IdUser = c.Int(nullable: false),
                        IdEvent = c.Long(nullable: false),
                        Voto = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.IdEvent, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdUser)
                .Index(t => t.IdEvent);
            
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
                        FechaAnulacion = c.DateTime(),
                    })
                .PrimaryKey(t => t.IdInteres);
            
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
            
            CreateTable(
                "dbo.UsersReportes",
                c => new
                    {
                        ReporteId = c.Int(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                        Observacion = c.String(nullable: false, maxLength: 300),
                        Fecha = c.DateTime(nullable: false),
                        Resuelto = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ReporteId);
            
            AddColumn("dbo.Users", "UserDestacado", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VotosUsersEvents", "IdUser", "dbo.Users");
            DropForeignKey("dbo.VotosUsersEvents", "IdEvent", "dbo.Events");
            DropForeignKey("dbo.EventsReportes", "IdUsuario", "dbo.Users");
            DropForeignKey("dbo.CommentsReportes", "IdUsuario", "dbo.Users");
            DropForeignKey("dbo.CommentsReportes", "CommentId", "dbo.Comments");
            DropForeignKey("dbo.EventsReportes", "EventId", "dbo.Events");
            DropForeignKey("dbo.Comments", "EventId", "dbo.Events");
            DropIndex("dbo.VotosUsersEvents", new[] { "IdEvent" });
            DropIndex("dbo.VotosUsersEvents", new[] { "IdUser" });
            DropIndex("dbo.CommentsReportes", new[] { "IdUsuario" });
            DropIndex("dbo.CommentsReportes", new[] { "CommentId" });
            DropIndex("dbo.EventsReportes", new[] { "IdUsuario" });
            DropIndex("dbo.EventsReportes", new[] { "EventId" });
            DropIndex("dbo.Comments", new[] { "EventId" });
            DropColumn("dbo.Users", "UserDestacado");
            DropTable("dbo.UsersReportes");
            DropTable("dbo.PuntuacionesEventos");
            DropTable("dbo.InteresesEventos");
            DropTable("dbo.VotosUsersEvents");
            DropTable("dbo.CommentsReportes");
            DropTable("dbo.EventsReportes");
            DropTable("dbo.Events");
            DropTable("dbo.Comments");
        }
    }
}

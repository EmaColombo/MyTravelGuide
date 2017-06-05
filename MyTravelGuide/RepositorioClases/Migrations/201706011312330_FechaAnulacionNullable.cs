namespace RepositorioClases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FechaAnulacionNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InteresesEventos", "FechaAnulacion", c => c.DateTime());
            AlterColumn("dbo.UsersReportes", "Resuelto", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UsersReportes", "Resuelto", c => c.Boolean());
            AlterColumn("dbo.InteresesEventos", "FechaAnulacion", c => c.DateTime(nullable: false));
        }
    }
}

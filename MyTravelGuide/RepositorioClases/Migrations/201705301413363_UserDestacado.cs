namespace RepositorioClases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserDestacado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "UserDestacado", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "UserDestacado");
        }
    }
}

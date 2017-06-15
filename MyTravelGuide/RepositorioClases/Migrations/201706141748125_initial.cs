namespace RepositorioClases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cities", "Id", "dbo.TravelGuides");
            DropColumn("dbo.Cities", "TravelGuideId");
            RenameColumn(table: "dbo.Cities", name: "Id", newName: "TravelGuideId");
            RenameIndex(table: "dbo.Cities", name: "IX_Id", newName: "IX_TravelGuideId");
            DropPrimaryKey("dbo.Cities");
            AlterColumn("dbo.Cities", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Cities", "Id");
            AddForeignKey("dbo.Cities", "TravelGuideId", "dbo.TravelGuides", "TravelGuideId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cities", "TravelGuideId", "dbo.TravelGuides");
            DropPrimaryKey("dbo.Cities");
            AlterColumn("dbo.Cities", "Id", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.Cities", "Id");
            RenameIndex(table: "dbo.Cities", name: "IX_TravelGuideId", newName: "IX_Id");
            RenameColumn(table: "dbo.Cities", name: "TravelGuideId", newName: "Id");
            AddColumn("dbo.Cities", "TravelGuideId", c => c.Long(nullable: false));
            AddForeignKey("dbo.Cities", "Id", "dbo.TravelGuides", "TravelGuideId");
        }
    }
}

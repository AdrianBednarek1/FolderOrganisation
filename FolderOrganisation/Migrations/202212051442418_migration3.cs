namespace FolderOrganisation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ModelViewFolders", "Parent", c => c.Int(nullable: false));
            DropColumn("dbo.ModelViewFolders", "ParentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ModelViewFolders", "ParentId", c => c.Int(nullable: false));
            DropColumn("dbo.ModelViewFolders", "Parent");
        }
    }
}

namespace FolderOrganisation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ModelViewFolders", name: "Parent_Id", newName: "ModelViewFolder_Id");
            RenameIndex(table: "dbo.ModelViewFolders", name: "IX_Parent_Id", newName: "IX_ModelViewFolder_Id");
            AddColumn("dbo.ModelViewFolders", "ParentId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ModelViewFolders", "ParentId");
            RenameIndex(table: "dbo.ModelViewFolders", name: "IX_ModelViewFolder_Id", newName: "IX_Parent_Id");
            RenameColumn(table: "dbo.ModelViewFolders", name: "ModelViewFolder_Id", newName: "Parent_Id");
        }
    }
}

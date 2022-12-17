namespace FolderOrganisation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class parentId : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Folders", name: "ParentFolder_Id", newName: "ParentFolderId");
            RenameIndex(table: "dbo.Folders", name: "IX_ParentFolder_Id", newName: "IX_ParentFolderId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Folders", name: "IX_ParentFolderId", newName: "IX_ParentFolder_Id");
            RenameColumn(table: "dbo.Folders", name: "ParentFolderId", newName: "ParentFolder_Id");
        }
    }
}

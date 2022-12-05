namespace FolderOrganisation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ModelViewFolders", "ModelViewFolder_Id", "dbo.ModelViewFolders");
            DropIndex("dbo.ModelViewFolders", new[] { "ModelViewFolder_Id" });
            DropTable("dbo.ModelViewFolders");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ModelViewFolders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Directory = c.String(),
                        Parent = c.Int(nullable: false),
                        ModelViewFolder_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.ModelViewFolders", "ModelViewFolder_Id");
            AddForeignKey("dbo.ModelViewFolders", "ModelViewFolder_Id", "dbo.ModelViewFolders", "Id");
        }
    }
}

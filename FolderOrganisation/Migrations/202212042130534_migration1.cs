namespace FolderOrganisation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ModelViewFolders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Directory = c.String(),
                        Parent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ModelViewFolders", t => t.Parent_Id)
                .Index(t => t.Parent_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ModelViewFolders", "Parent_Id", "dbo.ModelViewFolders");
            DropIndex("dbo.ModelViewFolders", new[] { "Parent_Id" });
            DropTable("dbo.ModelViewFolders");
        }
    }
}

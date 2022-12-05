namespace FolderOrganisation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Folders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentFolder = c.String(),
                        ParentFolder_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Folders", t => t.ParentFolder_Id)
                .Index(t => t.ParentFolder_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Folders", "ParentFolder_Id", "dbo.Folders");
            DropIndex("dbo.Folders", new[] { "ParentFolder_Id" });
            DropTable("dbo.Folders");
        }
    }
}

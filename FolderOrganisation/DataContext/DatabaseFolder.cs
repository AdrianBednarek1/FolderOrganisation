using System.Data.Entity;

namespace FolderOrganisation.DataContext
{
    public class DatabaseFolder : DbContext
    {
        public DbSet<Folder> DbFolders { get; set; }
    }
}
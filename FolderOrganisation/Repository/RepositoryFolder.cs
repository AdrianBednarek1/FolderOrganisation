using FolderOrganisation.DataContext;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FolderOrganisation.Repository
{
    public class RepositoryFolder
    {
        private DatabaseFolder DbFolder;
        private FolderManagement folderManagement;
        public RepositoryFolder()
        {
            DbFolder = new DatabaseFolder();
            folderManagement = new FolderManagement();

            DbFolder.DbFolders.RemoveRange(DbFolder.DbFolders);
            DbFolder.SaveChanges();
        }
        public async Task<List<Folder>> GetFolders()
        {
            if (DbFolder.DbFolders.Any()) return DbFolder.DbFolders.ToList();

            DbFolder.DbFolders.Add(await folderManagement.GetFolders());
            DbFolder.SaveChanges();

            return DbFolder.DbFolders.ToList();
        }
    }
}
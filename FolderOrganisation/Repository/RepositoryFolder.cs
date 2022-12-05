using FolderOrganisation.DataContext;
using System;
using System.IO;
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
            RemoveCurrentDb();
        }
        private void RemoveCurrentDb()
        {
            DbFolder.DbFolders.RemoveRange(DbFolder.DbFolders);
            DbFolder.SaveChanges();
        }
        public async Task<Folder> GetFolders()
        {
            await RefreshDb();
            return DbFolder.DbFolders.First();
        }

        public async Task Delete(Folder folder)
        {
            bool directoryDeleted = await folderManagement.DeleteDirectory(folder.CurrentFolder);
            if (!directoryDeleted) return;
            DbFolder.DbFolders.Remove(folder);
            await DbFolder.SaveChangesAsync();
        }

        private async Task RefreshDb()
        {
            if (DbFolder.DbFolders.Any()) return;

            DbFolder.DbFolders.Add(await folderManagement.GetFolders());
            DbFolder.SaveChanges();
        }
        public async Task CreateFolder(Folder createFolder)
        {
            string path = createFolder.CurrentFolder;
            if (!folderManagement.CreateDirectory(path)) return;
            await CreateNewFolder(createFolder);
        }

        private async Task CreateNewFolder(Folder createFolder)
        {
            Folder parent = DbFolder.DbFolders.FirstOrDefault(target => target.Id==createFolder.ParentFolder.Id);
            parent.SubFolders.Add(createFolder);
            await DbFolder.SaveChangesAsync();
        }
        public async Task<Folder> Search(int id)
        {
            return await DbFolder.DbFolders.FindAsync(id);
        }
    }
}
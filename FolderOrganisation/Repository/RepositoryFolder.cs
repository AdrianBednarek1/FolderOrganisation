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
            DbFolder.DbFolders.Remove(folder);
            await folderManagement.DeleteDirectory(folder.CurrentFolder);
        }

        private async Task RefreshDb()
        {
            if (DbFolder.DbFolders.Any()) return;

            DbFolder.DbFolders.Add(await folderManagement.GetFolders());
            DbFolder.SaveChanges();
        }
        public async Task CreateFolder(Folder createFolder)
        {
            string pathFOlder= Path.Combine(createFolder.CurrentFolder, "blabal");
            if (!folderManagement.CreateDirectory(pathFOlder)) return;
            await CreateNewFolder(new Folder(pathFOlder,createFolder));
        }

        private async Task CreateNewFolder(Folder createFolder)
        {
            Folder folder = DbFolder.DbFolders.FirstOrDefault(target => target.Id==createFolder.ParentFolder.Id);
            folder.SubFolders.Add(createFolder);
            await DbFolder.SaveChangesAsync();
        }
        public async Task<Folder> Search(int id)
        {
            return await DbFolder.DbFolders.FindAsync(id);
        }
    }
}
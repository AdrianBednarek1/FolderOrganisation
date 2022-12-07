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
        public async Task Delete(Folder folder)
        {
            if (folder==null) return;
            bool folderOnDiscDeleted = await folderManagement.DeleteFolderOnDisc(folder.CurrentFolder);
            if (!folderOnDiscDeleted) return;
            DbFolder.DbFolders.Remove(folder);
            await DbFolder.SaveChangesAsync();
        }
        private async Task RefreshDb()
        {
            if (DbFolder.DbFolders.Any()) return;
            DbFolder.DbFolders.Add(await folderManagement.GetFolders());
            DbFolder.SaveChanges();
        }

        public async Task Edit(int id, string newPath)
        {
            string path = DbFolder.DbFolders.SingleOrDefault(m=>m.Id==id).CurrentFolder;
            if (!folderManagement.EditFolderOnDisc(path,newPath)) return;
            await EditFolderInDb(id, newPath);
        }

        public async Task CreateFolder(Folder createFolder)
        {
            string path = createFolder.CurrentFolder;
            if (!folderManagement.CreateFolderOnDisc(path)) return;
            await CreateFolderInDb(createFolder);
        }
        private async Task CreateFolderInDb(Folder createFolder)
        {
            Folder parent = DbFolder.DbFolders.FirstOrDefault(target => target.Id==createFolder.ParentFolder.Id);
            parent.SubFolders.Add(createFolder);
            await DbFolder.SaveChangesAsync();
        }
        private async Task EditFolderInDb(int id, string newPath)
        {
            Folder folder = await DbFolder.DbFolders.FindAsync(id);
            DbFolder.DbFolders.Attach(folder);
            folder.CurrentFolder = newPath;
            await DbFolder.SaveChangesAsync();
        }
        public async Task<Folder> GetFolders(int? id)
        {
            await RefreshDb();
            return await DbFolder.DbFolders.FindAsync(id) ?? DbFolder.DbFolders.First();
        }
    }
}
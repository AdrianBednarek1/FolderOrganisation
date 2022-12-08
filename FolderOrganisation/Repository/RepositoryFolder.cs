using FolderOrganisation.DataContext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FolderOrganisation.Repository
{
    public class RepositoryFolder
    {
        private string defaultPath = @"C:\FolderOrganisation";
        private DatabaseFolder DbFolder;
        private FolderManagement folderManagement;
        public RepositoryFolder()
        {
            DbFolder = new DatabaseFolder();
            folderManagement = new FolderManagement();
            RestartDb(defaultPath);
        }
        public void RestartDb(string _path)
        {
            DbFolder.DbFolders.RemoveRange(DbFolder.DbFolders);
            DbFolder.DbFolders.Add(new Folder(_path));
            DbFolder.SaveChanges();
        }
        public async Task<List<string>> GetDrivesNames()
        {
            List<string> drives = new List<string>();
            drives.AddRange(await folderManagement.GetDrives());
            drives.Add(defaultPath);
            return drives;
        }
        public async Task Delete(Folder folder)
        {
            if (folder==null) return;
            bool folderOnDiscDeleted = await folderManagement.DeleteFolderOnDisc(folder.CurrentFolder);
            if (!folderOnDiscDeleted) return;
            DbFolder.DbFolders.Remove(folder);
            await DbFolder.SaveChangesAsync();
        }
        private async Task GetFoldersFromDisc(Folder folder)
        {
            if(!folder.SubFolders.Any()) await folderManagement.GetSubFolders(folder, 2);
            foreach (Folder subfolder in folder.SubFolders)
            {
                if (!subfolder.SubFolders.Any()) await folderManagement.GetSubFolders(subfolder,1);
            }
            await DbFolder.SaveChangesAsync();
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
            folder.CurrentFolder = newPath;
            foreach (var item in folder.SubFolders) { item.CurrentFolder = Path.Combine(folder.CurrentFolder,Path.GetFileName(item.CurrentFolder)); }
            await DbFolder.SaveChangesAsync();
        }
        public async Task<Folder> GetFolders(int? id)
        {
            Folder folder = await DbFolder.DbFolders.FindAsync(id) ?? DbFolder.DbFolders.First();
            await GetFoldersFromDisc(folder);
            return folder;
        }
    }
}
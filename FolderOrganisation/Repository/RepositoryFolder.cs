using FolderOrganisation.DataContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FolderOrganisation.Repository
{
    public class RepositoryFolder
    {
        private string defaultPath = @"C:\";
        private DatabaseFolder DbFolder;
        public RepositoryFolder()
        {
            DbFolder = new DatabaseFolder();          
        }
        public async Task RestartDb()
        {
            DbFolder.DbFolders.RemoveRange(DbFolder.DbFolders);
            await DbFolder.SaveChangesAsync(); 
            await CreateStartingDirectory();
        }

        private async Task CreateStartingDirectory()
        {
            Folder parent = new Folder(defaultPath);
            for (int i = 0; i < 6; i++)
            {
                parent.SubFolders.Add(new Folder(Path.Combine(defaultPath,"mapa"+i),parent));
            }
            DbFolder.DbFolders.Add(parent);
            await DbFolder.SaveChangesAsync();
        }
        public async Task Delete(Folder folder)
        {
            if (await DbFolder.DbFolders.FindAsync(folder?.Id)==null) return;
            DbFolder.DbFolders.Remove(folder);
            await DbFolder.SaveChangesAsync();
        }
        public async Task Edit(int id, string newPath)
        {
            string path = DbFolder.DbFolders.SingleOrDefault(m=>m.Id==id).CurrentFolder;
            if (await DbFolder.DbFolders.FirstOrDefaultAsync(d=>d.CurrentFolder.Equals(newPath))!=null) return;
            await EditFolderInDb(id, newPath);
        }
        public async Task CreateFolder(Folder createFolder)
        {
            string path = createFolder.CurrentFolder;
            if (await DbFolder.DbFolders.FirstOrDefaultAsync(d=>d.CurrentFolder.Equals(path))!=null) return;
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
            if(!DbFolder.DbFolders.Any()) await CreateStartingDirectory();
            Folder folder = await DbFolder.DbFolders.FindAsync(id) ?? DbFolder.DbFolders.First();
            return folder;
        }
    }
}
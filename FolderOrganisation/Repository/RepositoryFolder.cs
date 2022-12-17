using FolderOrganisation.DataContext;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FolderOrganisation.Repository
{
    public class RepositoryFolder
    {
        private readonly string defaultPath = @"C:\";
        private DatabaseFolder DbFolder;
        public RepositoryFolder()
        {
            DbFolder = new DatabaseFolder();
        }
        public async Task RestartDb()
        {
            DbFolder.DbFolders.RemoveRange(DbFolder.DbFolders);
            await DbFolder.SaveChangesAsync();
            Folder parent = CreateStartingDirectory(new Folder(defaultPath),2);
            DbFolder.DbFolders.Add(parent);
            await DbFolder.SaveChangesAsync();
        }
        private Folder CreateStartingDirectory(Folder parent,int subFolderLevel)
        {
            if (subFolderLevel < 0) return null;
            subFolderLevel--;
            for (int i = 0; i < 4; i++)
            {
                Folder newFolder = new Folder(Path.Combine(defaultPath, "mapa" + i), parent);
                CreateStartingDirectory(newFolder, subFolderLevel);
                parent.SubFolders.Add(newFolder);
            }
            return parent;
        }
        public async Task Delete(Folder folder)
        {
            if (await DbFolder.DbFolders.FindAsync(folder?.Id)==null) return;
            await DeleteSubFolders(folder.SubFolders);
            DbFolder.DbFolders.Remove(folder);  
            await DbFolder.SaveChangesAsync();
        }
        public async Task DeleteSubFolders(List<Folder> subFolders)
        {
            foreach (var item in subFolders.ToList())
            {
                if (item.SubFolders.Any()) await DeleteSubFolders(item.SubFolders);             
                DbFolder.DbFolders.Remove(await GetFolders(item.Id));
            }
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
            if(!DbFolder.DbFolders.Any()) await RestartDb();
            Folder FolderFromDb = 
                await DbFolder.DbFolders.Include(m=>m.SubFolders).SingleOrDefaultAsync(m=>m.Id==id) 
                ?? DbFolder.DbFolders.Include(m=>m.SubFolders).FirstOrDefault();
            return FolderFromDb;
        }
    }
}
﻿using FolderOrganisation.DataContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

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
            for (int i = 0; i < 3; i++)
            {
                Folder newFolder = new Folder(Path.Combine(parent.CurrentFolder, "mapa" + i), parent);
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
            bool newPathExists = await DbFolder.DbFolders.FirstOrDefaultAsync(d => d.CurrentFolder.Equals(newPath)) != null;
            if (newPathExists) return;
            Folder folder = await DbFolder.DbFolders.FindAsync(id);
            folder.CurrentFolder = newPath;
            UpdateSubFolders(folder);
            await DbFolder.SaveChangesAsync();
        }
        private void UpdateSubFolders(Folder folder)
        {
            foreach (var item in folder.SubFolders)
            {
                string updatedPath = Path.Combine(folder.CurrentFolder, Path.GetFileName(item.CurrentFolder));
                item.CurrentFolder = updatedPath;
                if (item.SubFolders.Any()) UpdateSubFolders(item);
            }
        }
        public async Task CreateFolder(Folder createFolder)
        {
            string path = createFolder.CurrentFolder;
            bool pathAlreadyExists = await DbFolder.DbFolders.FirstOrDefaultAsync(d => d.CurrentFolder.Equals(path)) != null;
            if (pathAlreadyExists) return;
            await CreateFolderInDb(createFolder);
        }
        private async Task CreateFolderInDb(Folder createFolder)
        {
            Folder parent = DbFolder.DbFolders.FirstOrDefault(target => target.Id==createFolder.ParentFolder.Id);
            parent.SubFolders.Add(createFolder);
            await DbFolder.SaveChangesAsync();
        }
        public async Task<Folder> GetFolders(int? id)
        {   
            if(!DbFolder.DbFolders.Any()) await RestartDb();
            Folder searchOrRoot = 
                await DbFolder.DbFolders.Include(m=>m.SubFolders.Select(y=>y.SubFolders)).SingleOrDefaultAsync(m=>m.Id==id) 
                ?? DbFolder.DbFolders.Include(m=>m.SubFolders.Select(y=>y.SubFolders)).FirstOrDefault();
            return searchOrRoot;
        }
        public async Task DeleteEverything()
        {
            DbFolder.DbFolders.RemoveRange(DbFolder.DbFolders);
            await DbFolder.SaveChangesAsync();
            DbFolder.DbFolders.Add(new Folder(defaultPath));
            await DbFolder.SaveChangesAsync();
        }
    }
}
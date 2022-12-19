using FolderOrganisation.DataContext;
using FolderOrganisation.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FolderOrganisation.Repository
{
    public static class ServiceFolder
    {
        private static RepositoryFolder repositoryFolder = new RepositoryFolder();
        public static async Task<List<ModelViewStructure>> GetStructure()
        {
            Folder folder = await repositoryFolder.GetFolders(null);
            List<ModelViewStructure> structure = new List<ModelViewStructure>();
            await GetStructureFolders(structure, folder,-1);
            return structure;
        }
        private static async Task GetStructureFolders(List<ModelViewStructure> structure, Folder folder, int subFolderLevel)
        {
            subFolderLevel++;
            foreach (var item in folder.SubFolders.OrderBy(d=>d.CurrentFolder).ToList())
            {
                Folder IncludedSubFolders = await repositoryFolder.GetFolders(item.Id);
                string folderName = Path.GetFileName(IncludedSubFolders.CurrentFolder);
                ModelViewStructure newModelView = new ModelViewStructure(folderName, subFolderLevel);
                structure.Add(newModelView);
                await GetStructureFolders(structure,item,subFolderLevel);
            }
        }
        public static async Task<ModelViewFolder> GetFolders(int? id)
        {
            Folder folder = await repositoryFolder.GetFolders(id);
            ModelViewFolder model = new ModelViewFolder(folder, folder.ParentFolder?.Id);
            return model;
        }
        public static async Task Delete(int id)
        {
            Folder folder = await repositoryFolder.GetFolders(id);
            bool folderIsEmpty = folder==null;
            if (folderIsEmpty) return;
            await repositoryFolder.Delete(folder);
        }
        public static async Task Create(ModelViewFolder model)
        {
            Folder parent = await repositoryFolder.GetFolders(model.Parent);
            Folder folder = new Folder(model.FullDirectory, parent);
            bool nameExists = parent.SubFolders.SingleOrDefault(d=>d.CurrentFolder.Equals(folder.CurrentFolder))!=null;
            if (nameExists) return;
            await repositoryFolder.CreateFolder(folder);
        }

        public static async Task Edit(ModelViewFolder model)
        {
            Folder parent = await repositoryFolder.GetFolders(model.Parent);
            bool nameExists = parent.SubFolders.SingleOrDefault(d=>d.CurrentFolder.Equals(model.FullDirectory))!=null;
            if (nameExists) return;
            await repositoryFolder.Edit(model.Id, model.FullDirectory);
        }
        public static async Task RestartDb()
        {
            await repositoryFolder.RestartDb();
        }
        public static async Task DeleteEverything()
        {
            await repositoryFolder.DeleteEverything();
        }
    }
}
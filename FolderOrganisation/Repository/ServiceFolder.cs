using FolderOrganisation.DataContext;
using FolderOrganisation.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FolderOrganisation.Repository
{
    public static class ServiceFolder
    {
        private static RepositoryFolder repositoryFolder = new RepositoryFolder();
        private static CMDfolderStructure cmdFolderStructure = new CMDfolderStructure();
        public static async Task<ModelViewFolder> GetFolders(int? id)
        {
            Folder folder = await repositoryFolder.GetFolders(id);
            ModelViewFolder model = new ModelViewFolder(folder, folder.ParentFolder?.Id);
            return model;
        }
        public static async Task Delete(int id)
        {
            Folder folder = await repositoryFolder.GetFolders(id);
            await repositoryFolder.Delete(folder);
        }
        public static async Task Create(ModelViewFolder model)
        {
            Folder parent = await repositoryFolder.GetFolders(model.Parent);
            Folder folder = new Folder(model.FullDirectory, parent);
            await repositoryFolder.CreateFolder(folder);
        }

        public static async Task Edit(ModelViewFolder model)
        {
            await repositoryFolder.Edit(model.Id, model.FullDirectory);
        }
        public static async Task RestartDb()
        {
            await repositoryFolder.RestartDb();
        }
    }
}
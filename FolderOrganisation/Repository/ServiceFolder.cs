using FolderOrganisation.DataContext;
using FolderOrganisation.ViewModels;
using System.Threading.Tasks;

namespace FolderOrganisation.Repository
{
    public static class ServiceFolder
    {
        private static RepositoryFolder repositoryFolder = new RepositoryFolder();
        public static async Task<ModelViewFolder> GetFolders(int? id)
        {
            Folder folder = await repositoryFolder.GetFolders(id);
            ModelViewFolder model = new ModelViewFolder(folder, folder.ParentFolder?.Id ?? 0);
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
    }
}
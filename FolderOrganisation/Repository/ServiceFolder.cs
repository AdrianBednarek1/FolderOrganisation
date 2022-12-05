using FolderOrganisation.DataContext;
using FolderOrganisation.ViewModels;
using System.Threading.Tasks;

namespace FolderOrganisation.Repository
{
    public static class ServiceFolder
    {
        private static RepositoryFolder repositoryFolder = new RepositoryFolder();
        public static async Task<ModelViewFolder> GetFolders()
        {
            return new ModelViewFolder(await repositoryFolder.GetFolders());
        }
        public static async Task Delete(int id)
        {
            Folder folder = await repositoryFolder.Search(id);
            await repositoryFolder.Delete(folder);
        }
        public static async Task Create(ModelViewFolder model)
        {
            Folder parent = await repositoryFolder.Search(model.Parent);
            Folder folder = new Folder(model.FullDirectory, parent);
            await repositoryFolder.CreateFolder(folder);
        }
    }
}
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
        public static async Task Create(int id)
        {
            Folder folder = await repositoryFolder.Search(id);
            await repositoryFolder.CreateFolder(folder);
        }
    }
}
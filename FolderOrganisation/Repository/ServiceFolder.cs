using FolderOrganisation.DataContext;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FolderOrganisation.Repository
{
    public static class ServiceFolder
    {
        private static RepositoryFolder repositoryFolder = new RepositoryFolder();
        public static async Task<List<Folder>> GetFolders()
        {
            return await repositoryFolder.GetFolders();
        }
    }
}
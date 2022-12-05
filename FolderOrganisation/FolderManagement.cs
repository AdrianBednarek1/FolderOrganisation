using FolderOrganisation.DataContext;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FolderOrganisation
{
    public class FolderManagement
    {
        private string path = @"C:\FolderOrganisation";
        public FolderManagement()
        {
            CreateStartingDirectory();
        }
        private void CreateStartingDirectory()
        {
            if(!CreateDirectory(path)) return;
            for (int i = 1; i < 4; i++)
            {
                string map = Path.Combine(path, "Mapa" + i);
                CreateDirectory(map);
            }
        }
        public bool CreateDirectory(string _path)
        {
            if (Directory.Exists(_path)) return false;
            Directory.CreateDirectory(_path);
            return true;
        }
        public async Task DeleteDirectory(string _path)
        {
            if (!Directory.Exists(_path)) return;
            Directory.Delete(_path);
        }
        public async Task<Folder> GetFolders()
        {
            DirectoryInfo mainDirectory = new DirectoryInfo(path);
            DirectoryInfo[] allDirectories = mainDirectory.GetDirectories();

            Folder folder = new Folder(path);
            folder = await GetAllFolders(folder, allDirectories);

            return folder;
        }
        private async Task<Folder> GetAllFolders(Folder folder, DirectoryInfo[] directories)
        {
            foreach (var item in directories)
            {
                try
                {
                    folder.SubFolders.Add(new Folder(item.FullName, folder));
                    await GetAllFolders(folder.SubFolders.Last(), item.GetDirectories());
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return folder;
        }
    }
}
using FolderOrganisation.DataContext;
using System;
using System.Collections.Generic;
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
            CheckDirectory();
        }

        private void CheckDirectory()
        {
            if (Directory.Exists(path)) return;
            CreateNewDirectory();
        }

        private void CreateNewDirectory()
        {
            Directory.CreateDirectory(path);
            for (int i = 1; i < 4; i++)
            {
                string map = Path.Combine(path, "Mapa" + i);
                Directory.CreateDirectory(map);
            }
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
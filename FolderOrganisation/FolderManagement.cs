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
            CreateStartingDirectory();
        }
        private void CreateStartingDirectory()
        {
            if(!CreateFolderOnDisc(path)) return;
            for (int i = 1; i < 4; i++)
            {
                string map = Path.Combine(path, "Mapa" + i);
                CreateFolderOnDisc(map);
            }
        }
        public bool CreateFolderOnDisc(string _path)
        {
            if (Directory.Exists(_path)) return false;
            Directory.CreateDirectory(_path);
            return true;
        }
        public bool EditFolderOnDisc(string _path, string newPath)
        {
            if (Directory.Exists(newPath)) return false;
            Directory.Move(_path,newPath);
            return true;
        }
        public async Task<bool> DeleteFolderOnDisc(string path)
        {
            if (!Directory.Exists(path)) return false;
            List<string> paths = new List<string>();
            paths.AddRange(Directory.GetDirectories(path).ToList());
            foreach (var item in paths){await DeleteFolderOnDisc(item);}
            Directory.Delete(path);
            return true;
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
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
            if (!CreateFolderOnDisc(path)) return;
            for (int i = 1; i < 4; i++)
            {
                string folder = Path.Combine(path, "Mapa" + i);
                CreateFolderOnDisc(folder);
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
        public async Task<List<string>> GetDrives()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            List<string> drivesNames = new List<string>();
            foreach (var item in drives)
            {
                drivesNames.Add(item.Name);
            }
            return drivesNames;
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
        public async Task<List<Folder>> GetSubFolders(Folder folder,int? level)
        {
            DirectoryInfo mainDirectory = new DirectoryInfo(folder.CurrentFolder);
            DirectoryInfo[] allDirectories = mainDirectory.GetDirectories().Where(f => (f.Attributes & FileAttributes.Hidden) == 0).ToArray();

            await GetSubFoldersPerLevel(folder, allDirectories, level);

            return folder.SubFolders;
        }
        public async Task<List<Folder>> GetSubFoldersPerLevel(Folder folder, DirectoryInfo[] directories, int? subFolderLevel)
        {
            if (subFolderLevel == 0) return null;
            foreach (var item in directories)
            {
                try
                {
                    if (item.FullName.Contains("System Volume Information")) continue;
                    folder.SubFolders.Add(new Folder(item.FullName, folder));
                    await GetSubFoldersPerLevel(folder.SubFolders.Last(), item.GetDirectories(),subFolderLevel--);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return folder.SubFolders;
        }
    }
}
using System.Collections.Generic;

namespace FolderOrganisation.DataContext
{
    public class Folder
    {
        public int Id { get; set; }
        public string CurrentFolder { get; set; }
        public List<Folder> SubFolders { get; set; }
        public Folder ParentFolder { get; set; }
        public Folder()
        {
            SubFolders = new List<Folder>();
            ParentFolder = null;
            CurrentFolder = null;
        }
        public Folder(string currentFolder)
        {
            SubFolders = new List<Folder>();
            ParentFolder = null;
            CurrentFolder = currentFolder;
        }
        public Folder(string currentFolder, Folder parent)
        {
            SubFolders = new List<Folder>();
            ParentFolder = parent;
            CurrentFolder = currentFolder;
        }
    }
}
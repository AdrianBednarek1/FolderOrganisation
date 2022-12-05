using FolderOrganisation.DataContext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FolderOrganisation.ViewModels
{
    public class ModelViewFolder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Directory { get; set; }
        public string FullDirectory { get { return Path.Combine(Directory, Name); } }
        public ModelViewFolder Parent { get; set; }
        public List<ModelViewFolder> SubFolders { get; set; }
        public ModelViewFolder()
        {
            Name = null;
            Directory = null;
            Parent = null;
            SubFolders = new List<ModelViewFolder>();
        }
        public ModelViewFolder(Folder model)
        {
            Id = model.Id;
            Name = Path.GetFileName(model.CurrentFolder);
            Directory = Path.GetDirectoryName(model.CurrentFolder);
            Parent = null;
            SubFolders = new List<ModelViewFolder>();
            FillSubFolders(model.SubFolders);
        }
        public ModelViewFolder(Folder model, ModelViewFolder parentModel)
        {
            Id = model.Id;
            Name = Path.GetFileName(model.CurrentFolder);
            Directory = Path.GetDirectoryName(model.CurrentFolder);
            Parent = parentModel;
            SubFolders = new List<ModelViewFolder>();
            FillSubFolders(model.SubFolders);
        }
        private void FillSubFolders(List<Folder> subFolders)
        {
            if (!subFolders.Any()) return;
            foreach (Folder folder in subFolders)
            {
                SubFolders.Add(new ModelViewFolder(folder,this));
            }
            SubFolders.OrderBy(x => x.Name).ToList();
        }
    }
}
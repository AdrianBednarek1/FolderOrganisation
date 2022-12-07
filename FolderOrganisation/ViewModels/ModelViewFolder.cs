using FolderOrganisation.DataContext;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace FolderOrganisation.ViewModels
{
    public class ModelViewFolder
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Directory { get; set; }
        public string FullDirectory { get { return Path.Combine(Directory, Name); } }
        public int? Parent { get; set; }
        public int SubFoldersCount { get { return SubFolders.Count(); } }
        public List<ModelViewFolder> SubFolders { get; set; }
        public IQueryable<ModelViewFolder> sortedSubFolders {
            get
            {
                return SubFolders.AsQueryable().OrderBy(m=>m.Name) ?? null;
            }}
        public ModelViewFolder()
        {
            Name = null;
            Directory = null;
            SubFolders = new List<ModelViewFolder>();
        }
        public ModelViewFolder(Folder model)
        {
            Id = model.Id;
            Name = Path.GetFileName(model.CurrentFolder);
            Directory = Path.GetDirectoryName(model.CurrentFolder);
            SubFolders = new List<ModelViewFolder>();
            FillSubFolders(model.SubFolders);
        }
        public ModelViewFolder(Folder model, int? parentId)
        {
            Id = model.Id;
            Name = Path.GetFileName(model.CurrentFolder);
            Directory = Path.GetDirectoryName(model.CurrentFolder);
            Parent = parentId;
            SubFolders = new List<ModelViewFolder>();
            FillSubFolders(model.SubFolders);
        }
        private void FillSubFolders(List<Folder> subFolders)
        {
            if (!subFolders.Any()) return;
            foreach (Folder folder in subFolders)
            {
                SubFolders.Add(new ModelViewFolder(folder,Id));
            }
            SubFolders.OrderBy(x => x.Name).ToList();
        }
    }
}
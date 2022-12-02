using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FolderOrganisation.DataContext
{
    public class DatabaseFolder : DbContext
    {
        public DbSet<Folder> DbFolders { get; set; }
    }
}
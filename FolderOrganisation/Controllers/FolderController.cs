using FolderOrganisation.DataContext;
using FolderOrganisation.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FolderOrganisation.Controllers
{
    public class FolderController : Controller
    {   
        public async Task<ActionResult> FolderList()
        {            
            return View(await ServiceFolder.GetFolders());
        }
    }
}
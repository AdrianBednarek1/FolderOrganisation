using FolderOrganisation.DataContext;
using FolderOrganisation.Repository;
using FolderOrganisation.ViewModels;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FolderOrganisation.Controllers
{
    public class FolderController : Controller
    {   
        public async Task<ActionResult> FolderList(ModelViewFolder folder)
        {
            ModelViewFolder modelFolder;
            modelFolder = String.IsNullOrEmpty(folder.Name) ? await ServiceFolder.GetFolders() : folder;
            return View(modelFolder);
        }
        public async Task<ActionResult> Create(int id)
        {       
            await ServiceFolder.Create(id);
            return RedirectToAction("FolderList");
        }
        public async Task<ActionResult> Delete(int id)
        {
            await ServiceFolder.Delete(id);
            return RedirectToAction("FolderList");
        }
    }
}
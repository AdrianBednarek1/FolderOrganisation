using FolderOrganisation.Repository;
using FolderOrganisation.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FolderOrganisation.Controllers
{
    public class FolderController : Controller
    {   
        public async Task<ActionResult> FolderList(int? currentFolderId)
        {
            ModelViewFolder modelFolder = await ServiceFolder.GetFolders(currentFolderId);
            return View(modelFolder);
        }
        public async Task<ActionResult> Create(ModelViewFolder model, int? currentFolderId)
        {   
            await ServiceFolder.Create(model);
            return RedirectToAction("FolderList", "Folder", new { currentFolderId = currentFolderId });
        }
        public async Task<ActionResult> Delete(int deleteFolderId, int? currentFolderId)
        {
            await ServiceFolder.Delete(deleteFolderId);
            return RedirectToAction("FolderList", "Folder", new { currentFolderId = currentFolderId });
        }
        public async Task<ActionResult> Edit(ModelViewFolder model, int? currentFolderId)
        {
            await ServiceFolder.Edit(model);
            return RedirectToAction("FolderList", "Folder", new { currentFolderId = currentFolderId });
        }
        public async Task<ActionResult> Cmd(int? currentFolderId)
        {
            await ServiceFolder.CMDtreeFolder();
            return RedirectToAction("FolderList", "Folder", new { currentFolderId = currentFolderId });
        }
        public async Task<ActionResult> ChangeRootDirectory()
        {
            ViewBag.dropDsownList = await ServiceFolder.GetDrives();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ChangeRootDirectory(string selectedRootDirectory)
        {
            ServiceFolder.ChangeRootDirectory(selectedRootDirectory);
            return RedirectToAction("FolderList", "Folder");
        }
    }
}
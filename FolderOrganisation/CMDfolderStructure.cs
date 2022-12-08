using System.Diagnostics;
using System.Threading.Tasks;

namespace FolderOrganisation
{
    public class CMDfolderStructure
    {
        private ProcessStartInfo proc1;
        public string anyCommand { get; set; }
        public CMDfolderStructure()
        {
            proc1 = new ProcessStartInfo();       
        }
        public async Task RunCMDtreeCommand()
        {
            anyCommand = "tree";
            proc1.UseShellExecute = true;
            proc1.WorkingDirectory = @"C:\Windows\System32";
            proc1.FileName = @"C:\Windows\System32\cmd.exe";
            proc1.Verb = "runas";
            proc1.Arguments = "/K " + anyCommand;
            proc1.WindowStyle = ProcessWindowStyle.Normal;
            Process.Start(proc1);
        }
    }
}
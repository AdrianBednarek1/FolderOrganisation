using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace FolderOrganisation.ViewModels
{
    public class ModelViewStructure
    {
        public string Name { get; set; }
        public int BranchLevel;


        public ModelViewStructure(string name, int branchLevel)
        {
            Name = "";
            for (int i = 0; i < branchLevel; i++){Name += "|-------";}
            Name += name;
        }
        public ModelViewStructure()
        {
            BranchLevel = 0;
            Name = null;
        }
    }
}
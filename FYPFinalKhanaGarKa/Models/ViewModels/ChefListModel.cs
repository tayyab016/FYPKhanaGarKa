using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPFinalKhanaGarKa.Models.ViewModels
{
    public class ChefListViewModel
    {
        public IList<ChefViewModel> Chefs { get; set; }
        public bool Find { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPFinalKhanaGarKa.Models.ViewModels
{
    public class ChefListViewModel
    {
        public IList<ChefViewModel> SearchedChefs { get; set; }
        public IList<ChefViewModel> OtherChefs { get; set; }

    }
}

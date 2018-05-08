using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPFinalKhanaGarKa.Models
{
    public class MenuOfferViewModel
    {
        public IEnumerable<Menu> Menus { get; set; }
        public IEnumerable<Offer> Offers { get; set; }
    }
}

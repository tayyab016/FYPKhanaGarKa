using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPFinalKhanaGarKa.Models.ViewModels
{
    public class ChefAccountViewModel
    {
        public int ChefId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Orders { get; set; }
        public int Rating { get; set; }
        public IEnumerable<Menu> Menu { get; set; }
        public IEnumerable<Offer> Offer { get; set; }
    }
}

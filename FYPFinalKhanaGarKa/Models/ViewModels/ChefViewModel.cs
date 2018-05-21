using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPFinalKhanaGarKa.Models.ViewModels
{
    public class ChefViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImgUrl { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Street { get; set; }
        public int Category { get; set; }
        public string About { get; set; }
        public int Rating { get; set; }
        public int Orders { get; set; }
        public int ChefId { get; set; }
    }
}

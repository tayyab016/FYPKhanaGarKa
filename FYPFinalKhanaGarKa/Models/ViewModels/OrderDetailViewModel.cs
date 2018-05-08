using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPFinalKhanaGarKa.Models.ViewModels
{
    public class OrderDetailViewModel
    {
        public List<OrderLine> Dishis { get; set; }
        public Chef Chef { get; set; }
        public Orders Order { get; set; }
        public string Role { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPFinalKhanaGarKa.Models.ViewModels
{
    public class OrderDetailViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime OrderDate { get; set; }
        public bool OrderType { get; set; }
        public int OrderId { get; set; }
        public bool OrderStatus { get; set; }
        public bool Confirmed { get; set; }
        public bool Received { get; set; }
        public IEnumerable<OrderLine> Orderline { get; set; }
        public string Role { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPFinalKhanaGarKa.Models.ViewModels
{
    public class OrderHistoryViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int Total { get; set; }
        public bool OrderStatus { get; set; }
        public bool Received { get; set; }
        public bool Confirmed { get; set; }
        public bool Canceled { get; set; }
        public string Role { get; set; }
        public int DeliveryDay { get; set; }

        public string DeliveryTime { get; set; }

    }
}

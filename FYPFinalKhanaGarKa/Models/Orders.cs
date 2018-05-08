using System;
using System.Collections.Generic;

namespace FYPFinalKhanaGarKa.Models
{
    public partial class Orders
    {
        public Orders()
        {
            OrderLine = new HashSet<OrderLine>();
        }

        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool OrderStatus { get; set; }
        public string Feedback { get; set; }
        public int ChefId { get; set; }
        public int? DeliveryBoyId { get; set; }
        public int CustomerId { get; set; }
        public string OrderType { get; set; }
        public string SpReq { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Street { get; set; }
        public int? DeliveryCharges { get; set; }

        public Chef Chef { get; set; }
        public Customer Customer { get; set; }
        public DeliveryBoy DeliveryBoy { get; set; }
        public ICollection<OrderLine> OrderLine { get; set; }
    }
}

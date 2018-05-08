using System;
using System.Collections.Generic;

namespace FYPFinalKhanaGarKa.Models
{
    public partial class OrderLine
    {
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Id { get; set; }

        public Orders Order { get; set; }
    }
}

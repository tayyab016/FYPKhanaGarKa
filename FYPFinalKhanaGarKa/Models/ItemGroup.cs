﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPFinalKhanaGarKa.Models
{
    public class ItemGroup
    {
        public int Total { get; set; }

        public bool OrderType { get; set; }

        public string Area { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string SpReq { get; set; }
        
        public int DeliveryDay { get; set; }

        public string DeliveryTime { get; set; }

        public int Cid { get; set; }

        public ICollection<CartItems> Items { get; set; }
    }
}

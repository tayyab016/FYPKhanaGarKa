﻿using System;
using System.Collections.Generic;

namespace FYPFinalKhanaGarKa.Models
{
    public partial class Menu
    {
        public int MenuId { get; set; }
        public string DishName { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string ImgUrl { get; set; }
        public int ChefId { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? DishLike { get; set; }
        public int? DishDislike { get; set; }
        public int? Serving { get; set; }

        public Chef Chef { get; set; }
    }
}

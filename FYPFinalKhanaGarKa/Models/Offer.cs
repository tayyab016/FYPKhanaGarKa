using System;
using System.Collections.Generic;

namespace FYPFinalKhanaGarKa.Models
{
    public partial class Offer
    {
        public int OfferId { get; set; }
        public int? Percentage { get; set; }
        public int Price { get; set; }
        public string OfferName { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public bool Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int ChefId { get; set; }

        public Chef Chef { get; set; }
    }
}

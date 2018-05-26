using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYPFinalKhanaGarKa.Models.ViewModels
{
    public class OfferViewModel
    {
        public int OfferId { get; set; }

        [Range(0, 100, ErrorMessage = "Please Enter discount from 0 to 100")]
        public int? Percentage { get; set; }

        [Required(ErrorMessage = "Price is Required")]
        [RegularExpression("[0-9]+", ErrorMessage = "Price can only contain numaric value")]
        public int Price { get; set; }

        [Required(ErrorMessage = "OfferName is Required")]
        [MaxLength(50, ErrorMessage = "Length should be not more than 50 charaters")]
        public string OfferName { get; set; }

        [MaxLength(200, ErrorMessage = "Length should be not more than 200 charaters")]
        public string Description { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime EndDate { get; set; }
        public string ImgUrl { get; set; }

        public IFormFile Image { get; set; }
    }
}

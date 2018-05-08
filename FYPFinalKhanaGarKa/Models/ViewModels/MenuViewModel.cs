using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYPFinalKhanaGarKa.Models.ViewModels
{
    public class MenuViewModel
    {
        public int MenuId { get; set; }

        [Required(ErrorMessage = "DishName is Required")]
        [MaxLength(50, ErrorMessage = "Length should be not more than 50 charaters")]
        [RegularExpression("[a-zA-Z0-9]+", ErrorMessage = "DishName can only contain alphanumaric value")]
        public string DishName { get; set; }

        [MaxLength(200, ErrorMessage = "Length should be not more than 200 charaters")]
        [RegularExpression("[a-zA-Z0-9]+", ErrorMessage = "Description can only contain alphanumaric value")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is Required")]
        [RegularExpression("[0-9]+", ErrorMessage = "Price can only contain numaric value")]
        public int Price { get; set; }
        public string ImgUrl { get; set; }
        public bool Status { get; set; }
        public IFormFile Image { get; set; }
    }
}

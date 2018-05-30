using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYPFinalKhanaGarKa.Models.ViewModels
{
    public class ModifyDetailsViewModel
    {
        [Required(ErrorMessage = "Your FirstName is Required")]
        [MaxLength(20, ErrorMessage = "Maximum length of your FirstName should not be more than 20 charaters")]
        [RegularExpression("[a-zA-z]+", ErrorMessage = "Your FirstName can only contain letters without space")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Your LastName is Required")]
        [MaxLength(20, ErrorMessage = "Maximum length of your LastName should not be more than 20 charaters")]
        [RegularExpression("[a-zA-z]+", ErrorMessage = "Your LastName can only contain letters without space.")]
        public string LastName { get; set; }

        public bool Gender { get; set; }

        [Required(ErrorMessage = "Your Date of Birth is Required")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        public string Cnic { get; set; }

        [Required(ErrorMessage = "Your Phone No. is Required")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Your Email is Required")]
        [EmailAddress]
        public string Email { get; set; }

        public string ImgUrl { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        [MaxLength(300, ErrorMessage = "Only 300 charcters are allowd.")]
        public string About { get; set; }
        public string Street { get; set; }
        public string Role { get; set; }
        public bool Status { get; set; }
        public IFormFile Image { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYPFinalKhanaGarKa.Models.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Your name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Your email is required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Your phone number is required.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNo { get; set; }
        [Required(ErrorMessage ="Your message is required.")]
        public string Msg { get; set; }
    }
}

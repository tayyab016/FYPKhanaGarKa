using System;
using System.Collections.Generic;

namespace FYPFinalKhanaGarKa.Models
{
    public partial class Chef
    {
        public Chef()
        {
            Menu = new HashSet<Menu>();
            Offer = new HashSet<Offer>();
            Orders = new HashSet<Orders>();
        }

        public int ChefId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public DateTime Dob { get; set; }
        public string Cnic { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Street { get; set; }
        public int Category { get; set; }
        public string Role { get; set; }
        public int? Rating { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ImgUrl { get; set; }
        public string About { get; set; }
        public bool Approved { get; set; }

        public ICollection<Menu> Menu { get; set; }
        public ICollection<Offer> Offer { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}

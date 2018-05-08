using System;
using System.Collections.Generic;

namespace FYPFinalKhanaGarKa.Models_force
{
    public partial class Admin
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhonNo { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Role { get; set; }
        public int AdminId { get; set; }
        public string Cnic { get; set; }
    }
}

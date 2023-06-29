using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OTPPROJECT.Models
{
    public class OTPModel
    {
        [Required]
        public string PhoneNumberOrEmail { get; set; }

        public string OTP { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
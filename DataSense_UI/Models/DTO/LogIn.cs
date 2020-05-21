using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class LogIn
    {
        public string emailAddress { get; set;  }

        public string passWord { get; set;  }
    }

    public class ForgotPassword
    {
        public string emailAddress { get; set; }
    }

    public class ResetPassword
    {
        public string resetCode { get; set; }
        public string emailAddress { get; set; }

        [Required]
        public string newPassWord { get; set; }

        [Required]
        [Compare("newPassWord", ErrorMessage = "New Password and Confirm New Password do not match.")]
        public string confirmNewPassWord { get; set; }
    }
    public class ChangePassword
    {
        [Required]
        public string currentPassword { get; set; }
        [Required]
        public string newPassWord { get; set; }

        [Required]
        [Compare("newPassWord", ErrorMessage = "New Password and Confirm New Password do not match.")]
        public string confirmNewPassWord { get; set; }
    }
    public class ChangePasswordPatch
    {
        public string passWord { get; set; }   
    }

    public class UpdatePasswordPost
    {
        public string emailAddress { get; set; }

        public string updatedPassword { get; set; }
    }

    public class ResetPasswordValidatePost
    {
        public string emailAddress { get; set; }
        public string resetCode { get; set; }
    }
}
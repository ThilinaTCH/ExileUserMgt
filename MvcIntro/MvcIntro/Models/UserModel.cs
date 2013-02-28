using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcIntro.Models
{
    public class UserModel
    {

    }

    public class LogOnModel
    {
        public virtual int UId { get; set; }

        [Required]
        [Display(Name = "User name")]
        public virtual string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public virtual string Password { get; set; }

        [Display(Name = "Remember me?")]
        public virtual bool RememberMe { get; set; }
    }

    public class User
    {
        public User()
        {
            ContactsList= new List<Contact>();
        }

        public virtual int UId { get; set; }

        [Required]
        [Display(Name = "User name")]
        public virtual string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public virtual string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public virtual string ConfirmPassword { get; set; }

        public virtual IList<Contact> ContactsList { get;  set; }
    }
}

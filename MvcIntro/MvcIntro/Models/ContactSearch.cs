using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcIntro.Models
{
    public class ContactSearch
    {        
        [Required]
        [Display(Name = "Enter word")]
        public string SearchQuery { get; set; }
        public List<Contact> UserList = new List<Contact>();
    }
}

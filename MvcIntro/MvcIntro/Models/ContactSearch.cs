using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcIntro.Models
{
    public class ContactSearch
    {        
        [Required]
        public string SearchQuery { get; set; }
        public List<Contact> UserList = new List<Contact>();
    }
}

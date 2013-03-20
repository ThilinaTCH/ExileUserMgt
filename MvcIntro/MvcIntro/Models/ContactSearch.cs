using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcIntro.Models
{
    public class ContactSearch
    {        
        [Required]
        [DisplayName("Search text")]
        public string SearchQuery { get; set; }
        public List<Contact> UserList = new List<Contact>();
    }
}

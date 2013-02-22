using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcIntro.Models
{
    public class UserSearch
    {        
        [Required]
        public string SearchQuery { get; set; }
        public List<User> UserList = new List<User>();
    }
}

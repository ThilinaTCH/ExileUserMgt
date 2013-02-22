using System.ComponentModel.DataAnnotations;

namespace MvcIntro.Models
{
    public class User
    {
        public User()
        {
            Id = 0;
            Name = "";
            Address = "";
        }
        public User(string name, string address)
        {
            Name = name;
            Address = address;
        }
        public virtual int Id { get; set; }
        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual string Address { get; set; }
        }
    }

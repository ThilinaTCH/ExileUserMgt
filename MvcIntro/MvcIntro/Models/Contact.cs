using System.ComponentModel.DataAnnotations;

namespace MvcIntro.Models
{
    public class Contact
    {
        public Contact()
        {
            Id = 0;
            UserId = 0;
            Name = "";
            Address = "";
        }
        public Contact(string name, string address)
        {
            Name = name;
            Address = address;
        }
        public virtual int Id { get; set; }
        public virtual int UserId { get; set; }
        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual string Address { get; set; }
        }
    }

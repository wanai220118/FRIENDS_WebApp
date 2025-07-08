using System;
using System.ComponentModel.DataAnnotations;

namespace FRIENDS_WebApp.Models
{
    public class Friend
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
    }
}
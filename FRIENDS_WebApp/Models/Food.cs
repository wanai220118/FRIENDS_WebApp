using System;
using System.ComponentModel.DataAnnotations;

namespace FRIENDS_WebApp.Models
{
    public class Food
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Food Name")]
        public string FoodName { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        [Display(Name = "Date Added")]
        [DataType(DataType.Date)]
        public DateTime DateAdded { get; set; }
    }
}
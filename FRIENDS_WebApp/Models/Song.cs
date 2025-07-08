using System;
using System.ComponentModel.DataAnnotations;

namespace FRIENDS_WebApp.Models
{
    public class Song
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Artist { get; set; }

        public string Genre { get; set; }

        [Display(Name = "Duration (minutes)")]
        public double Duration { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace FRIENDS_WebApp.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        public string ImagePath { get; set; }
        public DateTime DateUploaded { get; set; }
    }
}
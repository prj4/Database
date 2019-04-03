using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhotoBookDatabase.Model
{
    public class Picture
    {
        public int PictureId { get; set; }
        [Required]
        [Url]
        public string URL { get; set; }
        [Required]
        public int EventPin { get; set; }
        public Event Event { get; set; }
        [Required]
        public int TakerId { get; set; }
        public PictureTaker PictureTaker { get; set; }
    }
}

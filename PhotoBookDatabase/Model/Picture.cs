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
        
        public string EventPin { get; set; }
        public Event Event { get; set; }
       
        public int? TakerId { get; set; }
        public PictureTaker PictureTaker { get; set; }
    }
}

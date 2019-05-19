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
        public string EventPin { get; set; }
        public Event Event { get; set; }
        public Guest Guest { get; set; }
        public int? GuestId { get; set; }
        public Host Host { get; set; }
        public int? HostId { get; set; }
    }
}

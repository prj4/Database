using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhotoBookDatabase.Model
{
    public class Guest
    {
        public int GuestId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string EventPin { get; set; }
        public Event Event { get; set; }

        
    }
}

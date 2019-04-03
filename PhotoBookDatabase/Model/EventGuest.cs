using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PhotoBookDatabase.Model
{
    public class EventGuest
    {
        [Required]
        public int GuestId { get; set; }
        public Guest Guest { get; set; }
        [Required]
        public int EventPin { get; set; }
        public Event Event { get; set; }
    }
}

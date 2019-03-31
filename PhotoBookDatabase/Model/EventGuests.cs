using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PhotoBookDatabase.Model
{
    public class EventGuests
    {
        public int FKEventPin { get; set; }
        public Event Event { get; set; }
        public int FKGuestId { get; set; }
        public Guest Guest { get; set; }

        
    }
}

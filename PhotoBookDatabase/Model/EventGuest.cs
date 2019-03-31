using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoBookDatabase.Model
{
    public class EventGuest
    {
        public int Guest_Id { get; set; }
        public Guest Guest { get; set; }

        public int Event_Pin { get; set; }
        public Event Event { get; set; }
    }
}

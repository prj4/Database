using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhotoBookDatabase.Model
{
    public class Guest : PictureTaker
    {
        
        public IList<EventGuest> EventGuests { get; set; }
        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace PhotoBookDatabase.Model
{
    public class Event
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Pin { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public IList<EventGuest> EventGuests { get; set; }
        public int HostId { get; set; }
        public Host Host { get; set; }
        public List<Picture> Pictures { get; set; }

        
    }
}

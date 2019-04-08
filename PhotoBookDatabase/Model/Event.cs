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
        
        [Key]
        public string Pin { get; set; }

        [Required]
        public string Location { get; set; }

        public string Description { get; set; }
        [Required]

        public string Name { get; set; }
        [Required]

        public DateTime StartDate { get; set; }
        [Required]

        public DateTime EndDate { get; set; }
        
        public List<Guest> Guests { get; set; }

        [Required]
        public int HostId { get; set; }
        public Host Host { get; set; }
        public List<Picture> Pictures { get; set; }

    }
}

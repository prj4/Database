using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PhotoBookDatabase.Model
{
    public class Host
    {
        public int HostId { get; set; }
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set;}
        public List<Event> Events { get; set; }  
        public List<Picture> Pictures { get; set; }
      
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PhotoBookDatabase.Model
{
    public class Host : PictureTaker
    {
        
        
        [Required]
        public string Username { get; set; }
        [Required]
        [MinLength(8)]
        public string PW { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set;}
        public List<Event> Events { get; set; }

        public override string ToString()
        {
            return $"{Username}, {PW}, {Email} {base.Name}";
        }
    }
}

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
        [EmailAddress]
        public string Email { get; set;}
        public List<Event> Events { get; set; }     
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhotoBookDatabase.Model
{
    public class PictureTaker
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int PictureTakerId { get; set; }
        public String Name { get; set; }
        public List<Picture> Pictures { get; set; }

        
    }
}

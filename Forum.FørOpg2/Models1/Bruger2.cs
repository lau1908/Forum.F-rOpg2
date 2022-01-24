using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Forum.FørOpg2.Models1
{
    public class Bruger2
    {
        
            [Required(ErrorMessage = "UserName or Email is required")]
            public string UserMail { get; set; }
            [Required(ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            public string Parsword { get; set; }
        
    }
}
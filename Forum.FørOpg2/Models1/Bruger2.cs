using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Forum.FørOpg2.Models1
{
    public class Bruger2
    {
        /// <summary>
        /// egentlig en bruger der tager værdierne fra de respektive sider ind
        /// </summary>
        [Required(ErrorMessage = "UserName or Email is required", AllowEmptyStrings = false)]//en errror message til brugernavnet/email
        public string UserMail { get; set; }//en get set metode til brugernavnet
        [Required(ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            public string Parsword { get; set; }
        
    }
}
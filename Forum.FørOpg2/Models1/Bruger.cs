using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Forum.FørOpg2.Models1
{
    public class Bruger
    {
        /// <summary>
        /// egentlig en bruger der tager værdierne fra de respektive sider ind
        /// </summary>
        //public int Bruger_ID { get; set; }
        [Required(ErrorMessage = "Please provide username", AllowEmptyStrings = false)] //en errror message til brugernavnet, her må den ikke være tom
        public string Username { get; set; } //en get set metode til brugernavnet
        [Required(ErrorMessage = "Please provide Password", AllowEmptyStrings = false)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be 8 char long.")]
        public string Parsword { get; set; }
        [RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$",
                    ErrorMessage = "Please provide valid email id")]
        public string Email { get; set; }
    }
}
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Route
    {
        [Required]
        [Key]
        public int Route_Id { get; set; }
        [Required(ErrorMessage = "Поле обязательно")]
        public string Start {  get; set; }
        [Required(ErrorMessage = "Поле обязательно")]
        public string End { get; set; }
        public int Transport_Id { get; set; }
    }
}

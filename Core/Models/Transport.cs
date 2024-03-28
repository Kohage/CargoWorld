using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Transport
    {
        [Required]
        [Key]
        public int Transport_Id { get; set; }
        [Required(ErrorMessage = "Поле обязательно")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Поле обязательно")]
        public string Matricula { get; set; }
    }
}

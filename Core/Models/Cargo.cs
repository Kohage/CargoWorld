using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Cargo
    {
        [Required]
        [Key]
        public int Cargo_Id { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Поле обязательно")]
        public double Weight { get; set; }
        [Required(ErrorMessage = "Поле обязательно")]
        public double Volume { get; set; }
        public byte[] Image { get; set; }
        public int Client_Id { get; set; }
    }
}

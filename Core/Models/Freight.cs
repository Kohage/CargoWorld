using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Freight
    {
        [Required]
        [Key]
        public int Freight_Id { get; set; }

        public int Cargo_Id { get; set; }
        public int Route_Id { get; set; }
        public System.DateTime SendDate { get; set; }
        public System.DateTime ArriveDate { get; set; }
    }
}

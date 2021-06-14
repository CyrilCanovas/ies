using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IesWebPortal.Models
{
    public class Inventory
    {
        [Key]
        public string Depot { get; set; }
        [Key]
        public string Reference { get; set; }
        [Key]
        public string NoLot { get; set; }
        public string  CodeFamille { get; set; }
        public string  Designation { get; set; }
        public double QuantiteStock { get; set; }
        public double ? PointEclair { get; set; }
        public DateTime? DateFabrication { get; set; }
        public DateTime? DatePeremption { get; set; }
        public double ? CTE { get; set; }

        public string Dangeureux { get; set; }
    }
}

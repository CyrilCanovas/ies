using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IesWebPortal.Models
{
    public class FlashPoint
    {
        [Key]
        public string Depot { get; set; }
        [Key]
        public string Reference { get; set; }
        public string CodeFamille { get; set; }
        [Key]
        public string Emplacement { get; set; }
        public string Designation { get; set; }
        public double QuantiteStock { get; set; }
        public double? PointEclair { get; set; }
        public double? Coeff { get; set; }
        public double? CTE { get; set; }
        public string Dangeureux { get; set; }
        public string PhraseRisque { get; set; }
        public string NumeroEiniecs { get; set; }
        public string RID { get; set; }
        public string IMDG { get; set; }
        public string ICADIATA { get; set; }
        public string Pictogramme { get; set; }
        public string  UN { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IesWebPortal.Models
{
    public class PrintModel
    {
        public string DocumentNo { get; set; }
        public int DocumentType { get; set; }
        public bool? OnlyAddress { get; set; }
        public string ReportName { get; set; }
        public string ReportLanguage { get; set; }
        public int? ShiftTagCount {get;set;}
        public PrintModelDetail[] Lines { get; set; }
    }
}

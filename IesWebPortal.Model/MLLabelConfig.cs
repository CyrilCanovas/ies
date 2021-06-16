using IesWebPortal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IesWebPortal.Model
{
    public class MLLabelConfig : IMLLabelConfig
    {
        public string ReportName { get; set; }
        public string Description { get; set; }
        public string Settings { get; set; }
        public int ColCount { get; set; }
        public int RowCount { get; set; }
    }
}

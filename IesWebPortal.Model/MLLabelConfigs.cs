using IesWebPortal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IesWebPortal.Model
{
    public class MLLabelConfigs:Dictionary<string,IMLLabelConfig>, IMLLabelConfigs
    {
        public MLLabelConfigs(IEnumerable<MLLabelConfig> mlLabelConfigs) : base(mlLabelConfigs.Select(x => new KeyValuePair<string, IMLLabelConfig>(x.ReportName, x)))
        {

        }
    }
}

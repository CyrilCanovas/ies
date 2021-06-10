using System;
using System.Collections.Generic;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public class SalesHeader : DocHeader
    {
    
        protected override int GetDomaine()
        {
            return 0;
        }
    
        public SalesHeader()
            : base()
        {
            Transaction     = 11;
            Regime          = 21;
            TypeColis       = 1;
            Colisage        = 1;
        }
    }
}

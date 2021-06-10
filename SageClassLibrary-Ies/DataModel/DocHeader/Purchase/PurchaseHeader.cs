using System;
using System.Collections.Generic;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public class PurchaseHeader : DocHeader
    {
        public PurchaseHeader()
            : base()
        {
            Transaction  = 0;
            Regime       = 0;
            TypeColis    = 1;
            Colisage     = 1;
        }


        protected override int GetDomaine()
        {
            return 1;
        }
    }
}

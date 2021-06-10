using System;
using System.Collections.Generic;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public class InventoryHeader : DocHeader
    {
        protected override int GetDomaine()
        {
            return 2;
        }

        protected override string GetTiers()
        {
            return DeNo.ToString();
        }
        public InventoryHeader()
            : base()
        {
            Transaction  = 0;
            Regime       = 0;
            TypeColis    = 1;
            Colisage     = 1;
        }
    }

}

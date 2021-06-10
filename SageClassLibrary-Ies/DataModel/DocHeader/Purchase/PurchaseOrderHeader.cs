using System;
using System.Collections.Generic;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public class PurchaseOrderHeader : PurchaseHeader
    {
        public PurchaseOrderHeader():base()
        {
            //lines = new DocLineList<PurchaseOrderLine>(this);
        }
    }
}

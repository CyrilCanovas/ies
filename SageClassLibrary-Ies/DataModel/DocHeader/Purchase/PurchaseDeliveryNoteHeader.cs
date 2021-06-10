using System;
using System.Collections.Generic;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public class PurchaseDeliveryNoteHeader : PurchaseHeader
    {
        public PurchaseDeliveryNoteHeader():base()
        {
            //lines = new DocLineList<PurchaseDeliveryNoteLine>(this);
        }
    }
}

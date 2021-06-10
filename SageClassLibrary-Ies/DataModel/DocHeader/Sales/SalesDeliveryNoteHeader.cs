using System;
using System.Collections.Generic;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public class SalesDeliveryNoteHeader : SalesHeader
    {
        public SalesDeliveryNoteHeader():base()
        {
            //lines = new DocLineList<SalesDeliveryNoteLine>(this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public class SalesInvoiceHeader : SalesHeader
    {

        public SalesInvoiceHeader():base()
        {
            //lines = new DocLineList<SalesInvoiceLine>(this);
            
        }
    }
}

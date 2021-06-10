using System;
using System.Collections.Generic;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public class SalesOrderHeader : SalesHeader
    {
        public SalesOrderHeader():base()
        {
            //lines = new DocLineList<SalesOrderLine>(this);
        }
    }
}

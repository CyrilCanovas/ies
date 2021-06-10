using System;
using System.Collections.Generic;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public abstract class PurchaseLine : DocLine
    {
        public PurchaseLine():base()
        {
            
        }
    
        protected override int GetDomaine()
        {
            return 1;
        }
    }
}

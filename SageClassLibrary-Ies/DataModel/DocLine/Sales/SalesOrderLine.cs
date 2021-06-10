using System;
using System.Collections.Generic;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public class SalesOrderLine : SalesLine
    {
        public SalesOrderLine():base()
        {
            
        }
        public override void DoBeforeInsert()
        {
            Valorise = (IsComment ? 0 : 1);
            base.DoBeforeInsert();
            
            if (IsComment) return;
            //DateBL = DatePiece;
            //DateBC = DatePiece;
            //DatePL = DatePiece;
            
            if (QtePL != Qte) QtePL = Qte;
            //if (QteBC != Qte) QteBC = Qte;
        }

    }
}

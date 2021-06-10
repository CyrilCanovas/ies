using System;
using System.Collections.Generic;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public abstract class SalesLine : DocLine
    {
        public SalesLine():base()
        {
            
        }
        public override void DoBeforeInsert()
        {
            base.DoBeforeInsert();
            Valorise = (IsComment ? 0 : 1);

            //DateBL = DatePiece;
            //DateBC = DatePiece;
            //DatePL = DatePiece;

            //if (QtePL != Qte) QtePL = Qte;
            //if (QteBC != Qte) QteBC = Qte;
        }
        protected override int GetDomaine()
        {
            return 0;
        }
    }
}

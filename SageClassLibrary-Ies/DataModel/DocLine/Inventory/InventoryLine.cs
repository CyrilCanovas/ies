using System;
using System.Collections.Generic;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public abstract class InventoryLine : DocLine
    {
        public InventoryLine():base()
        {
            
        }
        protected override int GetDeNo()
        {
            if (DocHeader != null) return Convert.ToInt32(DocHeader.Tiers);
            else return base.GetDeNo();
        }
        public override void DoBeforeInsert()
        {
            base.DoBeforeInsert();
            Valorise = (IsComment ? 0 : 1);
            
            DateBL = DatePiece;
            DateBC= DatePiece;
            DatePL = DatePiece;

            if (QtePL != Qte) QtePL = Qte;
            if (QteBC != Qte) QteBC = Qte;
        }
        protected override int GetDomaine()
        {
            return 2;
        }
    }
}

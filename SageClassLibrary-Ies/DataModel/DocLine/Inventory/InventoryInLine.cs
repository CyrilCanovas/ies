using System;
using System.Collections.Generic;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public class InventoryInLine : InventoryLine
    {
        public InventoryInLine():base()
        {
            
        }
        public  override void DoBeforeInsert()
        {
            base.DoBeforeInsert();
            if (Item == null) return;
            if (PrixUnitaire == 0) PrixUnitaire = Item.PrixAch;
        }
    }
}

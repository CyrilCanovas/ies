using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SageClassLibrary.DataModel
{
    using IQToolkit;
    using IQToolkit.Data;
    using IQToolkit.Extensions;
    using SageClassLibrary.Other;
 
    public partial class DocNumManager : SageObject
    {
        
        private int domaine = 0;
        public int Domaine { get { return domaine; } set { domaine = value; } }
        private int idcol = 0;
        public int IdCol { get { return idcol; } set { idcol = value; } }
        private int souche = 0;
        public int Souche { get { return souche; } set { souche = value; } }

        static int DocTypeToIdCol(int doctype)
        {
            int result = -1;
            switch ((EnumDocType)doctype)
            {
                case EnumDocType.SalesQuote: result = 0;
                    break;
                case EnumDocType.SalesOrder: result = 1;
                    break;
                case EnumDocType.SalesInvoice: result = 6;
                    break;
                case EnumDocType.SalesDeliveryNote: result = 3;
                    break;
                case EnumDocType.InventoryOutMvt: result = 1;
                    break;
                case EnumDocType.InventoryInMvt: result = 0;
                    break;
                case EnumDocType.PurchaseOrder: result = 2;
                    break;
                case EnumDocType.PurchaseInvoice: result = 6;
                    break;
                case EnumDocType.PurchaseDeliveryNote: result = 3;
                    break;
            }
            return result;
        }
        static public string GetNextNum(DocHeader docheader)
        {
            return GetNextNum(docheader, (docheader as ICustomDbOwner).DbOwner);
        }
        static public string GetNextNum(DocHeader docheader, DbEntityProvider provider)
        {

            var docnummanagers = SageClassLibrary.SageDataContext.GetEntityTable<DocNumManager>(provider);
                

            var q = from i in docnummanagers
                    where i.Domaine == docheader.Domaine
                    && i.Souche == docheader.Souche
                    && i.IdCol == DocTypeToIdCol(docheader.DocType)
                    select i.Piece;
            return q.SingleOrDefault() ?? string.Empty;
        }

        static public string IncStr(string value)
        {
            return 
                Tools.IncStr(value, MaxLen);
        }

        public static readonly int MaxLen = 8;
    }

}

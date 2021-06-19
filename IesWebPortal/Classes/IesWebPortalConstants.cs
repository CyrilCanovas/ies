using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IesWebPortal.Classes
{
    public static class IesWebPortalConstants
    {
        public const string CST_LABELCOUNT = "LabelCount";
        public const string CST_TARE = "Tare";
        public const string CST_NETWEIGHT = "NetWeight";
        public const string CST_QTY = "Qty";
        public const string CST_SERIALNO = "SerialNo";
        public const string CST_MANUFACTUREDDATE = "ManufacturedDate";
        public const string CST_BESTBEFOREDATE = "BestBeforeDate";


        public const string COOKIE_LASTREPORTNAME = "LastReportName";
        public const string COOKIE_ONLYADDRESS = "OnlyAddress";
        public const string COOKIE_LASTDOCUMENTTYPE = "LastDocumentType";
        public const string COOKIE_LASTSORTSALEPURCHASE = "LastSalePurchase";

        public const string REPORT_PATH = "Reports";
        public const string PICTURE_PATH = "Pictures";

        public static KeyValuePair<int, string>[] CST_DOCUMENTS = new KeyValuePair<int, string>[]{
                                                                    new KeyValuePair<int, string>((int)SageClassLibrary.DataModel.EnumDocType.SalesOrder,"Bon de commande (vente)"),
                                                                    new KeyValuePair<int, string>((int)SageClassLibrary.DataModel.EnumDocType.SalesDeliveryNote,"Bon de livraison (vente)"),
                                                                    new KeyValuePair<int, string>((int)SageClassLibrary.DataModel.EnumDocType.PurchaseOrder,"Bon de commande (achat)"),
                                                                    new KeyValuePair<int, string>((int)SageClassLibrary.DataModel.EnumDocType.PurchaseDeliveryNote,"Bon de livraison (achat)")
                                                                };


    }
}

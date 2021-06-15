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

        public const string REPORT_PATH = "~/Reports/";
        public const string PICTURE_PATH = "~/Pictures/";

        public static KeyValuePair<int, string>[] CST_DOCUMENTS = new KeyValuePair<int, string>[]{
                                                                    new KeyValuePair<int, string>((int)SageClassLibrary.DataModel.EnumDocType.SalesOrder,"Bon de commande (vente)"),
                                                                    new KeyValuePair<int, string>((int)SageClassLibrary.DataModel.EnumDocType.SalesDeliveryNote,"Bon de livraison (vente)"),
                                                                    new KeyValuePair<int, string>((int)SageClassLibrary.DataModel.EnumDocType.PurchaseOrder,"Bon de commande (achat)"),
                                                                    new KeyValuePair<int, string>((int)SageClassLibrary.DataModel.EnumDocType.PurchaseDeliveryNote,"Bon de livraison (achat)")
                                                                };

        //public static KeyValuePair<string, LabelConfig>[] CST_REPORTS =

        //                                                               (new LabelConfig[]{
        //                                                                new LabelConfig(){ReportName="BigLabel.rdlc",
        //                                                                    Description="Etiquettes grand format",
        //                                                                    Settings=global::IesWebPortal.Properties.Settings.Default.BigLabelSettings
        //                                                                },
        //                                                                new LabelConfig(){ReportName="SmallLabel.rdlc",
        //                                                                    Description="Etiquettes petit format",
        //                                                                    Settings=global::IesWebPortal.Properties.Settings.Default.SmallLabelSettings
        //                                                                },
        //                                                                new LabelConfig(){ReportName="SampleLabel.rdlc",
        //                                                                    Description="Etiquettes echantillons",
        //                                                                    Settings=global::IesWebPortal.Properties.Settings.Default.SampleLabelSettings,
        //                                                                    //Settings=global::IesWebPortal.Properties.Settings.Default.    
        //                                                                }
        //                                                                ,
        //                                                                //new LabelConfig(){ReportName="Report12x3.rdlc",
        //                                                                //    Description="Etiquettes echantillons 12x3 (ancien)",
        //                                                                //    Settings=null,
        //                                                                //    ColCount=3,
        //                                                                //    RowCount=12
        //                                                                //}
        //                                                                //,

        //                                                                new LabelConfig(){ReportName="Report16x4.rdlc",
        //                                                                    Description="Etiquettes echantillons 16x4",
        //                                                                    Settings=null,
        //                                                                    ColCount=4,
        //                                                                    RowCount=16
        //                                                                },
        //                                                                new LabelConfig(){ReportName="DeliveryLabel.rdlc",
        //                                                                    Description="Etiquette livraison client",
        //                                                                    Settings=global::IesWebPortal.Properties.Settings.Default.DeliveryLabelSettings
        //                                                                }
        //                                                               }).Select(x => new KeyValuePair<string, LabelConfig>(x.ReportName, x)).ToArray();
    }
}

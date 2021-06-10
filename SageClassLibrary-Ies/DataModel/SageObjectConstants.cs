using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public static class SageObjectConstants
    {
        static SageObjectConstants()
        {
            IntBuildDictonaries();
        }

        private static  Dictionary<Type, EnumDocType> sagedoctypes = null;
        private static  Dictionary<Type, Type> linkedtypes2 = null;

        private static void IntBuildDictonaries()
        {

            var _sagedoctypes = new Dictionary<Type, EnumDocType>();
            foreach (var q in linkedtypes.AsEnumerable())
                foreach (var b in q.Value) _sagedoctypes.Add(b, q.Key);

            var _linkedtypes2 = new Dictionary<Type, Type>();
            foreach (var q in linkedtypes.Values)
            {
                if (q.Length != 2) continue;
                _linkedtypes2.Add(q[0], q[1]);
                _linkedtypes2.Add(q[1], q[0]);
            }

            linkedtypes2 = _linkedtypes2;
            sagedoctypes = _sagedoctypes;

        }
        public static Type GetLinkedType(Type type)
        {
            return linkedtypes2[type];
        }

        public static int GetDocType(Type type)
        {
            return (int)sagedoctypes[type];
        }

        public static Type GetTypeFromDocType(EnumDocType doctype, Type ancestor)
        {
            Type result = null;
            Type[] types = linkedtypes[doctype];
            foreach (var q in types)
                if (q.IsSubclassOf(ancestor)) return q;

            return result;
        }

        public static Type GetTypeFromDocType(int doctype, Type ancestor)
        {
            return GetTypeFromDocType((EnumDocType)doctype, ancestor);
        }
        private static Dictionary<EnumDocType, Type[]> linkedtypes =
             new List<KeyValuePair<EnumDocType, Type[]>>()
            {
                new KeyValuePair<EnumDocType, Type[]>(EnumDocType.SalesQuote,
                                                        new Type[]{
                                                            typeof(SalesQuoteLine),
                                                            typeof(SalesQuoteHeader)  
                                                        }),
                new KeyValuePair<EnumDocType, Type[]>(EnumDocType.SalesOrder,
                                                        new Type[]{
                                                            typeof(SalesOrderLine),
                                                            typeof(SalesOrderHeader)
                                                        
                                                        }),
                new KeyValuePair<EnumDocType, Type[]>(EnumDocType.SalesInvoice,
                                                        new Type[]{
                                                            typeof(SalesInvoiceLine),
                                                            typeof(SalesInvoiceHeader)
                                                        }),
                new KeyValuePair<EnumDocType, Type[]>(EnumDocType.SalesDeliveryNote,
                                                        new Type[]{
                                                            typeof(SalesDeliveryNoteLine),
                                                            typeof(SalesDeliveryNoteHeader)
                                                        }),
                new KeyValuePair<EnumDocType, Type[]>(EnumDocType.InventoryOutMvt,
                                                        new Type[]{
                                                            typeof(InventoryOutLine),
                                                            typeof(InventoryOutHeader)
                                                        }),
                new KeyValuePair<EnumDocType, Type[]>(EnumDocType.InventoryInMvt,
                                                        new Type[]{
                                                            typeof(InventoryInLine),
                                                            typeof(InventoryInHeader)
                                                        }),
                new KeyValuePair<EnumDocType, Type[]>(EnumDocType.PurchaseOrder,
                                                        new Type[]{
                                                            typeof(PurchaseOrderLine),
                                                            typeof(PurchaseOrderHeader)
                                                        }),
                //new KeyValuePair<EnumDocType, Type[]>(EnumDocType.PurchaseInvoice,
                //                                        new Type[]{
                //                                            typeof(),
                //                                            typeof()
                //                                        }),
                new KeyValuePair<EnumDocType, Type[]>(EnumDocType.PurchaseDeliveryNote,
                                                        new Type[]{
                                                            typeof(PurchaseDeliveryNoteLine),
                                                            typeof(PurchaseDeliveryNoteHeader)
                                                        })
            }.ToDictionary(x => x.Key, x => x.Value);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public enum EnumCpType
    {
        Sales = 0,
        Purchase = 1,
        Stock = 2
    }

    public enum EnumItemStatus
    {
        Enabled = 0,
        Disabled = 1
    }

    public enum EnumCustomerStatus
    {
        Enabled = 0,
        Disabled = 1
    }

    public enum EnumMemoType
    {
        ItemMemo = 0,
        DocumentMemo = 1,
        LineMemo = 2
    }

    public enum EnumCostingMethod
    {
        None = 0,
        Serial = 1,
        CMUP = 2,
        FIFO = 3,
        LIFO = 4,
        Lot = 5
    }

    public enum CustomerVendorType
    {
        Customer = 0,
        Vendor = 1
    }

    public enum EnumDocType
    {
        SalesQuote = 0,
        SalesOrder = 1,
        //SalesDeliveryPreparation=2,
        SalesDeliveryNote = 3,
        SalesInvoice = 6,
        InventoryOutMvt = 21,
        InventoryInMvt = 20,
        PurchaseOrder = 12,
        PurchaseDeliveryNote = 13,
        PurchaseInvoice = 16,
    }

    public enum EnumDiscountType
    {
        Amount = 0,
        Percent = 1,
        Qty = 2

    }

    public enum EnumTypeTaux
    {
        Percent = 0,
        Amount = 1,
        Qty = 2
    }


}

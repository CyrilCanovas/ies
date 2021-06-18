using System;
using System.Collections.Generic;

namespace IesWebPortal.Services.Interfaces
{
    public interface IDataService
    {
        IMLItemInventory[] GetInventories(Func<string, string> mapPicturePath);
        IMLItemLotSerial[] GetItemsLotSerial();
        IMLSalePurchaseHeader[] GetDocHeaders(int docType);
        IMLSalePurchaseLine[] GetDocLines(int doctype, string docpiece);
        IMLSalePurchaseHeader GetDocHeader(int doctype, string docpiece);
        IDictionary<string, string> ItemToVendor(string[] itemNos);
        IMLSalePurchaseHeader FillLabels(IMLSalePurchaseHeader mlSalePurchaseHeader, string codepays, Func<string, string> mapPicturePath);
    }
}
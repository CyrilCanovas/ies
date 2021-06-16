﻿using System;

namespace IesWebPortal.Services.Interfaces
{
    public interface IDataService
    {
        IMLItemInventory[] GetInventories(Func<string, string> mapPicturePath);
        IMLItemLotSerial[] GetItemsLotSerial();
        IMLSalePurchaseHeader[] GetDocHeaders(int docType);
        IMLSalePurchaseLine[] GetDocLines(int doctype, string docpiece);
        IMLSalePurchaseHeader GetDocHeader(int doctype, string docpiece);

    }
}
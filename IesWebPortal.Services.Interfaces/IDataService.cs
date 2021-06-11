using System;

namespace IesWebPortal.Services.Interfaces
{
    public interface IDataService
    {
        IMLItemInventory[] GetInventories(Func<string, string> mapPicturePath);
    }
}
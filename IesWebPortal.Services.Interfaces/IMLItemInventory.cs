namespace IesWebPortal.Services.Interfaces
{
    public interface IMLItemInventory
    {
        string DeIntitule { get; set; }
        int DeNo { get; set; }
        IMLItemInfo Item { get; set; }
        string ItemNo { get; set; }
        string LocationCode { get; set; }
        int NoPrincipal { get; set; }
        double Qty { get; set; }
    }
}
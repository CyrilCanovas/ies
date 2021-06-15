
namespace IesWebPortal.Services.Interfaces
{ 
    public interface IMLShipTo
    {
        string Address { get; set; }
        string Address1 { get; set; }
        string City { get; set; }
        string Country { get; set; }
        string Description { get; set; }
        string Fax { get; set; }
        string FullAddress { get; }
        int LiNo { get; set; }
        string Mail { get; set; }
        string Phone { get; set; }
        string ZipCode { get; set; }
    }
}
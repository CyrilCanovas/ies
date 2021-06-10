using IesWebPortal.Services.Interfaces;

namespace IesWebPortal.Services.Interfaces
{
    public interface IMLMemoStruct
    {
        string Description { get; set; }
        string FullDescription { get; set; }
        ELanguages Language { get; set; }
        string ShortName { get; set; }
    }
}
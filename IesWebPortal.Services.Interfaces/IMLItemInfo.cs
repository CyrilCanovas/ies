using IesWebPortal.Services.Interfaces;
using System.Collections.Generic;

namespace IesWebPortal.Services.Interfaces
{
    public interface IMLItemInfo
    {
        string Dangerous { get; set; }
        string DangerousEnglish { get; set; }
        string DangerousFrench { get; set; }
        string Description { get; set; }
        string Family { get; set; }
        string Files { get; set; }
        double FlashPoint { get; set; }
        string ICADIATA { get; set; }
        string IMDG { get; set; }
        string ItemNo { get; set; }
        byte[] Picture1 { get; set; }
        byte[] Picture2 { get; set; }
        byte[] Picture3 { get; set; }
        byte[] Picture4 { get; set; }
        byte[] Picture5 { get; set; }
        string RID { get; set; }
        string GetEinecsCodes();
        string GetEinecsDescriptions();
        string GetRisksOthers(ELanguages language);
        string GetRisksP(ELanguages language);
        string GetUN();
        string GetUNCode();
        void SetEiniecs(Dictionary<string, IMLMemoStruct> keyValuePairs);
        void SetRisks(Dictionary<string, IMLMemoStruct[]> keyValuePairs);
        void SetUn(IMLMemoStruct memoStruct);
        Dictionary<string, IMLMemoStruct[]> GetRisks();
        Dictionary<string, IMLMemoStruct> GetEiniecs();
        IMLMemoStruct GetUnMemoStruct();
    }
}
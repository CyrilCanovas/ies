using System.Collections.Generic;

namespace IesWebPortal.Services.Interfaces
{
    public interface IMemoManager
    {
        IMLMemoStruct[] GetEniecs(IEnumerable<string> values);
        IMLMemoStruct[] GetRisks(IEnumerable<string> values);
        IMLMemoStruct[] GetUN(IEnumerable<string> values);
        bool IsEniec(string value);
        bool IsPicture(string value);
        bool IsRisk(string value);
        bool IsUN(string value);
    }
}
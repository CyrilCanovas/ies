using IesWebPortal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IesWebPortal.Model
{
    public class MLMemoStruct : IMLMemoStruct
    {
        public string ShortName { get; set; }
        public ELanguages Language { get; set; }
        public string Description { get; set; }
        public string FullDescription { get; set; }
    }
}

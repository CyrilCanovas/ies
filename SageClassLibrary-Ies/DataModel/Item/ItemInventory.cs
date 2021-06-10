using System;
using System.Collections.Generic;
using System.Text;

namespace SageClassLibrary.DataModel
{
    
    public partial class ItemInventory:SageObject
    {
        private string refarticle = string.Empty;
    
        public string RefArticle { get { return refarticle; } set { refarticle = value; } }
        private int deno = 0;
        public int DeNo { get { return deno; } set { deno = value; } }

    }
}

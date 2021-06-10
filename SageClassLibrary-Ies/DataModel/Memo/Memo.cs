using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public partial class Memo : SageObject
    {
        public int GlNo;
        public int Domaine;
        public string Intitule;
        public string Raccourci;
        public DateTime PeriodeDeb;
        public DateTime PeriodeFin;
        //public int GlDocLigne;
        public string Text;
    }
}


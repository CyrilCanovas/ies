using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SageClassLibrary.DataModel
{
   
    public class Tax : SageObject
    {
        public string Intitule;
        public int TypeTaux;
        public double Taux;
        public int TaType;
        public string CgNum;
        public int TaNo;
        public string TaCode;
        public int Np;
        public int Sens;
        public int Provenance;
        public string Regroup;
        public double Assujet;
        public string GrilleBase;
        public string GrilleTaxe;
    }
}

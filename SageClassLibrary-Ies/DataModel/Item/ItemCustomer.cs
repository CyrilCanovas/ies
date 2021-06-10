using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SageClassLibrary.DataModel
{

    public partial class ItemCustomer : SageObject
    {
        
        public string RefArticle;
        public int Categorie; 
        public double Coef; 
        public double PrixVen;
        public int PrixTTC;
        public int Arrondi;
        public int QteMont;
        public int EgChamp;
        public double PrixDev;
        public int Devise;
        public string Tiers;
        public double Remise;
        public int Calcul;
        public int TypeRem;
        public string RefClient;
        public double CoefNouv;
        public double PrixVenNouv;
        public double PrixDevNouv;
        public double RemiseNouv;
        public DateTime DateApplication;

    }
}
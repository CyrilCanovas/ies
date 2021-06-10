
using System;
using System.Collections.Generic;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public  partial class DocLine 
    {
        private string piecebc = string.Empty;
        
        public string PieceBC { get { return piecebc; } set { piecebc = value; } }
        private string piecebl = string.Empty;
        
        public string PieceBL { get { return piecebl; } set { piecebl = value; } }
        private DateTime datebc = DateTime.MinValue;
        
        public DateTime DateBC { get { return datebc; } set { datebc = value; } }
        private DateTime datebl = DateTime.MinValue;
        
        public DateTime DateBL { get { return datebl; } set { datebl = value; } }
       
        
        public int TNomencl;
        private int tremexep = 0;
        
        public int TRemExep { get { return tremexep; } set { tremexep = value; } }
 //       private string refarticle = string.Empty;


        private double qte = 0;
        
        public double Qte { get { return qte; } set { qte = value; } }
        private double qtebc = 0;
        
        public double QteBC { get { return qtebc; } set { qtebc = value; } }
        private double qtebl = 0;
        
        public double QteBL { get { return qtebl; } set { qtebl = value; } }
        private double poidsnet = 0;
        
        public double PoidsNet { get { return poidsnet; } set { poidsnet = value; } }
        private double poidsbrut = 0;
        
        public double PoidsBrut { get { return poidsbrut; } set { poidsbrut = value; } }
        private double remise01valeur = 0;
        
        public double Remise01Valeur { get { return remise01valeur; } set { remise01valeur = value; } }
        private int remise01type = 0;
        
        public int Remise01Type { get { return remise01type; } set { remise01type = value; } }
        private double remise02valeur = 0;
        
        public double Remise02Valeur { get { return remise02valeur; } set { remise02valeur = value; } }
        private int remise02type = 0;
        
        public int Remise02Type { get { return remise02type; } set { remise02type = value; } }
        private double remise03valeur = 0;
        
        public double Remise03Valeur { get { return remise03valeur; } set { remise03valeur = value; } }
        private int remise03type = 0;
        
        public int Remise03Type { get { return remise03type; } set { remise03type = value; } }
        private double prixunitaire = 0;
        
        public double PrixUnitaire { get { return prixunitaire; } set { prixunitaire = value; } }
        private double prixunitairebc = 0;
        
        public double PrixUnitaireBC { get { return prixunitairebc; } set { prixunitairebc = value; } }
        private double taxe1 = 0;
        
        public double Taxe1 { get { return taxe1; } set { taxe1 = value; } }
        private int typetaux1 = 0;
        
        public int TypeTaux1 { get { return typetaux1; } set { typetaux1 = value; } }
        private int typetaxe1 = 0;
        
        public int TypeTaxe1 { get { return typetaxe1; } set { typetaxe1 = value; } }
        private double taxe2 = 0;
        
        public double Taxe2 { get { return taxe2; } set { taxe2 = value; } }
        private int typetaux2 = 0;
        
        public int TypeTaux2 { get { return typetaux2; } set { typetaux2 = value; } }
        private int typetaxe2 = 0;
        
        public int TypeTaxe2 { get { return typetaxe2; } set { typetaxe2 = value; } }
        private int no1 = 0;
        
        public int No1 { get { return no1; } set { no1 = value; } }
        private int no2 = 0;
        
        public int No2 { get { return no2; } set { no2 = value; } }
        private double prixru = 0;
        
        public double PrixRU { get { return prixru; } set { prixru = value; } }
        private double cmup = 0;
        
        public double CMUP { get { return cmup; } set { cmup = value; } }
        private int mvtstock = 0;
        
        public int MvtStock { get { return mvtstock; } set { mvtstock = value; } }
        private string reffourniss = string.Empty;
        
        public string RefFourniss { get { return reffourniss; } set { reffourniss = value; } }
        private double euqte = 0;
        
        public double EuQte { get { return euqte; } set { euqte = value; } }
        private int ttc = 0;
        
        public int TTC { get { return ttc; } set { ttc = value; } }
        private int noreference = 0;
        
        public int NoReference { get { return noreference; } set { noreference = value; } }
        private int typepl = 0;
        
        public int TypePL { get { return typepl; } set { typepl = value; } }
        private double pudevise = 0;
        
        public double PUDevise { get { return pudevise; } set { pudevise = value; } }
        private double puttc = 0;
        
        public double PuTTC { get { return puttc; } set { puttc = value; } }
        private double taxe3 = 0;
        
        public double Taxe3 { get { return taxe3; } set { taxe3 = value; } }
        private int typetaux3 = 0;
        
        public int TypeTaux3 { get { return typetaux3; } set { typetaux3 = value; } }
        private int typetaxe3 = 0;
        
        public int TypeTaxe3 { get { return typetaxe3; } set { typetaxe3 = value; } }
        private double frais = 0;
        
        public double Frais { get { return frais; } set { frais = value; } }
        //private int valorise = 0;
        
        public int Valorise; 
        
        public string RefCompose; 
        
        public int NonLivre; 
        
        public string RefClient; 
        
        public double MontantHT; 
        
        public double MontantTTC;
        
        public int FactPoids; 
        
        public int Escompte; 
        
        public string PiecePL;
        
        public DateTime DatePL; 
        
        public double QtePL; 
        
        public string NoColis; 
        
        public string NoSerie;
        
        public DateTime Peremption; 
        
        public DateTime Fabrication; 
        
        
        public string Complement;
    }
}

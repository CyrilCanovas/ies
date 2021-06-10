
using System;
using System.Collections.Generic;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public partial class Item 
    {
        private string photo = string.Empty;
        public string Photo { get { return photo; } set { photo = value; } }

        private string design = string.Empty;
        public string Design { get { return design; } set { design = value; } }
        private string codefamille = string.Empty;

        public string CodeFamille { get { return codefamille; } set { codefamille = value; } }
        private string substitut = string.Empty;

        public string Substitut { get { return substitut; } set { substitut = value; } }
        private string raccourci = string.Empty;

        public string Raccourci { get { return raccourci; } set { raccourci = value; } }
        private int garantie = 0;

        public int Garantie { get { return garantie; } set { garantie = value; } }
        private int unitepoids = 0;

        public int UnitePoids { get { return unitepoids; } set { unitepoids = value; } }
        private double poidsnet = 0;

        public double PoidsNet { get { return poidsnet; } set { poidsnet = value; } }
        private double poidsbrut = 0;

        public double PoidsBrut { get { return poidsbrut; } set { poidsbrut = value; } }
        private int uniteven = 0;

        public int UniteVen { get { return uniteven; } set { uniteven = value; } }
        private double prixach = 0;

        public double PrixAch { get { return prixach; } set { prixach = value; } }
        
        private double coef = 0;
        
        public double Coef { get { return coef; } set { coef = value; } }

        private double prixven = 0;

        public double PrixVen { get { return prixven; } set { prixven = value; } }
        private int prixttc = 0;

        public int PrixTTC { get { return prixttc; } set { prixttc = value; } }
        private int gamme1 = 0;

        public int Gamme1 { get { return gamme1; } set { gamme1 = value; } }
        private int gamme2 = 0;

        public int Gamme2 { get { return gamme2; } set { gamme2 = value; } }
        private int suivistock = 0;

        public int SuiviStock { get { return suivistock; } set { suivistock = value; } }
        private int nomencl = 0;

        public int Nomencl { get { return nomencl; } set { nomencl = value; } }
        private string stat01 = string.Empty;

        public string Stat01 { get { return stat01; } set { stat01 = value; } }
        private string stat02 = string.Empty;

        public string Stat02 { get { return stat02; } set { stat02 = value; } }
        private string stat03 = string.Empty;

        public string Stat03 { get { return stat03; } set { stat03 = value; } }
        private string stat04 = string.Empty;

        public string Stat04 { get { return stat04; } set { stat04 = value; } }
        private string stat05 = string.Empty;

        public string Stat05 { get { return stat05; } set { stat05 = value; } }
        private int escompte = 0;

        public int Escompte { get { return escompte; } set { escompte = value; } }
        private int delai = 0;

        public int Delai { get { return delai; } set { delai = value; } }
        private int horsstat = 0;

        public int HorsStat { get { return horsstat; } set { horsstat = value; } }
        private int vtedebit = 0;
        
        public int VteDebit { get { return vtedebit; } set { vtedebit = value; } }
        private int notimp = 0;
        
        public int NoTimp { get { return notimp; } set { notimp = value; } }
        private int sommeil = 0;
        
        public int Sommeil { get { return sommeil; } set { sommeil = value; } }
        private string langue1 = string.Empty;
        
        public string Langue1 { get { return langue1; } set { langue1 = value; } }
        private string langue2 = string.Empty;
        
        public string Langue2 { get { return langue2; } set { langue2 = value; } }
        private string codeediedcode1 = string.Empty;
        
        public string CodeEDIedCode1 { get { return codeediedcode1; } set { codeediedcode1 = value; } }
        private string codeediedcode2 = string.Empty;
        
        public string CodeEDIedCode2 { get { return codeediedcode2; } set { codeediedcode2 = value; } }
        private string codeediedcode3 = string.Empty;
        
        public string CodeEDIedCode3 { get { return codeediedcode3; } set { codeediedcode3 = value; } }
        private string codeediedcode4 = string.Empty;
        
        public string CodeEDIedCode4 { get { return codeediedcode4; } set { codeediedcode4 = value; } }
        private string codebarre = string.Empty;
        
        public string CodeBarre { get { return codebarre; } set { codebarre = value; } }
        private string codefiscal = string.Empty;
        
        public string CodeFiscal { get { return codefiscal; } set { codefiscal = value; } }
        private string pays = string.Empty;
        
        public string Pays { get { return pays; } set { pays = value; } }
        private string frais01frdenomination = string.Empty;
        
        public string Frais01FRDenomination { get { return frais01frdenomination; } set { frais01frdenomination = value; } }
        private double frais01frrem01remvaleur = 0;
        
        public double Frais01FRRem01RemValeur
        {
            get
            {
                return frais01frrem01remvaleur
       ;
            }
            set
            {
                frais01frrem01remvaleur
       = value;
            }
        }
        private int frais01frrem01remtype = 0;
        
        public int Frais01FRRem01RemType
        {
            get
            {
                return frais01frrem01remtype
       ;
            }
            set
            {
                frais01frrem01remtype
       = value;
            }
        }
        private double frais01frrem02remvaleur = 0;
        
        public double Frais01FRRem02RemValeur
        {
            get
            {
                return frais01frrem02remvaleur
       ;
            }
            set
            {
                frais01frrem02remvaleur
       = value;
            }
        }
        private int frais01frrem02remtype = 0;
        
        public int Frais01FRRem02RemType
        {
            get
            {
                return frais01frrem02remtype
       ;
            }
            set
            {
                frais01frrem02remtype
       = value;
            }
        }
        private double frais01frrem03remvaleur
 = 0;
        
        public double Frais01FRRem03RemValeur
        {
            get
            {
                return frais01frrem03remvaleur
       ;
            }
            set
            {
                frais01frrem03remvaleur
       = value;
            }
        }
        private int frais01frrem03remtype
 = 0;
        
        public int Frais01FRRem03RemType
        {
            get
            {
                return frais01frrem03remtype
       ;
            }
            set
            {
                frais01frrem03remtype
       = value;
            }
        }
        private string frais02frdenomination
 = string.Empty;
        
        public string Frais02FRDenomination
        {
            get
            {
                return frais02frdenomination
       ;
            }
            set
            {
                frais02frdenomination
       = value;
            }
        }
        private double frais02frrem01remvaleur
 = 0;
        
        public double Frais02FRRem01RemValeur
        {
            get
            {
                return frais02frrem01remvaleur
       ;
            }
            set
            {
                frais02frrem01remvaleur
       = value;
            }
        }
        private int frais02frrem01remtype
 = 0;
        
        public int Frais02FRRem01RemType
        {
            get
            {
                return frais02frrem01remtype
       ;
            }
            set
            {
                frais02frrem01remtype
       = value;
            }
        }
        private double frais02frrem02remvaleur
 = 0;
        
        public double Frais02FRRem02RemValeur
        {
            get
            {
                return frais02frrem02remvaleur
       ;
            }
            set
            {
                frais02frrem02remvaleur
       = value;
            }
        }
        private int frais02frrem02remtype
 = 0;
        
        public int Frais02FRRem02RemType
        {
            get
            {
                return frais02frrem02remtype
       ;
            }
            set
            {
                frais02frrem02remtype
       = value;
            }
        }
        private double frais02frrem03remvaleur
 = 0;
        
        public double Frais02FRRem03RemValeur
        {
            get
            {
                return frais02frrem03remvaleur
       ;
            }
            set
            {
                frais02frrem03remvaleur
       = value;
            }
        }
        private int frais02frrem03remtype
 = 0;
      
        public int Frais02FRRem03RemType
        {
            get
            {
                return frais02frrem03remtype
       ;
            }
            set
            {
                frais02frrem03remtype
       = value;
            }
        }
        private string frais03frdenomination
 = string.Empty;
        
        public string Frais03FRDenomination
        {
            get
            {
                return frais03frdenomination
       ;
            }
            set
            {
                frais03frdenomination
       = value;
            }
        }
        private double frais03frrem01remvaleur
 = 0;
        
        public double Frais03FRRem01RemValeur
        {
            get
            {
                return frais03frrem01remvaleur
       ;
            }
            set
            {
                frais03frrem01remvaleur
       = value;
            }
        }
        private int frais03frrem01remtype
 = 0;
        
        public int Frais03FRRem01RemType
        {
            get
            {
                return frais03frrem01remtype
       ;
            }
            set
            {
                frais03frrem01remtype
       = value;
            }
        }
        private double frais03frrem02remvaleur
 = 0;
        
        public double Frais03FRRem02RemValeur
        {
            get
            {
                return frais03frrem02remvaleur
       ;
            }
            set
            {
                frais03frrem02remvaleur
       = value;
            }
        }
        private int frais03frrem02remtype
 = 0;
        
        public int Frais03FRRem02RemType
        {
            get
            {
                return frais03frrem02remtype
       ;
            }
            set
            {
                frais03frrem02remtype
       = value;
            }
        }
        private double frais03frrem03remvaleur
 = 0;
        
        public double Frais03FRRem03RemValeur
        {
            get
            {
                return frais03frrem03remvaleur
       ;
            }
            set
            {
                frais03frrem03remvaleur
       = value;
            }
        }
        private int frais03frrem03remtype
 = 0;
        
        public int Frais03FRRem03RemType
        {
            get
            {
                return frais03frrem03remtype
       ;
            }
            set
            {
                frais03frrem03remtype
       = value;
            }
        }
        private int condition = 0;
        
        public int Condition { get { return condition; } set { condition = value; } }
        private double punet = 0;
        
        public double PuNet { get { return punet; } set { punet = value; } }
        private int contremarque = 0;
        
        public int Contremarque { get { return contremarque; } set { contremarque = value; } }
        private int factpoids = 0;
        
        public int FactPoids { get { return factpoids; } set { factpoids = value; } }
        private int factforfait = 0;
        
        public int FactForfait { get { return factforfait; } set { factforfait = value; } }
        private DateTime datecreation = DateTime.MinValue;
        
        public DateTime DateCreation { get { return datecreation; } set { datecreation = value; } }
        private int saisievar = 0;
        
        public int SaisieVar { get { return saisievar; } set { saisievar = value; } }
        private int transfere = 0;
        
        public int Transfere { get { return transfere; } set { transfere = value; } }
        private int publie = 0;
        
        public int Publie { get { return publie; } set { publie = value; } }
        private DateTime datemodif = DateTime.MinValue;
        
        public DateTime DateModif { get { return datemodif; } set { datemodif = value; } }
        private double prixachnouv = 0;
        
        public double PrixAchNouv { get { return prixachnouv; } set { prixachnouv = value; } }
        private double coefnouv = 0;
        
        public double CoefNouv { get { return coefnouv; } set { coefnouv = value; } }
        private double prixvennouv = 0;
        
        public double PrixVenNouv { get { return prixvennouv; } set { prixvennouv = value; } }
        private DateTime dateapplication = DateTime.MinValue;
        
        public DateTime DateApplication { get { return dateapplication; } set { dateapplication = value; } }
        private double coutstd = 0;
        
        public double CoutStd { get { return coutstd; } set { coutstd = value; } }
        private double qtecomp = 0;
        
        public double QteComp { get { return qtecomp; } set { qtecomp = value; } }
        private double qteoperatoire = 0;
        
        public double QteOperatoire { get { return qteoperatoire; } set { qteoperatoire = value; } }
        private int cono = 0;
        
        public int CoNo { get { return cono; } set { cono = value; } }
        private int prevision = 0;
        
        public int Prevision { get { return prevision; } set { prevision = value; } }

    }
}


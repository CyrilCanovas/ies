
using System;
using System.Collections.Generic;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public partial class DocHeader 
    {
        public int Period; 
        
        public int Devise;
        public double Cours;
        public string NumPayeur;
        public int Expedit;
        public int NbFacture;
        public int BlFact;
        public double TxEscompte;
        public int Reliquat;
        public int Imprim;
        public string Coord01; 
        public string Coord02;
        public string Coord03;
        public string Coord04;
        public int Condition; 
        public int Tarif; 
        public int Colisage;
        public int TypeColis ;
        public int Transaction; 
        public int Langue;
        public double Ecart;
        public int Regime;
        public int NCatCompta;
        public int Ventile;
        public int AbNo;
        public DateTime DebutAbo ;
        public DateTime FinAbo;
        public DateTime DebutPeriod ;
        public DateTime FinPeriod;
        public string CGNum;
        public int Statut;
        private System.TimeSpan heure = System.TimeSpan.Zero;
        public System.TimeSpan Heure { get { return heure; } set { heure = value; } }

        public string Sql_Heure { get {
            return "000000000";
            //"000" + Convert.ToString(heure.Minutes, "00")
            //+ Convert.ToString(heure.Minutes, "00")
            //+ Convert.ToString(heure.Seconds, "00");
            }
            set
            {

                if (value == null) return;
                heure = new TimeSpan(
                        Convert.ToInt32(
                        value.Substring(0, 2)),
                        Convert.ToInt32(value.Substring(0, 2)),
                        Convert.ToInt32(value.Substring(0, 2)));
            }
        }
        public int CANo;
//        public int ReNocaissier ;
        public int Transfere ;
        public int Cloture;
        public string Noweb; 
        public int Attente;
        public int Provenance ;
        public string CANumIFRS; 
        public int MrNo ;
        public int TypeFrais ;
        public double ValFrais; 
        public int TypeLigneFrais ;
        public int TypeFranco;
        public double ValFranco ;
        public int TypeLigneFranco;
        public double Taxe1 ;
        public int TypeTaux1 ;
        public int TypeTaxe1 ;
        public double Taxe2;
        public int TypeTaux2 ;
        public int TypeTaxe2 ;
        public double Taxe3 ;
        public int TypeTaux3;
        public int TypeTaxe3 ;
        public int MajCpta;
        public string Motif;
        
    }
}
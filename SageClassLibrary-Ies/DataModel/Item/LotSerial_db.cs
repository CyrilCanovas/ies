using System;
using System.Collections.Generic;
using System.Text;

namespace SageClassLibrary.DataModel
{
    public partial class LotSerial 
    {
        private DateTime peremption = DateTime.MinValue;
        
        public DateTime Peremption { get { return peremption; } set { peremption = value; } }
        private DateTime fabrication = DateTime.MinValue;
        
        public DateTime Fabrication { get { return fabrication; } set { fabrication = value; } }
        private double qte = 0;
        
        public double Qte { get { return qte; } set { qte = value; } }
        private double qterestant = 0;
        
        public double QteRestant { get { return qterestant; } set { qterestant = value; } }
        private double qteres = 0;
        
        public double QteRes { get { return qteres; } set { qteres = value; } }
        private int dlnoin = 0;
        
        public int DlNoIn { get { return dlnoin; } set { dlnoin = value; } }
        private int dlnoout = 0;
        
        public int DlNoOut { get { return dlnoout; } set { dlnoout = value; } }
        private int mvtstock = 0;
        
        public int MvtStock { get { return mvtstock; } set { mvtstock = value; } }
        private string complement = string.Empty;
        
        public string Complement { get { return complement; } set { complement = value; } }

    }
}

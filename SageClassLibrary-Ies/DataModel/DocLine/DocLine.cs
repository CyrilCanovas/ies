using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Reflection;
using IQToolkit.Extensions;

namespace SageClassLibrary.DataModel
{
    
    public partial class DocLine : SageObject,IClassConverter,IConvertible
    {
        public DocLine()
            : base()
        {
        }

        //public override string ToString()
        //{
        //    return string.Format("N° pièce [{0}] Tiers [{4}] Article [{1}] Qte [{2}] Lot [{3}]", Piece, RefArticle, Qte,NoSerie,Tiers);
        //}
        #region sage business rules
        /*
Si AR_SuiviStock de F_Article = 1 (SERIALISE) Alors
    Suivant la valeur de DO_Type
    Cas où DO_Type est compris dans : {0, 1, 4, 11, 12, 15, 22, 24}
    LS_NoSerie = « »
    LS_Fabrication = Null
    LS_Peremption = Null
    Cas où DO_Type est compris dans : {2, 5, 14, 21} (Série existante)
        Recherche de LS_NoSerie et de LS_Fabrication dans F_LotSerie où F_LotSerie.DE_No =
        F_DocLigne.DE_No et F_LotSerie.AR_Ref = F_DocLigne.AR_Ref et F_LotSerie.LotEpuise
        = 1 (non épuisé)
        LS_NoSerie = F_LotSerie.LS_NoSerie
        LS_Fabrication = F_LotSerie.LS_Fabrication
        LS_Peremption = Null
    Cas où DO_Type est compris dans : {3, 13, 16, 26, 25}
        LS_Fabrication = Formatage de la date système
        LS_NoSerie = Numéro au hasard
        LS_Peremption = Null
    Fin Suivant
Sinon
    Si AR_SuiviStock de F_Article = 5 (Lot) Alors
        Suivant la valeur de DO_Type
        Cas où DO_Type est compris dans : {0, 1, 4, 11, 12, 15, 22, 24}
        LS_NoSerie = « »
        LS_Fabrication = Null
        LS_Peremption = Null
        Cas où DO_Type est compris dans : {2, 5, 14, 21} (Série existante)
        Recherche de LS_NoSerie et de LS_Fabrication dans F_LotSerie ou
        F_LotSerie.DE_No = F_DocLigne.DE_No et F_LotSerie.AR_Ref =
        F_DocLigne.AR_Ref
        LS_NoSerie = F_LotSerie.LS_NoSerie
        LS_Fabrication = F_LotSerie.LS_Fabrication
        LS_Peremption = Null
        Cas où DO_Type est compris dans : {3, 13, 16, 26, 25}
        LS_Fabrication = Formatage de la date système
        LS_NoSerie = Numéro au hasard
        LS_Peremption = Null
        Fin Suivant
        Sinon
            LS_NoSerie = « »
            LS_Fabrication = Null
            LS_Peremption = Null
        Fin Si
Fin Si
*/
        #endregion
        private bool bForceDeNoToZero = false;
        public override void DoBeforeInsert()
        {
            base.DoBeforeInsert();
            bForceDeNoToZero = false;
            DlNo = 0;
            MontantHT = 0;
            MontantTTC = 0;
            if (DateBC != DatePiece) DateBC = DatePiece;
            if (EuQte != Qte) EuQte = Qte;
            //if (QteBC != Qte) QteBC = Qte;

            if (Item == null) return;

            PoidsBrut = Item.GetStdWeight(Item.PoidsBrut)*Qte;
            PoidsNet = Item.GetStdWeight(Item.PoidsNet) *Qte;

            bForceDeNoToZero = Item.SuiviStock == (int) EnumCostingMethod.None;

            if (Item.Escompte == 1)
            {
                FactPoids = 0;
                Escompte = 0;
            }
            
        }
        public override void DoBeforeUpdate()
        {
            base.DoBeforeUpdate();
            if (EuQte != Qte) EuQte = Qte;
            if (Item == null) return;

            PoidsBrut = Item.GetStdWeight(Item.PoidsBrut) * Qte;
            PoidsNet = Item.GetStdWeight(Item.PoidsNet) * Qte;

        }

        private string tiers = string.Empty;
        
        public string Tiers
        {
            get
            {
                if (DocHeader != null) return DocHeader.Tiers;
                else return tiers;
            }
            set
            {

                if (DocHeader != null) return;
                else tiers = value;
            }
        }


        protected virtual int GetDomaine()
        {
            return -1;
        }
        private int domaine;
        public int Domaine
        {
            get {
                if (DocHeader != null) return DocHeader.Domaine;
                else return GetDomaine(); 
                }
            set
            {
                domaine = value;
            }
        }

        private int doctype;
        public int DocType
        {
            get {
                if (DocHeader != null) return DocHeader.DocType;
                return doctype;
                //else {return DocLine.GetDocType(GetType());
                }
            set { doctype = value; }
        }



        public int NoLigne=0;
        
        private string piece = string.Empty;
        public string Piece { 
            get {
                if (DocHeader != null) return DocHeader.Piece;
                else return piece; 
            } 
            set {

                if (DocHeader != null) return ;
                else piece = value; 
            } 
        }
        
        private string reference = string.Empty;
        
        public string Reference { 
            get { 
                
                if (IsComment) return string.Empty;
                if (DocHeader != null && reference == string.Empty) return DocHeader.Reference;
                return reference;
            }
            set
            {
                if (DocHeader != null) return ;
                reference = value;
            } 
        }

        private DateTime datepiece = DateTime.MinValue;
        
        public DateTime DatePiece { 
            get {
                //if (IsComment) return DateTime.MinValue;
                if (DocHeader != null) return DocHeader.DatePiece;
                return datepiece; 
            } 
            set {
                if (DocHeader == null) datepiece = value; 
            } 
        }

        private DateTime datelivr = DateTime.MinValue;
        
        public DateTime DateLivr { get {
            if (IsComment) return DateTime.MinValue; 
            if (datelivr==DateTime.MinValue)
            {
                if (DocHeader != null) return DocHeader.DateLivr;
            }
            return datelivr; 
        } 

            set { datelivr = value; } 
        }
        
        
        private string canum = string.Empty;
        
        public string CaNum { get {
            if (IsComment) return string.Empty;
            if (canum == string.Empty)
            {
                if (DocHeader != null && canum== string.Empty) return DocHeader.CaNum;
            }
            return canum; 
        } 
            set 
            { canum = value; } 
        }

        private int cono = 0;
        
        public int CoNo { get {
            if (IsComment) return 0;

            if (cono == 0)
            {
                if (DocHeader != null && cono == 0) return DocHeader.CoNo;
            }
            return cono;

            }
            set
            {
                cono = value; 
        
            } 
        }

        private int deno = 0;
        
        public int DeNo { 
            get {
                if (IsComment || bForceDeNoToZero) return 0;
                return GetDeNo();
            } 
            set { 
                deno = value; 
            } 
        }

        protected virtual int GetDeNo()
        {
            if (DocHeader != null) return DocHeader.DeNo;
            return deno; 
        }

        
        public int TRemPied;
        public int DlNo;

        [IgnoreDataMember]
        public DocHeader DocHeader
        {
            get;
            set;
        }

        private string designation = string.Empty;
        public string Designation { 
            get {
                if (designation != string.Empty) return designation;
                if (Item == null) return designation;
                return Item.Design;
            } 
            set { 
                designation = value; 
            } 
        }

        private string euenumere = string.Empty;

        public string EuEnumere { 
            get {
                if (euenumere != string.Empty) return euenumere;
                if (Item == null) return euenumere;
                return Item.EuEnumere;
            } 
            set { euenumere = value; } }
        [IgnoreDataMember]
        public Item Item
        {
            get;
            set;
        }

        //[IgnoreDataMember]
        //public Memo Memo
        //{
        //    get;
        //    set;
        //}

        //private int glno = 0;

        //public int GlNo { 
        //    get { 
        //        if (Memo==null) return glno;
        //        if (Memo.GlNo == 0) return glno;
        //        return Memo.GlNo;
        //    } 
        //    set { glno = value; } 
        //}


        private string refarticle =string.Empty;
        public string RefArticle
        {
            get
            {
                if (Item != null) return Item.RefArticle;
                return refarticle;
            }
            set
            {
                if (Item == null) refarticle = value;
            }
        }
        public bool IsComment
        {
            get
            {
                return ((RefArticle ??string.Empty)== string.Empty);
            }
 
        }

        public object Copy()
        {

            return Copy(this.GetType());
        }

        public object Copy(Type targetType)
        {
            DocLine obj = SageClassLibrary.Common.Tools.Copy2(this, targetType) as DocLine;
            //(obj as DocLine).Item = this.Item;
            if (targetType==this.GetType())
            //(obj as DocLine).DocHeader = this.DocHeader;
                (obj as ICustomDbOwner).Attach((this as ICustomDbOwner).DbOwner);
            return obj;
        }
        #region IClassConverter Membres

        public object ConvertClass()
        {
            return
                Convert.ChangeType(this,
                    SageObjectConstants.GetTypeFromDocType(this.DocType, this.GetType()));
        }

        #endregion

        #region IConvertible Membres

        public TypeCode GetTypeCode()
        {
            return this.GetTypeCode();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        public int ToInt32(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        public string ToString(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        //public static T Cast<T>(object o)
        //{
        //    return (T)o;
        //}
        //MethodInfo castMethod = this.GetType().GetMethod("Cast").MakeGenericMethod(conversionType);
        //object castedObject = castMethod.Invoke(null, new object[] { this });


        public object ToType(Type conversionType, IFormatProvider provider)
        {

            return SageClassLibrary.Common.Tools.Copy(this, conversionType);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Linq;
using System.Reflection;
using IQToolkit.Extensions;

namespace SageClassLibrary.DataModel
{
    
    public partial class DocHeader : SageObject,IClassConverter,IConvertible
    {
        //public override string ToString()
        //{
        //    return string.Format("N° pièce [{0}] Tiers [{1}] Référence [{2}]", Piece,  Tiers,Reference);
        //}


        public override void DoBeforeInsert()
        {
            base.DoBeforeInsert();

            if (string.IsNullOrEmpty(Piece)) Piece = DocNumManager.GetNextNum(this);
            if (DatePiece <= DateTime.MinValue) DatePiece = DateTime.Today;
            Heure = DateTime.Now.TimeOfDay;
        }

        public DocHeader():base()
        {
           
        }
        public string GetNextDocNum()
        {
            throw new NotImplementedException();
            //return DocNumManager.GetNextNum(this);
        }
        
        public void Add(DocLine docline)
        {
            if (Lines == null) Lines = new List<DocLine>();

            if (docline.NoLigne == 0)
            {
                int maxnoligne =0;
                if (Lines.Count > 0) maxnoligne = Lines.Max(x => x.NoLigne);
                docline.NoLigne = maxnoligne + 10000;
            }
            docline.DocHeader = this;
            Lines.Add(docline);
        }


        protected virtual int GetDomaine()
        {
            return -1;
        }

        private int domaine;
        public int Domaine
        {
            get { return GetDomaine(); }
            set {
                domaine = value;
            }
        }

        protected int intdoctype = -1;
        [IgnoreDataMember]
        public int DocType
        {
            get {
//                if (intdoctype != -1) return intdoctype;
//                  else 
                return SageObjectConstants.GetDocType(this.GetType());
            }
            set { intdoctype = value; } 
        }

        public string Piece;



        public DateTime DatePiece;
        public string Reference;

        private string tiers = string.Empty;

        protected virtual string GetTiers()
        {
            return tiers;
        }
        public string Tiers { 
            get {
                return GetTiers();
            } 
            set { tiers = value; } 
        }
       
        
        public int CoNo;
        private int deno = 0;
        
        public int DeNo { get { return deno; } set { deno = value; } }
        
        
        public int LiNo;

        
        public string CaNum;
        private int souche;
        
        public int Souche { get { return souche; } set { souche = value; } }
        
        
        public DateTime DateLivr;



        //protected SageObjectList<DocLine> lines = null;
        [IgnoreDataMember]
        public List<DocLine> Lines;

        public DocLine CreateNewDocLine()
        {
            DocLine docline = Activator.CreateInstance(GetLineType()) as DocLine;
            return docline;
        }

        public Type GetLineType()
        {
            return SageObjectConstants.GetLinkedType(this.GetType());
        }
       
        public void SubCopy(Type type)
        {
                
        }
        public DocHeader Copy(
            Type targetType,
            bool ignoreLines)
        {
            var docheader = SageClassLibrary.Common.Tools.Copy2(this, targetType) as DocHeader;
            //docheader.Lines = new List<DocLine>();

            (docheader as ICustomDbOwner).Attach((this as ICustomDbOwner).DbOwner);

            if (ignoreLines) docheader.Lines = null;
            if (!ignoreLines && Lines!=null)
            {
                docheader.Lines = new List<DocLine>();
                foreach (var docline in Lines)
                {
                    DocLine newdocline = docline.Copy(docheader.GetLineType()) as DocLine;
                    newdocline.DocHeader = docheader;
                    docheader.Lines.Add(newdocline);
                }
            
            }
            
            
            return docheader;
        }
        //private int GetNoLigne()
        //{
        //    int value = 0;
        //    if (Lines != null) if (Lines.Count>0) value = Lines.Max(x => x.NoLigne);
        //    value += 10000;
        //    return value;
        //}

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
            object obj = SageClassLibrary.Common.Tools.Copy(this, conversionType);


            return obj;

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

        #region IClassConverter Membres

        public object ConvertClass()
        {
            return 
                

                Convert.ChangeType(this,
                SageObjectConstants.GetTypeFromDocType(intdoctype, this.GetType()));
        }

        #endregion
    }
}

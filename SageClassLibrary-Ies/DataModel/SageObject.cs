using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using IQToolkit.Extensions;
using IQToolkit.Data;
using System.Runtime.Serialization;
using System.Xml;

namespace SageClassLibrary.DataModel
{

    public abstract class SageObject :ICustomDbOwner
    {
        public static double CalcDiscount(double qte,double unitprice,int discounttype,double discountvalue)
        {
            double result;
            switch ((EnumDiscountType)discounttype)
            {
                case EnumDiscountType.Qty:
                    result = (unitprice - discountvalue) * qte;
                    break;
                case EnumDiscountType.Percent:
                    result = (1 - discountvalue / 100) * qte;
                    break;
                default:
                    result = unitprice*qte-discountvalue;
                    break;
            }
            return result;

        }

        public static DateTime SageMinDate = new DateTime(1900, 01, 01);

        //public void Copy(SageObject sageobject)
        //{
        //    throw new NotImplementedException();
        //    //var qsrc = from i in sageobject.GetType().GetProperties().ToDictionary(x=>x.Name);
        //    //var qdst = from i in this.GetType().GetProperties().ToDictionary(x=>x.Name);

        //    //foreach (string propertyname in src.Keys)
        //    //{
        //    //    PropertyInfo dstproperty = null;
        //    //    if (!dst.TryGetValue(fieldname, out dstproperty)) continue;
        //    //    if (!dstproperty.CanWrite) continue;
        //    //    dstproperty.SetValue(this, src[fieldname].GetValue(sageobject, null), null);

        //    //}
        //}

        public static string Serialize(SageObject sageobject)
        {
            var result = new StringBuilder();
            XmlWriterSettings xmlwritersettings = new XmlWriterSettings() { ConformanceLevel = System.Xml.ConformanceLevel.Fragment, OmitXmlDeclaration = true };
            DataContractSerializer ds = new DataContractSerializer(sageobject.GetType());
            using (XmlWriter s = XmlWriter.Create(result, xmlwritersettings))
            {
                ds.WriteObject(s, sageobject);

            }
            return result.ToString();
        }

        private DbEntityProvider dbowner;
        #region IDbOwner Membres
        DbEntityProvider ICustomDbOwner.DbOwner
        {
            get{return dbowner;}
        }

        void ICustomDbOwner.Attach(DbEntityProvider dbprovider)
        {
            dbowner = dbprovider;
        }

        void ICustomDbOwner.Detach()
        {
            dbowner = null;
        }

        #endregion

        public override string ToString()
        {
            return SageObject.Serialize(this);
        }

        public virtual void DoBeforeInsert()
        {

        }
        public virtual void DoBeforeDelete()
        {

        }
        public virtual void DoBeforeUpdate()
        {

        }

    }
}

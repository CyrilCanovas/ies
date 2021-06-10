using System;
using System.Collections.Generic;
using System.Text;
using IQToolkit;
using System.Linq;
using IQToolkit.Data;
using System.Runtime.Serialization;

namespace SageClassLibrary.DataModel
{



    public partial class Item:SageObject
    {
        private static double[] WeightConv = new double[] { 1000000, 100000, 1000, 1, 0.001 };
        private string refarticle = string.Empty;
        public string RefArticle { get { return refarticle; } set { refarticle = value; } }

        //public List<LotSerial> GetLotSerialList(string noserie, int deno)
        //{
        //    var q =
        //        from i in
        //            (
        //            SageDataContext.GetUpdatableTable<LotSerial>(

        //            (this as IDbOwner).DbOwner))
        //        where i.DeNo == deno && i.NoSerie == noserie && i.RefArticle == this.RefArticle
        //        select i;

        //    return q.ToList();
        //}

        //public List<LotSerial> GetLotSerialList(string noserie, Warehouse warehouse)
        //{
        //    return GetLotSerialList(noserie, warehouse.DeNo);
        //}

        //public override string ToString()
        //{
        //    return refarticle.ToString();
        //}

        [IgnoreDataMember]
        public Memo Memo
        {
            get;
            set;
        }
        //private string euenumere;
        public string EuEnumere{
            get{
                if (SalesUnit != null) return SalesUnit.UniteVen;
                else return String.Empty;
                
                //return SageObjectsLib.SalesUnitList<SageObjectsLib.SalesUnit>.Get(UniteVen);
            }

        }
        public SalesUnit SalesUnit;

        public static double GetStdWeight(int unitepoids, double weight)
        {
            if (unitepoids < 0) return 0;
            if (unitepoids >= WeightConv.Length) return 0;
            return WeightConv[unitepoids] * weight;
        }

        public double GetStdWeight(double weight)
        {
            return Item.GetStdWeight(this.UnitePoids, weight);
        }

    }
}

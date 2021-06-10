using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Runtime.Serialization;
using System.IO;
using System.Reflection;



namespace SageClassLibrary.DataModel
{

    public class RepositoryClassDefinition
    {
        private static RepositoryClassDefinition current =null;
        public static RepositoryClassDefinition Current {
         get{
             if (current==null) {

                current= new RepositoryClassDefinition();
             }
             return current;
         }
        }

        private Dictionary<string, SageTableDefinition> sagetabledefinitions ; //new Dictionary<string, SageTableDefinition>();
        //public Dictionary<string, SageTableDefinition> SageTableDefinitions { 
        //    get { return sagetabledefinitions; }
        //    set { sagetabledefinitions = value; }
        //}
        public RepositoryClassDefinition()
        {
            intLoadSageTableDefinition();
        }
    
        public SageTableDefinition GetByClassId(Type type)
        {
            return sagetabledefinitions[type.Name];
        }
        public MemoryStream Serialize()
        {
            List<SageTableDefinition> values = sagetabledefinitions.Values.ToList().Where(x => x.Parent == null).ToList();
            return Serialize(typeof(List<SageTableDefinition>), values);
        }

        public static Dictionary<String, SageTableDefinition> GetTableDefitionsFromStream(Stream stream)
        {
            
            var oo = RepositoryClassDefinition.DeSerialize(stream, typeof(List<SageTableDefinition>)) as List<SageTableDefinition>;
            var result = SageTableDefinition.Walk(oo).ToDictionary(x => x.ClassId);
            return result;
        }
        
        public void Merge(Dictionary<String, SageTableDefinition> values)
        {
            var q = from i in values.Keys
                    join a in sagetabledefinitions.Keys on i equals a
                    select new { Source = sagetabledefinitions[i], Update = values[i] };

            foreach (var o in q)
            {
                foreach (var p in o.Update.SageMemberInfos)
                    o.Source.SageMemberInfos.Add(p.Key,p.Value);
            }

            var q2 = from i in values.Keys
                    where !sagetabledefinitions.Keys.Contains(i)
                    select values[i];

            foreach (var o in q2)
                sagetabledefinitions.Add(o.ClassId, o);
        }

        private void intLoadSageTableDefinition()
        {
     //       Assembly a = Assembly.GetExecutingAssembly();
            Assembly a = Assembly.GetAssembly(this.GetType());
            
            var q = from i in a.GetManifestResourceNames()
                       where i.Contains("DataModel.SageTableDefinitions.xml")
                       select i;
            var qres = q.FirstOrDefault();
            if (qres!=null)
            using (var resstream = a.GetManifestResourceStream(qres))
            {
                sagetabledefinitions = GetTableDefitionsFromStream(resstream);
            }
            
            if (sagetabledefinitions == null) return;
            var q2 = from i in a.GetManifestResourceNames()
                    where i.Contains("Customization.SageTableDefinitions.xml")
                    select i;
            var qres2 = q2.FirstOrDefault();
            if (qres2 != null)
                using (var resstream = a.GetManifestResourceStream(qres2))
                {
                    Merge(GetTableDefitionsFromStream(resstream));
                }

        }
        
        
        public static object DeSerialize(Stream stream, Type type)
        {
            object result = null;
            DataContractSerializer ser = new DataContractSerializer(type);
            result = ser.ReadObject(stream);
            return result;
        }


        public static MemoryStream Serialize(Type type, object obj)
        {
            MemoryStream result = new MemoryStream();
            var s = new DataContractSerializer(type);
            s.WriteObject(result, obj);
            return result;
        }
    }
    

    [DataContract]
    public class  SageTableDefinition
    {


        public static IEnumerable<SageTableDefinition> Walk(IEnumerable<SageTableDefinition> values)
        {
            foreach (var b in values)
            {
                yield return b;
                if (b.Childs == null) continue;
                foreach (var i in SageTableDefinition.Walk(b.Childs))
                {
                    yield return i;
                }
            }
        }
        public SageTableDefinition()
        {
            SageMemberInfos = new Dictionary<string, SageMemberInfo>();
        }
        public SageTableDefinition(string tableid,string classid):this()
        {
            if (tableid!=null) TableID = tableid;
            ClassId = classid;
        }
        [OnSerializing]
        void OnSerializing(StreamingContext context)
        {
            sagememberinfos = SageMemberInfos.Values.ToList();        
        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            
            if (sagememberinfos != null)
                SageMemberInfos = sagememberinfos.ToDictionary(x => x.MemberName);
            else SageMemberInfos = new Dictionary<string, SageMemberInfo>();

            if (Childs != null)
            {
                foreach (var i in Childs) i.SetParent(this);    
            }
        }
        protected void SetParent(SageTableDefinition parent)
        {
            if (parent == this) this.parent = null;
            else this.parent = parent;
        }

        [DataMember]
        private string tableid;

        [IgnoreDataMember]
        public string TableID
        {
            get
            {
                if (tableid != null) return tableid;
                return Parent.TableID;
            }
            set
            {
                tableid = value;
            }
        }

        
        private SageTableDefinition parent;
        
        [IgnoreDataMember]
        public SageTableDefinition Parent
        {
            get
            {
                return parent;
            }
            set
            {
                if (parent != null) parent.Childs.Remove(this);
                if (value != null)
                {
                    if (value!=this) value.Childs.Add(this);
                }
                if (this == value) parent = null;
                else parent = value;
                
            }
        }

        [DataMember(Name = "SageTableDefinitions")]
        protected List<SageTableDefinition> Childs = new List<SageTableDefinition>();
        
        [DataMember(Name="SageMemberInfos")]
        private List<SageMemberInfo> sagememberinfos;

        [IgnoreDataMember]
        public Dictionary<string, SageMemberInfo> SageMemberInfos
        {
            get;
            set;
        }

        public SageMemberInfo GetByName(string membername)
        {
            SageMemberInfo result =null;
            if (SageMemberInfos.TryGetValue(membername,out result)) return result;
            if (Parent!=null) return Parent.GetByName(membername);
            return result;
            
        }
        
        [DataMember]
        public string ClassId
        {
            get;
            set;
        }

    }
}

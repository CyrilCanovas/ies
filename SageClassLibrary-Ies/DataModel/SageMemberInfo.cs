using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Runtime.Serialization;
using System.Xml;
using System.ComponentModel;

namespace SageClassLibrary.DataModel
{
    [DataContract]
    public enum EnumAccessibility
    {
        [EnumMember]
        Both,
        [EnumMember]
        OnlySQL,
        [EnumMember]
        OnlyOdbc
    }
    [DataContract]
    public class SageMemberInfo
    {

        [DataMember]
        public string MemberName
        {
            get;
            set;
        }

        [DataMember]
        public string FieldName
        {
            get;
            set;
        }
        [DataMember]
        public  bool NotNull { get; set; }
        
        [DataMember]
        public bool CanBeInserted { get; set; }
        
        [DataMember]
        public bool CanBeUpdated  { get; set; }
        [DataMember]
        public bool CanBeSelected { get; set; }
        [DataMember]
        public bool IsIndexed { get; set; }
        [DataMember]
        public bool PrimaryKey { get; set; }
        [DataMember]
        public object DefaultValue
        {
            get;
            set;
        }

        [DataMember]
        public EnumAccessibility Accessibility { get; set; }


        [OnSerializing]
        void OnSerializing(StreamingContext context)
        {
                odbctype = Enum.GetName(typeof(OdbcType), OdbcType);
                if (odbctype == null) odbctype = OdbcType.VarChar.ToString();
            
        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {

            OdbcType = (OdbcType)
                Enum.Parse(typeof(OdbcType), odbctype);
        }

        [DataMember(Name = "OdbcType")]
        private string odbctype;
        
        
        [IgnoreDataMember]
        public OdbcType OdbcType
        {
            get;
            set;
        }

        [DataMember]
        [DefaultValue(69)]
        public int Length
        {
            get;
            set;
        }
    }
}

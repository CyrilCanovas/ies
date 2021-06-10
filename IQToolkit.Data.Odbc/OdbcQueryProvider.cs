using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Data.Common;

namespace IQToolkit.Data.Odbc
{
    using IQToolkit.Data.Common;
 
    using IQToolkit.Extensions;


    public class OdbcQueryProvider : DbEntityProvider
    {


        public OdbcQueryProvider(OdbcConnection connection, QueryMapping mapping, QueryPolicy policy)
            : base(connection, OdbcSqlLanguage.Default,
            mapping, policy)
        {
            KeepConnectionAlive = true;
        }


        //public override DbTransaction Transaction
        //{
        //    get { return null; }
        //    set
        //    {

        //    }
        //}
        public static OdbcType GetOdbcType(QueryType type)
        {
            return ToOdbcType((type as OdbcQueryType).DbType);
        }

        public static OdbcType ToOdbcType(DbType type)
        {
            switch (type)
            {
                case DbType.AnsiString:
                    return OdbcType.VarChar;
                case DbType.AnsiStringFixedLength:
                    return OdbcType.Char;
                case DbType.Binary:
                    return OdbcType.Binary;
                case DbType.Boolean:
                    return OdbcType.Bit;
                //case DbType.Byte:
                //    return OdbcType.UnsignedTinyInt;
                //case DbType.Currency:
                //    return OdbcType.;
                case DbType.Date:
                    return OdbcType.Date;
                case DbType.DateTime:
                    return OdbcType.DateTime;
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                    return OdbcType.Timestamp;
                case DbType.Decimal:
                    return OdbcType.Decimal;
                case DbType.Double:
                    return OdbcType.Double;
                case DbType.Guid:
                    return OdbcType.UniqueIdentifier;
                case DbType.Int16:
                    return OdbcType.SmallInt;
                case DbType.Int32:
                    return OdbcType.Int;
                case DbType.Int64:
                    return OdbcType.BigInt;
                //case DbType.Object:
                //    return OdbcType.;
                case DbType.SByte:
                    return OdbcType.TinyInt;
                case DbType.Single:
                    return OdbcType.Real;
                case DbType.String:
                    return OdbcType.VarChar;
                case DbType.StringFixedLength:
                    return OdbcType.NChar;
                case DbType.Time:
                    return OdbcType.Time;
                case DbType.UInt16:
                    return OdbcType.SmallInt;
                case DbType.UInt32:
                    return OdbcType.Int;
                case DbType.UInt64:
                    return OdbcType.BigInt;
                case DbType.VarNumeric:
                    return OdbcType.Numeric;
                case DbType.Xml:
                    return OdbcType.Text;
                default:
                    throw new InvalidOperationException(string.Format("Unhandled db type '{0}'.", type));
            }
        }
        
        protected override QueryExecutor CreateExecutor()
        {
            return new Executor(this);
        }

        new class Executor : DbEntityProvider.Executor
        {
            public Executor(OdbcQueryProvider provider)
                : base(provider)
            {
            }
            protected override IEnumerable<T> Project<T>(DbDataReader reader, Func<FieldReader, T> fnProjector, MappingEntity entity, bool closeReader)
            {
                
                var freader = new DbFieldReader(this, reader);
                try
                {
                    while (reader.Read())
                    {
                        T value = fnProjector(freader);
                        IClassConverter iclassconverter = value as IClassConverter;
                        if (iclassconverter != null)
                            value = (T)iclassconverter.ConvertClass();
                        ICustomDbOwner idbowner = value as ICustomDbOwner;
                        if (idbowner != null) idbowner.Attach(this.Provider);

                        yield return value;
                    }
                }
                finally
                {
                    if (closeReader)
                    {
                        reader.Close();
                    }
                }
            }
            public static DateTime OdbcMinDate = new DateTime(1900, 01, 01);
            protected override void AddParameter(DbCommand command, QueryParameter parameter, object value)
            {
                QueryType qt = parameter.QueryType;
                if (qt == null)
                    qt = (DbQueryType)this.Provider.Language.TypeSystem.GetColumnType(parameter.Type);
                var odbctype = OdbcQueryProvider.GetOdbcType(qt);
                //var p = ((OdbcCommand)command).Parameters.Add(parameter.Name, odbctype, qt.Length);
                 OdbcParameter p;
                // if (odbctype == OdbcType.Time) p = ((OdbcCommand)command).Parameters.Add(parameter.Name, OdbcType.DateTime);
                //else  
                p = ((OdbcCommand)command).Parameters.Add(parameter.Name, odbctype,qt.Length);
                if (qt.Precision != 0)
                    p.Precision = (byte)qt.Precision;
                if (qt.Scale != 0)
                    p.Scale = (byte)qt.Scale;
                switch (odbctype)
                {
                    //case OdbcType.Time:
                    //    p.Value = DateTime.Now;
                    //    break;

                    case OdbcType.Date:
                    case OdbcType.DateTime:
                    case OdbcType.SmallDateTime:
                        if (value != null)
                        {
                            var dvalue = System.Convert.ToDateTime(value);
                            if (dvalue == null) p.Value = DBNull.Value;
                            else if (dvalue <= OdbcMinDate) p.Value = DBNull.Value;
                            else p.Value = value;
                        }
                        else p.Value =DBNull.Value;
                        break;
                    
                    default:
                        
                        p.Value = value ?? DBNull.Value;
                        break;
                        
                }

                //p.Value = value ?? DBNull.Value;
            }
        }

      
    }
}

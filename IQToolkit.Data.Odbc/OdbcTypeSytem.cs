using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQToolkit.Data.Common;
using System.Data.Odbc;
using System.Data;

namespace IQToolkit.Data.Odbc
{
    public class OdbcTypeSytem:QueryTypeSystem
    {
        public override QueryType Parse(string typeDeclaration)
        {
            string[] args = null;
            string typeName = null;
            string remainder = null;
            int openParen = typeDeclaration.IndexOf('(');
            if (openParen >= 0)
            {
                typeName = typeDeclaration.Substring(0, openParen).Trim();

                int closeParen = typeDeclaration.IndexOf(')', openParen);
                if (closeParen < openParen) closeParen = typeDeclaration.Length;

                string argstr = typeDeclaration.Substring(openParen + 1, closeParen - (openParen + 2));
                args = argstr.Split(',');
                remainder = typeDeclaration.Substring(closeParen + 1);
            }
            else
            {
                int space = typeDeclaration.IndexOf(' ');
                if (space >= 0)
                {
                    typeName = typeDeclaration.Substring(0, space);
                    remainder = typeDeclaration.Substring(space + 1).Trim();
                }
                else
                {
                    typeName = typeDeclaration;
                }
            }

            if (String.Compare(typeName, "rowversion", StringComparison.OrdinalIgnoreCase) == 0)
            {
                typeName = "Timestamp";
            }

            if (String.Compare(typeName, "numeric", StringComparison.OrdinalIgnoreCase) == 0)
            {
                typeName = "Decimal";
            }

            if (String.Compare(typeName, "sql_variant", StringComparison.OrdinalIgnoreCase) == 0)
            {
                typeName = "Variant";
            }

            OdbcType dbType = (OdbcType)Enum.Parse(typeof(OdbcType), typeName, true);

            bool isNotNull = (remainder != null) ? remainder.ToUpper().Contains("NOT NULL") : false;


            int length = 0;
            short precision = 0;
            short scale = 0;

            switch (dbType)
            {
                case OdbcType.Binary:
                case OdbcType.Char:
                case OdbcType.Image:
                case OdbcType.NChar:
                case OdbcType.NVarChar:
                case OdbcType.VarBinary:
                case OdbcType.VarChar:
                    if (args == null || args.Length < 1)
                    {
                        length = 80;
                    }
                    else if (string.Compare(args[1], "max", true) == 0)
                    {
                        length = Int32.MaxValue;
                    }
                    else
                    {
                        length = Int32.Parse(args[1]);
                    }
                    break;
                //case OdbcType.Money:
                //    if (args == null || args.Length < 1)
                //    {
                //        precision = 29;
                //    }
                //    else
                //    {
                //        precision = Int16.Parse(args[0]);
                //    }
                //    if (args == null || args.Length < 2)
                //    {
                //        scale = 4;
                //    }
                //    else
                //    {
                //        scale = Int16.Parse(args[1]);
                //    }
                //    break;
                case OdbcType.Decimal:
                    if (args == null || args.Length < 1)
                    {
                        precision = 29;
                    }
                    else
                    {
                        precision = Int16.Parse(args[0]);
                    }
                    if (args == null || args.Length < 2)
                    {
                        scale = 0;
                    }
                    else
                    {
                        scale = Int16.Parse(args[1]);
                    }
                    break;
                //case OdbcType.Float:
                case OdbcType.Real:
                    if (args == null || args.Length < 1)
                    {
                        precision = 29;
                    }
                    else
                    {
                        precision = Int16.Parse(args[0]);
                    }
                    break;
            }

            return new OdbcQueryType(dbType, isNotNull, length, precision, scale);
        }

        public override QueryType GetColumnType(Type type)
        {
            bool isNotNull = type.IsValueType && !TypeHelper.IsNullableType(type);
            type = TypeHelper.GetNonNullableType(type);
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return new OdbcQueryType(OdbcType.Bit, isNotNull, 0, 0, 0);
                case TypeCode.SByte:
                case TypeCode.Byte:
                    return new OdbcQueryType(OdbcType.TinyInt, isNotNull, 0, 0, 0);
                case TypeCode.Int16:
                case TypeCode.UInt16:
                    return new OdbcQueryType(OdbcType.SmallInt, isNotNull, 0, 0, 0);
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    return new OdbcQueryType(OdbcType.Int, isNotNull, 0, 0, 0);
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return new OdbcQueryType(OdbcType.BigInt, isNotNull, 0, 0, 0);
                case TypeCode.Single:
                case TypeCode.Double:
                    return new OdbcQueryType(OdbcType.Double, isNotNull, 0, 0, 0);
                case TypeCode.String:
                    return new OdbcQueryType(OdbcType.NVarChar, isNotNull, 2000, 0, 0);
                case TypeCode.Char:
                    return new OdbcQueryType(OdbcType.NChar, isNotNull, 1, 0, 0);
                case TypeCode.DateTime:
                    return new OdbcQueryType(OdbcType.DateTime, isNotNull, 0, 0, 0);
                default:
                    if (type == typeof(byte[]))
                        return new OdbcQueryType(OdbcType.NVarChar, isNotNull, 2000, 0, 0);
                    else if (type == typeof(Guid))
                        return new OdbcQueryType(OdbcType.UniqueIdentifier, isNotNull, 0, 0, 0);
                    else if (type == typeof(DateTimeOffset))
                        return new OdbcQueryType(OdbcType.DateTime, isNotNull, 0, 0, 0);
                    else if (type == typeof(TimeSpan))
                        return new OdbcQueryType(OdbcType.Time, isNotNull, 0, 0, 0);
                    else if (type == typeof(decimal))
                        return new OdbcQueryType(OdbcType.Decimal, isNotNull, 0, 29, 4);
                    return null;
            }
        }
        public override string GetVariableDeclaration(QueryType type, bool suppressSize)
        {
            var sqlType = (OdbcQueryType)type;
            StringBuilder sb = new StringBuilder();
            sb.Append(sqlType.OdbcType.ToString().ToUpper());
            if (sqlType.Length > 0 && !suppressSize)
            {
                if (sqlType.Length == Int32.MaxValue)
                {
                    sb.Append("(max)");
                }
                else
                {
                    sb.AppendFormat("({0})", sqlType.Length);
                }
            }
            else if (sqlType.Precision != 0)
            {
                if (sqlType.Scale != 0)
                {
                    sb.AppendFormat("({0},{1})", sqlType.Precision, sqlType.Scale);
                }
                else
                {
                    sb.AppendFormat("({0})", sqlType.Precision);
                }
            }
            return sb.ToString();
        }

        public static DbType GetDbType(OdbcType dbType)
        {
            switch (dbType)
            {
                case OdbcType.BigInt:
                    return DbType.Int64;
                case OdbcType.Binary:
                    return DbType.Binary;
                case OdbcType.Bit:
                    return DbType.Boolean;
                case OdbcType.Char:
                    return DbType.AnsiStringFixedLength;
                case OdbcType.Date:
                    return DbType.Date;
                case OdbcType.DateTime:
                case OdbcType.SmallDateTime:
                    return DbType.DateTime;
                //case OdbcType.DateTime2:
                //    return DbType.DateTime2;
                //case OdbcType.DateTimeOffset:
                //    return DbType.DateTimeOffset;
                case OdbcType.Decimal:
                    return DbType.Decimal;

                case OdbcType.Double:
                case OdbcType.Real:
                    return DbType.Double;
                case OdbcType.Image:
                    return DbType.Binary;
                case OdbcType.Int:
                    return DbType.Int32;
                //case OdbcType.Money:
                //case OdbcType.SmallMoney:
                //    return DbType.Currency;

                case OdbcType.NChar:
                    return DbType.StringFixedLength;
                case OdbcType.NText:
                case OdbcType.NVarChar:
                    return DbType.String;
                case OdbcType.SmallInt:
                    return DbType.Int16;
                case OdbcType.Text:
                    return DbType.AnsiString;
                case OdbcType.Time:
                    return DbType.Time;
                case OdbcType.Timestamp:
                    return DbType.Binary;
                case OdbcType.TinyInt:
                    return DbType.SByte;
                //case OdbcType.Udt:
                //    return DbType.Object;
                case OdbcType.UniqueIdentifier:
                    return DbType.Guid;
                case OdbcType.VarBinary:
                    return DbType.Binary;
                case OdbcType.VarChar:
                    return DbType.AnsiString;
                //case OdbcType.Variant:
                //    return DbType.Object;
                //case OdbcType.Xml:
                //    return DbType.String;
                default:
                    throw new InvalidOperationException(string.Format("Unhandled sql type: {0}", dbType));
            }
        }
    }
}

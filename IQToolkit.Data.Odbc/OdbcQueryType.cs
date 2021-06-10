using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQToolkit.Data.Common;
using System.Data.Odbc;
using System.Data;

namespace IQToolkit.Data.Odbc
{
    public class OdbcQueryType : QueryType
    {
        OdbcType dbType;
        bool notNull;
        int length;
        short precision;
        short scale;

        public OdbcQueryType(OdbcType dbType, bool notNull, int length, short precision, short scale)
        {
            this.dbType = dbType;
            this.notNull = notNull;
            this.length = length;
            this.precision = precision;
            this.scale = scale;
        }

        public virtual DbType DbType
        {
            get
            {
                return OdbcTypeSytem.GetDbType(this.dbType);
            }
        }

        public OdbcType OdbcType
        {
            get { return this.dbType; }
        }

        public override int Length
        {
            get { return this.length; }
        }

        public override bool NotNull
        {
            get { return this.notNull; }
        }

        public override short Precision
        {
            get { return this.precision; }
        }

        public override short Scale
        {
            get { return this.scale; }
        }
    }
}

// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace IQToolkit.Data.Odbc
{
    using IQToolkit.Data.Common;

    /// <summary>
    /// TSQL specific QueryLanguage
    /// </summary>
    public class OdbcSqlLanguage : QueryLanguage
    {
        OdbcTypeSytem typeSystem = new OdbcTypeSytem();

        public override QueryTypeSystem TypeSystem
        {
            get { return this.typeSystem; }
        }

        public override string Quote(string name)
        {
            return name;
            //if (name.StartsWith("[") && name.EndsWith("]"))
            //{
            //    return name;
            //}
            //else 
            //{
            //    return "[" + name + "]";
            //}
        }

        public override Expression GetGeneratedIdExpression(MemberInfo member)
        {
            return new FunctionExpression(TypeHelper.GetMemberType(member), "@@IDENTITY", null);
        }


        public override QueryLinguist CreateLinguist(QueryTranslator translator)
        {
            return new OdbcLinguist(this, translator);
        }

        class OdbcLinguist : QueryLinguist
        {
            public OdbcLinguist(OdbcSqlLanguage language, QueryTranslator translator)
                : base(language, translator)
            {
                
            }

            public override Expression Translate(Expression expression)
            {
                #region ancien pipeline de traitement
                // fix up any order-by's
                //expression = OrderByRewriter.Rewrite(this.Language, expression);

                //expression = base.Translate(expression);

                //expression = CrossJoinIsolator.Isolate(expression);
                //expression = SkipToNestedOrderByRewriter.Rewrite(this.Language, expression);
                //expression = OrderByRewriter.Rewrite(this.Language, expression);
                //expression = UnusedColumnRemover.Remove(expression);
                //expression = RedundantSubqueryRemover.Remove(expression);
                #endregion
                // fix up any order-by's
                expression = OrderByRewriter.Rewrite(this.Language, expression);

                expression = base.Translate(expression);

                //expression = SkipToNestedOrderByRewriter.Rewrite(expression);
                expression = UnusedColumnRemover.Remove(expression);

                return expression;
            }

            public override string Format(Expression expression)
            {
                return OdbcSqlFormatter.Format(expression);
            }
        }
        private static OdbcSqlLanguage _default;

        public static OdbcSqlLanguage Default
        {
            get
            {
                if (_default == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _default, new OdbcSqlLanguage(), null);
                }
                return _default;
            }
        }
    }
}
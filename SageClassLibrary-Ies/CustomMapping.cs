using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQToolkit.Data;
using SageClassLibrary.DataModel;
using System.Reflection;
using System.Linq.Expressions;
using IQToolkit.Data.Common;
using IQToolkit.Data.Odbc;
using IQToolkit.Data.SqlClient;
using IQToolkit;
using System.Data.Odbc;

namespace SageClassLibrary
{
    //public class OdbcPolicy : QueryPolicy
    //{
    //    HashSet<string> included;

    //    internal OdbcPolicy(params string[] includedRelationships)
    //        : base(SageDb.StandardPolicy.Mapping)
    //    {
    //        this.included = new HashSet<string>(includedRelationships);
    //    }

    //    public override bool IsIncluded(MemberInfo member)
    //    {
    //        return this.included.Contains(member.Name);
    //    }
    //}

    public class CustomMapping : BasicMapping
    {


        private EnumAccessibility accessibility = EnumAccessibility.Both;
        public CustomMapping(QueryLanguage language)
            : base()
        {


            if (language is OdbcSqlLanguage) accessibility = EnumAccessibility.OnlyOdbc;
            else if (language is TSqlLanguage) accessibility = EnumAccessibility.OnlySQL;
//            accessibility = EnumAccessibility.OnlyOdbc;
        }

        public override string GetTableName(MappingEntity entity)
        {
            return RepositoryClassDefinition.Current.GetByClassId(entity.EntityType).TableID;
        }

        public override bool IsRelationshipSource(MappingEntity entity, MemberInfo member)
        {
            return false;
        }
        public override bool IsRelationshipTarget(MappingEntity entity, MemberInfo member)
        {
            return false;
        }
        
        public override bool IsPrimaryKey(MappingEntity entity, MemberInfo member)
        {
            var q = RepositoryClassDefinition.Current.GetByClassId(entity.EntityType).GetByName(member.Name);
            if (q == null) return false;

            return q.PrimaryKey;
        }

        
        public override string GetColumnName(MappingEntity entity, MemberInfo member)
        {
            var q = RepositoryClassDefinition.Current.GetByClassId(entity.EntityType).GetByName(member.Name);
            if (q == null) return string.Empty;
            return q.FieldName;
        }

       
        //public override bool IsGenerated(MappingEntity entity, MemberInfo member)
        //{
        //    var q = RepositoryClassDefinition.Current.GetByClassId(entity.Type).GetByName(member.Name);
        //    if (q == null) return false;
        //    return true;
        //}

        public override bool IsMapped(MappingEntity entity, MemberInfo member)
        {
            var q = RepositoryClassDefinition.Current.GetByClassId(entity.EntityType).GetByName(member.Name);
            if (q == null) return false;
            if (q.Accessibility==EnumAccessibility.Both) return true;
            return q.Accessibility == accessibility;
        }

        public override bool IsColumn(MappingEntity entity, MemberInfo member)
        {
            var q = RepositoryClassDefinition.Current.GetByClassId(entity.EntityType).GetByName(member.Name);
            if (q == null) return false;
            return true;
        }

        //private Expression GetIdentityCheck(Expression root, MappingEntity entity, Expression instance)
        //{
        //    return this.GetMappedMembers(entity)
        //    .Where(m => this.IsPrimaryKey(entity, m))
        //    .Select(m =>
        //        Expression.Equal(
        //            this.GetMemberExpression(root, entity, m),
        //            Expression.MakeMemberAccess(instance, m)
        //            ))
        //    .Aggregate((x, y) => Expression.And(x, y));
        //}

        public override bool IsUpdatable(MappingEntity entity, MemberInfo member)
        {
            var q = RepositoryClassDefinition.Current.GetByClassId(entity.EntityType).GetByName(member.Name);
            if (q == null) return false;
            return q.CanBeUpdated;
        }

        public bool IsInsertable(MappingEntity entity, MemberInfo member)
        {
            var q = RepositoryClassDefinition.Current.GetByClassId(entity.EntityType).GetByName(member.Name);
            if (q == null) return false;
            return q.CanBeInserted;
        }


        //public override System.Linq.Expressions.Expression GetUpdateExpression(MappingEntity entity, Expression instance, LambdaExpression updateCheck, LambdaExpression selector, Expression @else)
        //{
        //    var tableAlias = new TableAlias();
        //    var table = new TableExpression(tableAlias, entity, this.GetTableName(entity));

        //    var where = this.GetIdentityCheck(table, entity, instance);
        //    if (updateCheck != null)
        //    {
        //        Expression typeProjector = this.GetTypeProjection(table, entity);
        //        Expression pred = DbExpressionReplacer.Replace(updateCheck.Body, updateCheck.Parameters[0], typeProjector);
        //        where = Expression.And(where, pred);
        //    }

        //    var assignments =
        //        from m in this.GetMappedMembers(entity)
        //        where this.IsColumn(entity, m) && this.IsUpdatable(entity,m)
        //        select new ColumnAssignment(
        //            (ColumnExpression)this.GetMemberExpression(table, entity, m),
        //            Expression.MakeMemberAccess(instance, m)
        //            );

        //    Expression result = null;
        //    if (selector != null)
        //    {
        //        Expression resultWhere = this.GetIdentityCheck(table, entity, instance);
        //        Expression typeProjector = this.GetTypeProjection(table, entity);
        //        Expression selection = DbExpressionReplacer.Replace(selector.Body, selector.Parameters[0], typeProjector);
        //        TableAlias newAlias = new TableAlias();
        //        var pc = ColumnProjector.ProjectColumns(this.Language.CanBeColumn, selection, null, newAlias, tableAlias);
        //        result = new ProjectionExpression(
        //            new SelectExpression(newAlias, pc.Columns, table, resultWhere),
        //            pc.Projector,
        //            this.GetAggregator(selector.Body.Type, typeof(IEnumerable<>).MakeGenericType(selector.Body.Type))
        //            );
        //    }

        //    return new UpdateExpression(table, where, assignments, result);

        //}

        //public override Expression GetInsertExpression(MappingEntity entity, Expression instance, LambdaExpression selector)
        //{
        //    var tableAlias = new TableAlias();
        //    var table = new TableExpression(tableAlias, entity, this.GetTableName(entity));

        //    var assignments =
        //        from m in this.GetMappedMembers(entity)
        //        where this.IsColumn(entity, m) && !this.IsGenerated(entity, m) && IsInsertable(entity,m)
        //        select new ColumnAssignment(
        //            (ColumnExpression)this.GetMemberExpression(table, entity, m),
        //            Expression.MakeMemberAccess(instance, m)
        //            );

        //    Expression selectorResult = null;
        //    if (selector != null)
        //    {
        //        Expression where = null;
        //        var generatedIds = this.GetMappedMembers(entity).Where(m => this.IsPrimaryKey(entity, m) && this.IsGenerated(entity, m));
        //        if (generatedIds.Any())
        //        {
        //            where = generatedIds.Select((m, i) =>
        //                Expression.Equal(this.GetMemberExpression(table, entity, m), this.Language.GetGeneratedIdExpression(m))
        //                ).Aggregate((x, y) => Expression.And(x, y));
        //        }
        //        else
        //        {
        //            where = this.GetIdentityCheck(table, entity, instance);
        //        }

        //        Expression typeProjector = this.GetTypeProjection(table, entity);
        //        Expression selection = DbExpressionReplacer.Replace(selector.Body, selector.Parameters[0], typeProjector);
        //        TableAlias newAlias = new TableAlias();
        //        var pc = ColumnProjector.ProjectColumns(this.Language.CanBeColumn, selection, null, newAlias, tableAlias);
        //        ProjectionExpression proj = new ProjectionExpression(
        //            new SelectExpression(newAlias, pc.Columns, table, where),
        //            pc.Projector,
        //            this.GetAggregator(selector.Body.Type, typeof(IEnumerable<>).MakeGenericType(selector.Body.Type))
        //            );

        //        selectorResult = proj;
        //    }

        //    return new InsertExpression(table, assignments, selectorResult);
        //}
        //#region typeman
        //private static Type OdbcTypeToType(OdbcType dbType)
        //{
        //    switch (dbType)
        //    {
        //        case OdbcType.BigInt:
        //            return typeof(Int64);
        //        case OdbcType.Binary:
        //            return typeof(byte[]);
        //        case OdbcType.Bit:
        //            return typeof(Boolean);
        //        case OdbcType.Char:
        //            return typeof(char);
        //        case OdbcType.Date:
        //            return typeof(DateTime);
        //        case OdbcType.DateTime:
        //        case OdbcType.SmallDateTime:
        //            return typeof(DateTime);
        //        //case OdbcType.DateTime2:
        //        //    return DbType.DateTime2;
        //        //case OdbcType.DateTimeOffset:
        //        //    return DbType.DateTimeOffset;
        //        case OdbcType.Decimal:
        //            return typeof(decimal);

        //        case OdbcType.Double:
        //        case OdbcType.Real:
        //            return typeof(double);
        //        case OdbcType.Image:
        //            return typeof(byte[]);
        //        case OdbcType.Int:
        //            return typeof(Int32);
        //        //case OdbcType.Money:
        //        //case OdbcType.SmallMoney:
        //        //    return DbType.Currency;

        //        case OdbcType.NChar:
        //            return typeof(string);
        //        case OdbcType.NText:
        //        case OdbcType.NVarChar:
        //            return typeof(string);
        //        case OdbcType.SmallInt:
        //            return typeof(Int16);
        //        case OdbcType.Text:
        //            return typeof(byte[]);
        //        case OdbcType.Time:
        //            return typeof(TimeSpan);
        //        case OdbcType.Timestamp:
        //            return typeof(TimeSpan);
        //        case OdbcType.TinyInt:
        //            return typeof(byte);
        //        //case OdbcType.Udt:
        //        //    return DbType.Object;
        //        case OdbcType.UniqueIdentifier:
        //            return typeof(Guid);
        //        case OdbcType.VarBinary:
        //            return typeof(byte[]);
        //        case OdbcType.VarChar:
        //            return typeof(string);
        //        //case OdbcType.Variant:
        //        //    return DbType.Object;
        //        //case OdbcType.Xml:
        //        //    return DbType.String;
        //        default:
        //            throw new InvalidOperationException(string.Format("Unhandled sql type: {0}", dbType));
        //    }
        //}
        ////public override QueryType GetColumnType(MappingEntity entity, MemberInfo member)
        ////{
        ////    var q = RepositoryClassDefinition.Current.GetByClassId(entity.EntityType).GetByName(member.Name);
        ////    if (q == null) return this.Language.TypeSystem.GetColumnType(TypeHelper.GetMemberType(member));
        ////    else return this.Language.TypeSystem.GetColumnType(OdbcTypeToType(q.OdbcType));
        ////}
        //#endregion typeman

    }
  
}

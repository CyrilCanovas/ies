using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IesWebPortal.Classes
{
    public static class HtmlHelperExtensions
    {
        //public static HtmlString SortBy(this HtmlHelper html)
        //{
        //    //< a href = "#" class="btn btn-success btn-xs" role="button">
        //    //<span class="glyphicon glyphicon-sort-by-alphabet" aria-hidden="true"></span>
        //    //</a>
        //    return new MvcHtmlString("ccouc");
        //}

        private static string Decompose(System.Linq.Expressions.MemberExpression body)
        {
            if (body == null)
            {
                return null;
            }
            var value = Decompose(body.Expression as System.Linq.Expressions.MemberExpression);
            if (value == null)
            {
                return body.Member.Name;
            }
            return value + "." + body.Member.Name;
        }

        //public static HtmlString SortBy<T, TValue>(this IHtmlHelper<T> html, Expression<Func<T, TValue>> expression, object obj, string action = null)
        //{
        //    var proppath = Decompose(expression.Body as System.Linq.Expressions.MemberExpression);
        //    var _obj = (dynamic)obj;


        //    if (_obj.SortColumn == proppath)
        //    {
        //        switch ((SortDirection)_obj.SortDirection)
        //        {
        //            case SortDirection.Ascending:
        //                return new HtmlString(@"<a href = ""#"" class=""btn btn-success btn-xs"" role=""button""><span class=""glyphicon glyphicon-sort-by-alphabet"" aria-hidden=""true""></span></a>");
        //            case SortDirection.Descending:
        //                return new HtmlString(@"<a href = ""#"" class=""btn btn-success btn-xs"" role=""button""><span class=""glyphicon glyphicon-sort-by-alphabet-alt"" aria-hidden=""true""></span></a>");
        //        }
        //    }
        //    return new HtmlString(@"<a href = ""#"" class=""btn btn-success btn-xs"" role=""button""><span class=""glyphicon glyphicon-sort-by-alphabet"" aria-hidden=""true""></span></a>");
        //}

        public static HtmlString SortBy<T, TValue>(this IHtmlHelper<IEnumerable<T>> html, Expression<Func<T, TValue>> expression, string sortcolumn, SortDirection? sortdirection, string actionName = null, string controllerName = null, object htmlAttributes = null)
        {
            var proppath = Decompose(expression.Body as System.Linq.Expressions.MemberExpression);
            var _sortdirection = SortDirection.Ascending;
            if (sortcolumn == proppath)
            {
                _sortdirection = ((sortdirection ?? SortDirection.Ascending) == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending);
            }

            var routevalues = new RouteValueDictionary(new { sortcolumn = proppath, sortdirection = _sortdirection });
            if (htmlAttributes != null)
            {
                foreach (var routevalue in new RouteValueDictionary(htmlAttributes))
                {
                    routevalues.Add(routevalue.Key, routevalue.Value);
                }
            }
            var result = new HtmlString(
                string.Format(
                    //@"<a href = ""/{0}?{1}"" class=""btn btn-success btn-xs"" role=""button""><span class=""glyphicon glyphicon-sort-by-alphabet{2}"" aria-hidden=""true""></span></a>",
                    @"<a href = ""/{0}?{1}"" class=""btn btn-success btn-xs"" role=""button""><span class=""bi-sort-alpha-{2}"" aria-hidden=""true""></span></a>",
                    string.Join("/", (new string[] { controllerName, actionName }).Where(x => x != null)),
                    string.Join("&", routevalues.Select(x => string.Format("{0}={1}", x.Key, x.Value))),
                    _sortdirection == SortDirection.Ascending ? "up" : "down"
                )
            );
            return result;
        }
    }
}

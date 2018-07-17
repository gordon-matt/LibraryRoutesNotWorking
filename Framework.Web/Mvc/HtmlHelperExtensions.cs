using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Extenso.AspNetCore.Mvc;
using Extenso.AspNetCore.Mvc.Rendering;
using Framework.ComponentModel;
using Framework.Infrastructure;
using Framework.Tenants.Services;
using Framework.Web.Mvc.KoreUI;
using Framework.Web.Mvc.KoreUI.Providers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Framework.Web.Mvc
{
    public static class HtmlHelperExtensions
    {
        #region Html Link

        public static IHtmlContent EmailLink(this IHtmlHelper helper, string emailAddress)
        {
            return helper.Link(string.Concat("mailto:", emailAddress));
        }

        public static IHtmlContent Link(this IHtmlHelper helper, string href, PageTarget target = PageTarget.Default)
        {
            return helper.Link(href, href, target);
        }

        public static IHtmlContent Link(this IHtmlHelper helper, string linkText, string href, PageTarget target = PageTarget.Default)
        {
            return helper.Link(linkText, href, null, target);
        }

        public static IHtmlContent Link(this IHtmlHelper helper, string linkText, string href, object htmlAttributes, PageTarget target = PageTarget.Default)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", href);
            builder.InnerHtml.Append(linkText);

            switch (target)
            {
                case PageTarget.Blank: builder.MergeAttribute("target", "_blank"); break;
                case PageTarget.Parent: builder.MergeAttribute("target", "_parent"); break;
                case PageTarget.Self: builder.MergeAttribute("target", "_self"); break;
                case PageTarget.Top: builder.MergeAttribute("target", "_top"); break;
            }

            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            return new HtmlString(builder.Build());
        }

        public static IHtmlContent Link(this IHtmlHelper helper, string linkText, string href, RouteValueDictionary htmlAttributes, PageTarget target = PageTarget.Default)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", href);
            builder.InnerHtml.Append(linkText);

            switch (target)
            {
                case PageTarget.Blank: builder.MergeAttribute("target", "_blank"); break;
                case PageTarget.Parent: builder.MergeAttribute("target", "_parent"); break;
                case PageTarget.Self: builder.MergeAttribute("target", "_self"); break;
                case PageTarget.Top: builder.MergeAttribute("target", "_top"); break;
            }

            builder.MergeAttributes(htmlAttributes);

            return new HtmlString(builder.Build());
        }

        #endregion Html Link

        public static IHtmlContent HelpText(this IHtmlHelper html, string helpText, object htmlAttributes = null)
        {
            var tagBuilder = new FluentTagBuilder("p")
                .AddCssClass("help-block")
                .MergeAttributes(htmlAttributes)
                .SetInnerHtml(helpText);

            return new HtmlString(tagBuilder.ToString());
        }

        public static IHtmlContent HelpTextFor<TModel, TProperty>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            var memberExpression = expression.Body as MemberExpression;
            var propertyInfo = (memberExpression.Member as PropertyInfo);
            var attribute = propertyInfo.GetCustomAttributes().OfType<LocalizedHelpTextAttribute>().FirstOrDefault();

            if (attribute == null)
            {
                return HtmlString.Empty;
            }

            var tagBuilder = new FluentTagBuilder("p")
                .AddCssClass("help-block")
                .MergeAttributes(htmlAttributes)
                .SetInnerHtml(attribute.HelpText);

            return new HtmlString(tagBuilder.ToString());
        }

        public static Framework<TModel> Framework<TModel>(this IHtmlHelper<TModel> html) where TModel : class
        {
            return new Framework<TModel>(html);
        }

        public static KoreUI<TModel> KoreUI<TModel>(this IHtmlHelper<TModel> htmlHelper, IKoreUIProvider provider = null)
        {
            if (provider != null)
            {
                return new KoreUI<TModel>(htmlHelper, provider);
            }

            string areaName = (string)htmlHelper.ViewContext.RouteData.DataTokens["area"];
            if (!string.IsNullOrEmpty(areaName) && KoreUISettings.AreaUIProviders.ContainsKey(areaName))
            {
                return new KoreUI<TModel>(htmlHelper, KoreUISettings.AreaUIProviders[areaName]);
            }
            return new KoreUI<TModel>(htmlHelper);
        }

        ///// <summary>
        ///// Create an HTML tree from a recursive collection of items
        ///// </summary>
        //public static TreeView<T> TreeView<T>(this IHtmlHelper html, IEnumerable<T> items)
        //{
        //    return new TreeView<T>(html, items);
        //}
    }

    public class Framework<TModel>
        where TModel : class
    {
        private readonly IHtmlHelper<TModel> html;

        internal Framework(IHtmlHelper<TModel> html)
        {
            this.html = html;
        }

        public IHtmlContent TenantsCheckBoxList(
            string name,
            IEnumerable<string> selectedTenantIds,
            object labelHtmlAttributes = null,
            object checkboxHtmlAttributes = null)
        {
            var selectList = GetTenantsMultiSelectList(selectedTenantIds);
            return html.CheckBoxList(name, selectList, selectedTenantIds, labelHtmlAttributes: labelHtmlAttributes, checkboxHtmlAttributes: checkboxHtmlAttributes);
        }

        public IHtmlContent TenantsDropDownList(string name, string selectedValue = null, object htmlAttributes = null, string emptyText = null)
        {
            var selectList = GetTenantsSelectList(selectedValue, emptyText);
            return html.DropDownList(name, selectList, htmlAttributes);
        }

        private static IEnumerable<SelectListItem> GetTenantsMultiSelectList(IEnumerable<string> selectedValues = null, string emptyText = null)
        {
            var service = EngineContext.Current.Resolve<ITenantService>();

            using (var connection = service.OpenConnection())
            {
                return connection.Query()
                    .OrderBy(x => x.Name)
                    .ToList()
                    .ToMultiSelectList(
                        value => value.Id,
                        text => text.Name,
                        selectedValues,
                        emptyText);
            }
        }

        private static IEnumerable<SelectListItem> GetTenantsSelectList(string selectedValue = null, string emptyText = null)
        {
            var service = EngineContext.Current.Resolve<ITenantService>();

            using (var connection = service.OpenConnection())
            {
                return connection.Query()
                    .OrderBy(x => x.Name)
                    .ToList()
                    .ToSelectList(
                        value => value.Id,
                        text => text.Name,
                        selectedValue,
                        emptyText);
            }
        }
    }
}
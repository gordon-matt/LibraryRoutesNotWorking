﻿using System.Collections.Generic;
using System.Linq;
using Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Framework.Web.Mvc.Razor
{
    public class TenantViewLocationExpander : FrameworkViewLocationExpander
    {
        private const string THEME_KEY = "theme";

        public override void PopulateValues(ViewLocationExpanderContext context)
        {
            base.PopulateValues(context);
            //context.Values[THEME_KEY] = context.ActionContext.HttpContext.GetTenant<Tenant>()?.Theme;
            context.Values[THEME_KEY] = "Default";
        }

        public override IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            viewLocations = base.ExpandViewLocations(context, viewLocations);

            string theme = null;
            if (context.Values.TryGetValue(THEME_KEY, out theme))
            {
                viewLocations = new[]
                {
                    $"/Areas/{{2}}/Themes/{theme}/Views/{{1}}/{{0}}.cshtml",
                    $"/Areas/{{2}}/Themes/{theme}/Views/Shared/{{0}}.cshtml",
                    $"/Themes/{theme}/Views/{{1}}/{{0}}.cshtml",
                    $"/Themes/{theme}/Views/Shared/{{0}}.cshtml",
                }
                .Concat(viewLocations);
            }

            return viewLocations;
        }
    }
}
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Framework.Web.Mvc.Controls
{
    public class ExtendedSelectListItem : SelectListItem
    {
        public object HtmlAttributes { get; set; }

        public string Category { get; set; }
    }
}
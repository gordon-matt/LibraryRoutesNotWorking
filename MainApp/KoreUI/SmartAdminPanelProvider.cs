using System.IO;
using Extenso.AspNetCore.Mvc.Rendering;
using Framework.Web.Mvc.KoreUI;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrameworkDemo.KoreUI
{
    public class SmartAdminPanelProvider : IPanelProvider
    {
        #region IPanelProvider Members

        public void BeginPanel(Panel panel, TextWriter writer)
        {
            switch (panel.State)
            {
                case State.Default: panel.EnsureClass("jarviswidget"); break;
                case State.Important: panel.EnsureClass("jarviswidget jarviswidget-color-red"); break;
                case State.Info: panel.EnsureClass("jarviswidget jarviswidget-color-pink"); break;
                case State.Inverse: panel.EnsureClass("jarviswidget jarviswidget-color-darken"); break;
                case State.Primary: panel.EnsureClass("jarviswidget jarviswidget-color-blue"); break;
                case State.Success: panel.EnsureClass("jarviswidget jarviswidget-color-greenLight"); break;
                case State.Warning: panel.EnsureClass("jarviswidget jarviswidget-color-orange"); break;
            }
            panel.EnsureHtmlAttribute("data-widget-deletebutton", "false");
            panel.EnsureHtmlAttribute("data-widget-fullscreenbutton", "true");
            panel.EnsureHtmlAttribute("data-widget-editbutton", "false");
            panel.EnsureHtmlAttribute("data-widget-togglebutton", "true");
            panel.EnsureHtmlAttribute("data-widget-colorbutton", "false");
            panel.EnsureHtmlAttribute("data-widget-sortable", "false");
            panel.EnsureHtmlAttribute("role", "widget");

            var builder = new TagBuilder("div");
            builder.TagRenderMode = TagRenderMode.StartTag;
            builder.MergeAttributes<string, object>(panel.HtmlAttributes);
            string tag = builder.Build();

            writer.Write(tag);
        }

        public void BeginPanelSection(PanelSectionType sectionType, TextWriter writer, string title = null)
        {
            switch (sectionType)
            {
                case PanelSectionType.Heading:
                    {
                        writer.Write(string.Format(
@"<header role=""heading"">
    <h2 class=""panel-title"">{0}</h2>", title));
                    }
                    break;

                case PanelSectionType.Body:
                    {
                        writer.Write(@"<div role=""content""><div class=""widget-body"">");
                    }
                    break;
            }
        }

        public void EndPanel(Panel panel, TextWriter writer)
        {
            writer.Write("</div>");
        }

        public void EndPanelSection(PanelSectionType sectionType, TextWriter writer)
        {
            switch (sectionType)
            {
                case PanelSectionType.Heading: writer.Write("</header>"); break;
                case PanelSectionType.Body: writer.Write("</div></div>"); break;
            }
        }

        #endregion IPanelProvider Members
    }
}
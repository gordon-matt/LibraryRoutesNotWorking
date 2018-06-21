﻿using System.IO;
using Extenso.AspNetCore.Mvc.Rendering;
using Framework.Web.Mvc.KoreUI.Providers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Framework.Web.Mvc.KoreUI
{
    public class Bootstrap3ToolbarProvider : IToolbarProvider
    {
        private readonly IKoreUIProvider uiProvider;

        public Bootstrap3ToolbarProvider(IKoreUIProvider uiProvider)
        {
            this.uiProvider = uiProvider;
        }

        #region IToolbarProvider Members

        public void BeginToolbar(Toolbar toolbar, TextWriter writer)
        {
            toolbar.EnsureClass("btn-toolbar");
            toolbar.EnsureHtmlAttribute("role", "toolbar");

            var builder = new TagBuilder("div");
            builder.TagRenderMode = TagRenderMode.StartTag;
            builder.MergeAttributes<string, object>(toolbar.HtmlAttributes);
            string tag = builder.Build();

            writer.Write(tag);
        }

        public void BeginButtonGroup(TextWriter writer)
        {
            writer.Write(@"<div class=""btn-group"">");
        }

        public void EndButtonGroup(TextWriter writer)
        {
            writer.Write("</div>");
        }

        public void EndToolbar(Toolbar toolbar, TextWriter writer)
        {
            writer.Write("</div>");
        }

        public void AddButton(TextWriter writer, string text, State state, string onClick = null, object htmlAttributes = null)
        {
            var button = uiProvider.Button(text, state, onClick, htmlAttributes);
            writer.Write(button.ToString());
        }

        #endregion IToolbarProvider Members
    }
}
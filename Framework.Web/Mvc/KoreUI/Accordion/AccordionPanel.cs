﻿using System;
using System.IO;
using Framework.Web.Mvc.KoreUI.Providers;

namespace Framework.Web.Mvc.KoreUI
{
    public class AccordionPanel : IDisposable
    {
        private readonly TextWriter textWriter;
        private readonly IKoreUIProvider provider;

        internal AccordionPanel(IKoreUIProvider provider, TextWriter writer, string title, string panelId, string parentAccordionId, bool expanded = false)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            this.provider = provider;
            this.textWriter = writer;

            provider.AccordionProvider.BeginAccordionPanel(this.textWriter, title, panelId, parentAccordionId, expanded);
        }

        public void Dispose()
        {
            provider.AccordionProvider.EndAccordionPanel(this.textWriter);
        }
    }
}
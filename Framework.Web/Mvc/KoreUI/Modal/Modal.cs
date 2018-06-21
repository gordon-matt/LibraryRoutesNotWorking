﻿using System;
using System.IO;

namespace Framework.Web.Mvc.KoreUI
{
    public class Modal : HtmlElement
    {
        public string Id { get; private set; }

        public Modal(string id = null, object htmlAttributes = null)
            : base(htmlAttributes)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = "modal-" + Guid.NewGuid();
            }
            this.Id = id;
            EnsureHtmlAttribute("id", this.Id);
        }

        protected internal override void StartTag(TextWriter textWriter)
        {
            Provider.ModalProvider.BeginModal(this, textWriter);
        }

        protected internal override void EndTag(TextWriter textWriter)
        {
            Provider.ModalProvider.EndModal(this, textWriter);
        }
    }
}
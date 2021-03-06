﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Framework.Infrastructure;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Framework.Web.Mvc.EmbeddedResources
{
    public class EmbeddedViewFileProvider : IFileProvider
    {
        private static readonly char[] invalidFileNameChars = Path.GetInvalidFileNameChars()
            .Where(c => c != '/' && c != '\\')
            .ToArray();

        private EmbeddedResourceTable embeddedViews;

        private EmbeddedResourceTable EmbeddedViews
        {
            get
            {
                if (embeddedViews == null)
                {
                    var embeddedResourceResolver = EngineContext.Current.Resolve<IEmbeddedResourceResolver>();
                    embeddedViews = embeddedResourceResolver.Views;
                }
                return embeddedViews;
            }
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            if (string.IsNullOrEmpty(subpath))
            {
                return new NotFoundFileInfo(subpath);
            }

            if (subpath.StartsWith("/", StringComparison.Ordinal))
            {
                subpath = subpath.Substring(1);
            }

            if (!IsEmbeddedScript(subpath))
            {
                return null;
            }

            var metadata = EmbeddedViews.FindEmbeddedResource(subpath);

            if (metadata == null)
            {
                return null;
            }

            return new EmbeddedResourceFileInfo(metadata);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            // The file name is assumed to be the remainder of the resource name.
            if (subpath == null)
            {
                return NotFoundDirectoryContents.Singleton;
            }

            // Relative paths starting with a leading slash okay
            if (subpath.StartsWith("/", StringComparison.Ordinal))
            {
                subpath = subpath.Substring(1);
            }

            // Non-hierarchal.
            if (!subpath.Equals(string.Empty))
            {
                return NotFoundDirectoryContents.Singleton;
            }

            var entries = new List<IFileInfo>();

            // TODO: The list of resources in an assembly isn't going to change. Consider caching.
            var resources = EmbeddedViews.Resources;
            foreach (var resource in resources)
            {
                entries.Add(new EmbeddedResourceFileInfo(resource));
            }

            return new EnumerableDirectoryContents(entries);
        }

        public IChangeToken Watch(string pattern)
        {
            return NullChangeToken.Singleton;
        }

        private static bool HasInvalidPathChars(string path)
        {
            return path.IndexOfAny(invalidFileNameChars) != -1;
        }

        private bool IsEmbeddedScript(string subpath)
        {
            if (string.IsNullOrEmpty(subpath))
            {
                return false;
            }

            return EmbeddedViews.ContainsEmbeddedResource(subpath);
        }
    }
}
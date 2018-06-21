namespace Framework.Web.Mvc.EmbeddedResources
{
    public interface IEmbeddedResourceResolver
    {
        EmbeddedResourceTable Scripts { get; }

        EmbeddedResourceTable Content { get; }

        EmbeddedResourceTable Views { get; }
    }
}
using Framework.Web.Navigation;

namespace Framework.Web
{
    public interface IWebWorkContext : IWorkContext
    {
        BreadcrumbCollection Breadcrumbs { get; set; }

        string CurrentTheme { get; }
    }
}
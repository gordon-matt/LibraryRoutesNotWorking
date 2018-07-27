namespace Framework.Web
{
    public interface IWebWorkContext : IWorkContext
    {
        string CurrentTheme { get; }
    }
}
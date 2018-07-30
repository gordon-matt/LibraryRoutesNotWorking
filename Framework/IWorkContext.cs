using Framework.Security.Membership;
using Framework.Tenants.Domain;

namespace Framework
{
    public interface IWorkContext
    {
        T GetState<T>(string name);

        void SetState<T>(string name, T value);

        string CurrentCultureCode { get; }
        
        FrameworkUser CurrentUser { get; }
    }
}
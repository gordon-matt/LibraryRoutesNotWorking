using System;

namespace Framework
{
    public interface IWorkContextStateProvider
    {
        Func<IWorkContext, T> Get<T>(string name);
    }
}
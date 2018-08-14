namespace Framework
{
    public interface IWorkContext
    {
        T GetState<T>(string name);

        void SetState<T>(string name, T value);

        string CurrentCultureCode { get; }
    }
}
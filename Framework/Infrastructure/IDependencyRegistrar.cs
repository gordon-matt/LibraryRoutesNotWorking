namespace Framework.Infrastructure
{
    public interface IDependencyRegistrar<TContainerBuilder>
    {
        void Register(TContainerBuilder builder, ITypeFinder typeFinder);

        int Order { get; }
    }
}
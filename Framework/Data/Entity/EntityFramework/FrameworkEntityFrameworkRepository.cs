using Extenso.Data.Entity;
using Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Framework.Data.Entity.EntityFramework
{
    public class FrameworkEntityFrameworkRepository<TEntity> : EntityFrameworkRepository<TEntity>
        where TEntity : class, IEntity
    {
        #region Constructor

        public FrameworkEntityFrameworkRepository(
            IDbContextFactory contextFactory,
            ILoggerFactory loggerFactory)
            : base(contextFactory, loggerFactory)
        {
        }

        #endregion Constructor

        protected override DbContext GetContext()
        {
            if (contextFactory == null)
            {
                contextFactory = EngineContext.Current.Resolve<IDbContextFactory>();
            }

            return contextFactory.GetContext();
        }
    }
}
using Extenso.Data.Entity;
using Framework.Caching;
using Framework.Data.Services;
using Framework.Logging.Domain;

namespace Framework.Logging.Services
{
    public interface ILogService : IGenericDataService<LogEntry>
    {
    }

    public class LogService : GenericDataService<LogEntry>, ILogService
    {
        public LogService(ICacheManager cacheManager, IRepository<LogEntry> repository)
            : base(cacheManager, repository)
        {
        }
    }
}
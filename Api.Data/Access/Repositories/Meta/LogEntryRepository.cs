using Api.Data.Models.Meta;

namespace Api.Data.Access.Repositories.Meta
{
    public interface ILogEntryRepository : IGenericRepository<LogEntry, string>
    {

    }

    public class LogEntryRepository : EntityFrameworkRepository<LogEntry, string>, ILogEntryRepository
    {
        public LogEntryRepository(AppDatabaseContext context) : base(context)
        {
        }
    }
}

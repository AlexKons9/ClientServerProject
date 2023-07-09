using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Services.Interfaces
{
    public interface ILoggingService
    {
        Task InsertLog(Log log);
        Task<IEnumerable<Log>> RetrieveLogsBetweenTimeSpan(DateTime startTime, DateTime endTime);
    }
}

using System;
using System.Collections.Generic;

namespace Sean.Core.DbMigrator;

public interface IMigrationFactory
{
    long GetCurrentVersion();
    bool Add(Action<IMigrationHistoryEntity> action);
    bool IsExecuted(long version);
    bool ExecuteSql(IList<string> sqlList);
    void OnException(ExceptionContext context);
}
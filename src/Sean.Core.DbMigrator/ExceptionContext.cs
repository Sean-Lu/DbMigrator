using System;

namespace Sean.Core.DbMigrator;

public class ExceptionContext
{
    public ExceptionContext(Exception exception)
    {
        Exception = exception;
    }

    public Exception Exception { get; }
    public bool AddHistory { get; set; }
    public bool Rollback { get; set; }
}
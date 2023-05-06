using System;

namespace Sean.Core.DbMigrator;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class MigrationAttribute : Attribute
{
    public long Version => _version;
    public string Description { get; set; }

    private readonly long _version;

    public MigrationAttribute(long version)
    {
        _version = version;
    }
}
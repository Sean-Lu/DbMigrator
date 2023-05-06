using System;

namespace Sean.Core.DbMigrator;

/// <summary>
/// The table entity of database migration history.
/// </summary>
public interface IMigrationHistoryEntity
{
    /// <summary>
    /// Primary key
    /// </summary>
    long Id { get; set; }
    /// <summary>
    /// Migration version
    /// </summary>
    long Version { get; set; }
    /// <summary>
    /// Migration class
    /// </summary>
    string MigrationClass { get; set; }
    /// <summary>
    /// Whether the execution is successful
    /// </summary>
    bool Success { get; set; }
    /// <summary>
    /// The description of the execution
    /// </summary>
    string Description { get; set; }
    /// <summary>
    /// The start time of execution
    /// </summary>
    DateTime ExecutionTime { get; set; }
    /// <summary>
    /// The total elapsed time of execution, in milliseconds.
    /// </summary>
    long ExecutionElapsed { get; set; }

}
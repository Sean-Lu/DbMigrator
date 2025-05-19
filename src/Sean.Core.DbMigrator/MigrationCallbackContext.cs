using System.Collections.Generic;

namespace Sean.Core.DbMigrator;

public class MigrationCallbackContext
{
    public List<MigrationInfo> MigrationInfos { get; set; }
    public MigrationStep Step { get; set; }

    public int MigrationInfoIndex { get; set; }
    public MigrationInfo MigrationInfo { get; set; }
}
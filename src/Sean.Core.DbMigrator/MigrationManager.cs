namespace Sean.Core.DbMigrator;

public class MigrationManager
{
    public static IMigrationBuilder CreateBuilder(IMigrationBuilder migrationBuilder)
    {
        return migrationBuilder ?? new DefaultMigrationBuilder();
    }
    public static IMigrationBuilder CreateDefaultBuilder()
    {
        return new DefaultMigrationBuilder();
    }
}
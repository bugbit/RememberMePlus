using Microsoft.Maui.Storage;

namespace RememberMePlusApp.Infrastructure.Data.Options;

public sealed class SqliteDataOptions
{
    public const string DefaultDatabaseFileName = "remembermeplus.db3";

    public string DatabasePath { get; init; } = Path.Combine(FileSystem.AppDataDirectory, DefaultDatabaseFileName);
}

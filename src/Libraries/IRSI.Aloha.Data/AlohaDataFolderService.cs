using System.IO.Abstractions;
using IRSI.Aloha.Data.Abstractions;
using IRSI.Common.Abstractions;

namespace IRSI.Aloha.Data;

public class AlohaDataFolderService(
    IEnvironment environment,
    IFileSystem fileSystem
) : IAlohaDataFolderService
{
    private const string IBERDIR = "IBERDIR";
    private readonly Dictionary<DateOnly, IAlohaDataFolder> _businessDateCache = [];
    private IAlohaDataFolder? _dataFolderCache;

    public IAlohaDataFolder GetDataFolder(bool skipCache = false)
    {
        if (!skipCache && _dataFolderCache != null) return _dataFolderCache;

        var basePath = environment.GetEnvironmentVariable(IBERDIR) ??
                       throw new InvalidOperationException($"{IBERDIR} environment variable is not set.");

        var dataPath = fileSystem.Path.Combine(basePath, "Data");
        var dataFolder = GetDataFolder(dataPath);
        if (!skipCache) _dataFolderCache = dataFolder;
        return dataFolder;
    }

    public IAlohaDataFolder GetDataFolder(string fullPath)
    {
        if (!fileSystem.Directory.Exists(fullPath))
            throw new DirectoryNotFoundException($"Directory {fullPath} not found.");
        return new AlohaDataFolder(fileSystem, fullPath);
    }

    public IAlohaDataFolder GetBusinessDateFolder(DateOnly businessDate, bool skipCache = false)
    {
        if (!skipCache && _businessDateCache.TryGetValue(businessDate, out var businessDateFolder))
            return businessDateFolder;

        var basePath = environment.GetEnvironmentVariable(IBERDIR) ??
                       throw new InvalidOperationException($"{IBERDIR} environment variable is not set.");

        var datedFolder = new AlohaDataFolder(fileSystem, basePath, businessDate);
        if (!skipCache) _businessDateCache.Add(businessDate, datedFolder);

        return datedFolder;
    }

    public Dictionary<DateOnly, IAlohaDataFolder> GetBusinessDateFolders(DateOnly startDate, DateOnly endDate,
        bool skipCache = false)
    {
        var businessDates = new Dictionary<DateOnly, IAlohaDataFolder>();

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            businessDates.Add(date, GetBusinessDateFolder(date, skipCache));
        }

        return businessDates;
    }
}
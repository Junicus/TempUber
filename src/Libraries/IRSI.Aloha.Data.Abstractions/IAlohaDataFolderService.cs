namespace IRSI.Aloha.Data.Abstractions;

public interface IAlohaDataFolderService
{
    IAlohaDataFolder GetDataFolder(string fullPath);
    IAlohaDataFolder GetDataFolder(bool skipCache = false);
    IAlohaDataFolder GetBusinessDateFolder(DateOnly businessDate, bool skipCache = false);
    Dictionary<DateOnly, IAlohaDataFolder> GetBusinessDateFolders(DateOnly startBusinessDate, DateOnly endBusinessDate, bool skipCache = false);
}
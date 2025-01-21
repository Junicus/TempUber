using System.Data;

namespace IRSI.Aloha.Data.Abstractions;

public interface IAlohaDataFolder
{
    bool IsBusinessDateFolder { get; }
    // DateOnly? BusinessDate { get; }
    // DataTable? GetAlohaDataTable(string filename, bool skipCache = false);
    T? GetAlohaDataTable<T>(bool skipCache = false) where T : DataTable, IAlohaDataTable, new();
}
using System.Data;
using System.IO.Abstractions;
using System.Text;
using IRSI.Aloha.Data.Abstractions;
using NDbfReaderEx;

namespace IRSI.Aloha.Data;

public class AlohaDataFolder : IAlohaDataFolder
{
    private readonly IFileSystem _fileSystem;
    private readonly string _basePath;
    private readonly Dictionary<string, DataTable> _fileCache = [];

    public bool IsBusinessDateFolder { get; }
    public DateOnly? BusinessDate { get; }

    internal AlohaDataFolder(IFileSystem fileSystem, string basePath)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        _fileSystem = fileSystem;
        _basePath = basePath;
        IsBusinessDateFolder = false;
    }

    internal AlohaDataFolder(IFileSystem fileSystem, string basePath, DateOnly businessDate)
    {
        _fileSystem = fileSystem;
        _basePath = basePath;
        IsBusinessDateFolder = true;
        BusinessDate = businessDate;
    }

    public T? GetAlohaDataTable<T>(bool skipCache = false) where T : DataTable, IAlohaDataTable, new()
    {
        if (!skipCache && _fileCache.TryGetValue(T.FileName, out var dataTable)) return dataTable as T;

        var filePath = IsBusinessDateFolder
            ? Path.Combine(_basePath, $"{BusinessDate:yyyyMMdd}", T.FileName)
            : Path.Combine(_basePath, T.FileName);

        if (!_fileSystem.File.Exists(filePath)) return null;

        var table = GetDataTable(filePath);

        _fileCache.Add(T.FileName, AlohaDataFolderExtensions.ConvertToTypedDataTable<T>(table));

        return _fileCache[T.FileName] as T;
    }

    private DataTable GetDataTable(string filePath)
    {
        var stream = _fileSystem.File.Open(filePath, FileMode.Open);
        var dbfTable = DbfTable.Open(stream, encoding: Encoding.GetEncoding(1252));
        var dataTable = dbfTable.AsDataTable();
        dataTable.TableName = _fileSystem.Path.GetFileNameWithoutExtension(filePath);
        return dataTable;
    }
}
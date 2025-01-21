using System.Data;

namespace IRSI.Aloha.Data.Abstractions;

public static class AlohaDataFolderExtensions
{
    public static T ConvertToTypedDataTable<T>(DataTable dataTable) where T : DataTable, IAlohaDataTable, new()
    {
        var typedDataTable = new T();
        typedDataTable.Merge(dataTable);
        return typedDataTable;
    }
}
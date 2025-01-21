using System.Data;

namespace IRSI.Aloha.Data.Abstractions;

public class ItmDataTable : DataTable, IAlohaDataTable
{
    public static string FileName => "itm.dbf";

    public ItmDataTable()
    {
        Columns.Add(new DataColumn("ID", typeof(decimal)));
        Columns.Add(new DataColumn("OWNERID", typeof(decimal)));
        Columns.Add(new DataColumn("USERNUMBER", typeof(decimal)));
        Columns.Add(new DataColumn("SHORTNAME", typeof(string)));
        Columns.Add(new DataColumn("CHITNAME", typeof(string)));
        Columns.Add(new DataColumn("LONGNAME", typeof(string)));
        Columns.Add(new DataColumn("LONGNAME2", typeof(string)));
        Columns.Add(new DataColumn("BOHNAME", typeof(string)));
        Columns.Add(new DataColumn("ABBREV", typeof(string)));
    }

    protected override Type GetRowType()
    {
        return typeof(ItmDataRow);
    }

    protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
    {
        return new ItmDataRow(builder);
    }

    public ItmDataRow this[int index] => (ItmDataRow)Rows[index];

    public class ItmDataRow : DataRow
    {
        protected internal ItmDataRow(DataRowBuilder builder) : base(builder)
        {
        }

        public decimal Id
        {
            get => (decimal)base["ID"];
            set => base["ID"] = value;
        }
    }
}
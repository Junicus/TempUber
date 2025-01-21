using System.Data;

namespace IRSI.Aloha.Data.Abstractions;

public class MnuDataTable : DataTable, IAlohaDataTable
{
    public static string FileName => "mnu.dbf";

    public MnuDataTable()
    {
        Columns.Add(new DataColumn("ID", typeof(decimal)));
        Columns.Add(new DataColumn("OWNERID", typeof(decimal)));
        Columns.Add(new DataColumn("USERNUMBER", typeof(decimal)));
        Columns.Add(new DataColumn("SHORTNAME", typeof(string)));
        Columns.Add(new DataColumn("LONGNAME", typeof(string)));

        for (var i = 1; i <= 98; i++)
            Columns.Add(new DataColumn($"MENU{i:00}", typeof(decimal)));

        Columns.Add(new DataColumn("REROUTE1", typeof(decimal)));
        Columns.Add(new DataColumn("REROUTE2", typeof(decimal)));
        Columns.Add(new DataColumn("REROUTE3", typeof(decimal)));
        Columns.Add(new DataColumn("REROUTE4", typeof(decimal)));
        Columns.Add(new DataColumn("REROUTE5", typeof(decimal)));
        Columns.Add(new DataColumn("REROUTETO1", typeof(decimal)));
        Columns.Add(new DataColumn("REROUTETO2", typeof(decimal)));
        Columns.Add(new DataColumn("REROUTETO3", typeof(decimal)));
        Columns.Add(new DataColumn("REROUTETO4", typeof(decimal)));
        Columns.Add(new DataColumn("REROUTETO5", typeof(decimal)));
        Columns.Add(new DataColumn("TAX1", typeof(decimal)));
        Columns.Add(new DataColumn("TAX2", typeof(decimal)));
        Columns.Add(new DataColumn("TAX3", typeof(decimal)));
        Columns.Add(new DataColumn("TAX4", typeof(decimal)));
        Columns.Add(new DataColumn("TAX5", typeof(decimal)));
        Columns.Add(new DataColumn("TAX1TO", typeof(decimal)));
        Columns.Add(new DataColumn("TAX2TO", typeof(decimal)));
        Columns.Add(new DataColumn("TAX3TO", typeof(decimal)));
        Columns.Add(new DataColumn("TAX4TO", typeof(decimal)));
        Columns.Add(new DataColumn("TAX5TO", typeof(decimal)));
    }

    protected override Type GetRowType()
    {
        return typeof(MnuDataRow);
    }

    protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
    {
        return new MnuDataRow(builder);
    }

    public MnuDataRow this[int index] => (MnuDataRow)Rows[index];

    public class MnuDataRow : DataRow
    {
        protected internal MnuDataRow(DataRowBuilder builder) : base(builder)
        {
            Menu = new(this);
            Submenu = new(this);
        }

        public decimal Id
        {
            get => (decimal)base["ID"];
            set => base["ID"] = value;
        }

        public decimal OwnerId
        {
            get => (decimal)base["OWNERID"];
            set => base["OWNERID"] = value;
        }

        public decimal UserNumber
        {
            get => (decimal)base["USERNUMBER"];
            set => base["USERNUMBER"] = value;
        }

        public string ShortName
        {
            get => (string)base["SHORTNAME"];
            set => base["SHORTNAME"] = value;
        }

        public string LongName
        {
            get => (string)base["LONGNAME"];
            set => base["LONGNAME"] = value;
        }

        public MenuIndexing Menu { get; }

        public decimal Reroute1
        {
            get => (decimal)base["REROUTE1"];
            set => base["REROUTE1"] = value;
        }

        public decimal Reroute2
        {
            get => (decimal)base["REROUTE2"];
            set => base["REROUTE2"] = value;
        }

        public decimal Reroute3
        {
            get => (decimal)base["REROUTE3"];
            set => base["REROUTE3"] = value;
        }

        public decimal Reroute4
        {
            get => (decimal)base["REROUTE4"];
            set => base["REROUTE4"] = value;
        }

        public decimal Reroute5
        {
            get => (decimal)base["REROUTE5"];
            set => base["REROUTE5"] = value;
        }

        public decimal RerouteTo1
        {
            get => (decimal)base["REROUTETO1"];
            set => base["REROUTETO1"] = value;
        }

        public decimal RerouteTo2
        {
            get => (decimal)base["REROUTETO2"];
            set => base["REROUTETO2"] = value;
        }

        public decimal RerouteTo3
        {
            get => (decimal)base["REROUTETO3"];
            set => base["REROUTETO3"] = value;
        }

        public decimal RerouteTo4
        {
            get => (decimal)base["REROUTETO4"];
            set => base["REROUTETO4"] = value;
        }

        public decimal RerouteTo5
        {
            get => (decimal)base["REROUTETO5"];
            set => base["REROUTETO5"] = value;
        }

        public decimal Tax1
        {
            get => (decimal)base["TAX1"];
            set => base["TAX1"] = value;
        }

        public decimal Tax2
        {
            get => (decimal)base["TAX2"];
            set => base["TAX2"] = value;
        }

        public decimal Tax3
        {
            get => (decimal)base["TAX3"];
            set => base["TAX3"] = value;
        }

        public decimal Tax4
        {
            get => (decimal)base["TAX4"];
            set => base["TAX4"] = value;
        }

        public decimal Tax5
        {
            get => (decimal)base["TAX5"];
            set => base["TAX5"] = value;
        }

        public decimal Tax1To
        {
            get => (decimal)base["TAX1TO"];
            set => base["TAX1TO"] = value;
        }

        public decimal Tax2To
        {
            get => (decimal)base["TAX2TO"];
            set => base["TAX2TO"] = value;
        }

        public decimal Tax3To
        {
            get => (decimal)base["TAX3TO"];
            set => base["TAX3TO"] = value;
        }

        public decimal Tax4To
        {
            get => (decimal)base["TAX4TO"];
            set => base["TAX4TO"] = value;
        }

        public decimal Tax5To
        {
            get => (decimal)base["TAX5TO"];
            set => base["TAX5TO"] = value;
        }

        public IEnumerable<(int position, decimal id)> Submenus
        {
            get
            {
                for (var index = 1; index <= 98; index++)
                {
                    var menu = (decimal)base[$"MENU{index:00}"];
                    if (menu != 0)
                    {
                        yield return (index, menu);
                    }
                }
            }
        }

        public readonly SubmenuIndexing Submenu;

        public class MenuIndexing
        {
            private readonly MnuDataRow _row;

            internal MenuIndexing(MnuDataRow row)
            {
                _row = row;
            }

            public decimal this[int index]
            {
                get
                {
                    ArgumentOutOfRangeException.ThrowIfNegative(index);
                    ArgumentOutOfRangeException.ThrowIfGreaterThan(index, 98);
                    return (decimal)_row[$"MENU{index:00}"];
                }
                set
                {
                    ArgumentOutOfRangeException.ThrowIfNegative(index);
                    ArgumentOutOfRangeException.ThrowIfGreaterThan(index, 98);
                    _row[$"MENU{index:00}"] = value;
                }
            }
        }

        public class SubmenuIndexing
        {
            private readonly MnuDataRow _row;

            internal SubmenuIndexing(MnuDataRow row)
            {
                _row = row;
            }

            public decimal this[int index]
            {
                get
                {
                    ArgumentOutOfRangeException.ThrowIfNegative(index);
                    ArgumentOutOfRangeException.ThrowIfGreaterThan(index, 98);
                    return (decimal)_row[$"MENU{index:00}"];
                }
                set
                {
                    ArgumentOutOfRangeException.ThrowIfNegative(index);
                    ArgumentOutOfRangeException.ThrowIfGreaterThan(index, 98);
                    _row[$"MENU{index:00}"] = value;
                }
            }
        }
    }
}
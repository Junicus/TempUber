using System.Data;

namespace IRSI.Aloha.Data.Abstractions;

public class SubDataTable : DataTable, IAlohaDataTable
{
    public static string FileName => "sub.dbf";

    public SubDataTable()
    {
        Columns.Add(new DataColumn("ID", typeof(decimal)));
        Columns.Add(new DataColumn("OWNERID", typeof(decimal)));
        Columns.Add(new DataColumn("USERNUMBER", typeof(decimal)));
        Columns.Add(new DataColumn("SHORTNAME", typeof(string)));
        Columns.Add(new DataColumn("LONGNAME", typeof(string)));
        Columns.Add(new DataColumn("EXCEPTIONS", typeof(decimal)));
        for (var i = 1; i <= 48; i++)
            Columns.Add(new DataColumn($"ITEM{i:00}", typeof(decimal)));
        for (var i = 1; i <= 48; i++)
            Columns.Add(new DataColumn($"PRICE{i:00}", typeof(decimal)));
        for (var i = 1; i <= 48; i++)
            Columns.Add(new DataColumn($"PRICELVL{i:00}", typeof(decimal)));
        for (var i = 1; i <= 48; i++)
            Columns.Add(new DataColumn($"PRMETHOD{i:00}", typeof(decimal)));
        Columns.Add(new DataColumn("SLAVETOSUB", typeof(decimal)));
        Columns.Add(new DataColumn("AMOWRITE", typeof(string)));
        Columns.Add(new DataColumn("PANEL_ID", typeof(decimal)));
    }

    protected override Type GetRowType()
    {
        return typeof(SubDataRow);
    }

    protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
    {
        return new SubDataRow(builder);
    }

    public SubDataRow this[int index] => (SubDataRow)Rows[index];

    public class SubDataRow : DataRow
    {
        protected internal SubDataRow(DataRowBuilder builder) : base(builder)
        {
            Item = new(this);
            Price = new(this);
            PriceLvl = new(this);
            PrMethod = new(this);
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

        public decimal Exceptions
        {
            get => (decimal)base["EXCEPTIONS"];
            set => base["EXCEPTIONS"] = value;
        }

        public ItemIndexing Item { get; }
        public PriceIndexing Price { get; }
        public PriceLvlIndexing PriceLvl { get; }
        public PrMethodIndexing PrMethod { get; }

        public decimal SlaveToSub
        {
            get => (decimal)base["SLAVETOSUB"];
            set => base["SLAVETOSUB"] = value;
        }

        public string AmoWrite
        {
            get => (string)base["AMOWRITE"];
            set => base["AMOWRITE"] = value;
        }

        public decimal PanelId
        {
            get => (decimal)base["PANEL_ID"];
            set => base["PANEL_ID"] = value;
        }

        public IEnumerable<(int position, decimal id, decimal price, decimal pricelvl, decimal prmethod)> Items
        {
            get
            {
                for (var index = 1; index <= 48; index++)
                {
                    var item = (decimal)this[$"ITEM{index:00}"];
                    if (item != 0)
                    {
                        yield return (index, item, Price[index], PriceLvl[index], PrMethod[index]);
                    }
                }
            }
        }

        public class ItemIndexing
        {
            private readonly SubDataRow _row;

            internal ItemIndexing(SubDataRow row)
            {
                _row = row;
            }

            public decimal this[int index]
            {
                get
                {
                    ArgumentOutOfRangeException.ThrowIfNegative(index);
                    ArgumentOutOfRangeException.ThrowIfGreaterThan(index, 48);
                    return (decimal)_row[$"ITEM{index:00}"];
                }
                set
                {
                    ArgumentOutOfRangeException.ThrowIfNegative(index);
                    ArgumentOutOfRangeException.ThrowIfGreaterThan(index, 48);
                    _row[$"ITEM{index:00}"] = value;
                }
            }
        }

        public class PriceIndexing
        {
            private readonly SubDataRow _row;

            internal PriceIndexing(SubDataRow row)
            {
                _row = row;
            }

            public decimal this[int index]
            {
                get
                {
                    ArgumentOutOfRangeException.ThrowIfNegative(index);
                    ArgumentOutOfRangeException.ThrowIfGreaterThan(index, 48);
                    return (decimal)_row[$"PRICE{index:00}"];
                }
                set
                {
                    ArgumentOutOfRangeException.ThrowIfNegative(index);
                    ArgumentOutOfRangeException.ThrowIfGreaterThan(index, 48);
                    _row[$"PRICE{index:00}"] = value;
                }
            }
        }

        public class PriceLvlIndexing
        {
            private readonly SubDataRow _row;

            internal PriceLvlIndexing(SubDataRow row)
            {
                _row = row;
            }

            public decimal this[int index]
            {
                get
                {
                    ArgumentOutOfRangeException.ThrowIfNegative(index);
                    ArgumentOutOfRangeException.ThrowIfGreaterThan(index, 48);
                    return (decimal)_row[$"PRICELVL{index:00}"];
                }
                set
                {
                    ArgumentOutOfRangeException.ThrowIfNegative(index);
                    ArgumentOutOfRangeException.ThrowIfGreaterThan(index, 48);
                    _row[$"PRICELVL{index:00}"] = value;
                }
            }
        }

        public class PrMethodIndexing
        {
            private readonly SubDataRow _row;

            internal PrMethodIndexing(SubDataRow row)
            {
                _row = row;
            }

            public decimal this[int index]
            {
                get
                {
                    ArgumentOutOfRangeException.ThrowIfNegative(index);
                    ArgumentOutOfRangeException.ThrowIfGreaterThan(index, 48);
                    return (decimal)_row[$"PRMETHOD{index:00}"];
                }
                set
                {
                    ArgumentOutOfRangeException.ThrowIfNegative(index);
                    ArgumentOutOfRangeException.ThrowIfGreaterThan(index, 48);
                    _row[$"PRMETHOD{index:00}"] = value;
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmlPrint.TableStructures
{
    public class PrintTable : PrintTableCell
    {
        public PrintTable()
        {
            Rows = new List<PrintTableRow>();
        }
        public PrintTable(int columns,float[] columnWeights = null) : this()
        {
            Columns = columns;
            if (columnWeights != null)
                ColumnWeights = columnWeights;
            else
            {
                ColumnWeights = new float[Columns];
                for (int i = 0; i < columns; i++)
                    ColumnWeights[i] = 100f / columns;
            }
            if (Columns != ColumnWeights.Length)
                throw new Exception("Column Count doesn't match with the columnWeights");
        }
        #region Dynamic
        public int DummyRowCount
        {
            get
            {
                if (Rows == null)
                    return 0;
                return Rows.Count(r => r.IsDummy);
            }
        }
        #endregion

        #region Override
        public override void SetHeight(float height)
        {
            float increment = height - Bounds.Height;
            base.SetHeight(height);
            float totalHeight = 0;
            foreach (PrintTableRow row in Rows)
                if (row.AutoSize)
                    totalHeight += row.Bounds.Height;
            foreach (PrintTableRow row in Rows)
                if (row.AutoSize && totalHeight != 0)
                    row.SetHeight(row.Bounds.Height + ((row.Bounds.Height / totalHeight) * increment));
        }
        public override void SetBounds(RectangleF bounds)
        {
            SetHeight(bounds.Height);
            base.SetBounds(bounds);
        }
        public override string ToString()
        {
            return $"PrintTable[{Index}] | Rows = {Rows.Count}, Columns = {Columns}";
        }
        #endregion

        public float TotalWeight => ColumnWeights.Sum();
        public List<PrintTableRow> Rows { get; set; }
        public int Columns { get; }
        public float[] ColumnWeights { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmlPrint.TableStructures
{
    public class PrintTableRow : PrintTableCell
    {
        public PrintTableRow()
        {
            _Cells = new List<PrintTableCell>();
        }
        public PrintTableCell this[int index] => _Cells[index];
        public void Add(PrintTableCell cell)
        {
            _Cells.Add(cell);
        }
        #region Override
        public override void SetHeight(float height)
        {
            base.SetHeight(height);
            foreach (PrintTableCell cell in Cells)
                if (cell.AutoSize)
                    cell.SetHeight(height);
        }
        public override void SetBounds(RectangleF bounds)
        {
            base.SetBounds(bounds);
            SetHeight(bounds.Height);
        }
        public override string ToString()
        {
            return $"PrintTableRow[{Index}]  | Cells = {Cells.Length}";
        }
        #endregion

        public PrintTableCell[] Cells => _Cells.ToArray();
        public bool IsDummy { get; set; }
        private List<PrintTableCell> _Cells;
    }
}

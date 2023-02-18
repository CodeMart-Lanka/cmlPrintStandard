using cmlPrint.TableStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmlPrint.Print
{
    public partial class cmlPrintDocument
    {
        public void Validate(PrintTable table)
        {
            // Columns Count
            for (int i = 0; i < table.Rows.Count; i++)
            {
                PrintTableRow row = table.Rows[i];
                if (row.Cells == null || row.Cells.Count() != table.Columns)
                    throw new Exception($"Row : ({row}) of Table : ({table}) " +
                        $"Contains Invalid Amount of Rows");
            }
        }
    }
}

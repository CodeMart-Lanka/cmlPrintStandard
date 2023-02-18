using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmlPrint.TableStructures
{
    public class PrintTableTextCell : PrintTableCell
    {
        public PrintTableTextCell()
        {
            Lines = new string[] { "" };
        }
        public override string ToString()
        {
            return $"PrintTableTextCell[{Index}]  | Text = {Text}";
        }
        public string[] Lines;
        public string Text { get; set; } = "";
        public Font Font { get; set; } = new Font("Calibri", 8);
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        public GraphicsState ApplyRotation(Graphics g, ref SizeF lineSize)
        {
            if(Rotation == Rotations.Horizontal) 
                return null;
            var state = g.Save();
            var dx = X - AbsolutePrintableArea.Top;
            var dy = Y + AbsolutePrintableArea.Left + lineSize.Height;
            if (ContentHorizontalAlign == HorizontalAlign.Center)
                dx += ((Width / 2) - (lineSize.Width / 2));
            if (ContentVerticalAlign == VerticalAlign.Center)
                dy += ((Height / 2) - (lineSize.Height / 2));
            g.TranslateTransform(dx, dy); 
            g.RotateTransform(-90);
            return state;
        }
        public string[] Lines;
        public string Text { get; set; } = "";
        public Font Font { get; set; } = new Font("Calibri", 8);
        public Rotations Rotation { get; set; } = 0; // 0, 90, 180, 270
    }
}

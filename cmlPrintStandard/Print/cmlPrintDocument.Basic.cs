using cmlPrint.TableStructures;
using cmlPrintStandard.Extension;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmlPrint.Print
{
    public partial class cmlPrintDocument
    {
        public PointF Add(PointF origin, float x, float y) => new PointF(origin.X + x, origin.Y + y);
        public void Resize(ref RectangleF rect, float top, float left,float bottom,float right)
        {
            rect.Y -= top;
            rect.Width += left;
            rect.Height += bottom;
            rect.X -= right;
        } 
        public void SetMargins(ref RectangleF rect,Margins margins)
        {
            Resize(ref rect, -margins.Top, -margins.Left, -margins.Bottom, -margins.Right);
        }
        #region Measuring
        public SizeF MeasureString(string s, Font f, Rotations rotation)
        {
            var size = Graphics.MeasureString(s, f);
            return rotation == Rotations.Vertical ? size.Flip() : size;
        }
        public SizeF MeasureString(string[] lines, Font f, Rotations rotation)
        {
            SizeF size = new SizeF();
            foreach (string line in lines)
            {
                SizeF temp = Graphics.MeasureString(line, f);
                size.Height += temp.Height;
                size.Width = Math.Max(temp.Width, size.Width);
            }
            return rotation == Rotations.Vertical ? size.Flip() : size;
        }
        #endregion
    }
}

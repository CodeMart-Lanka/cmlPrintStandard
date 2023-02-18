using cmlPrint.TableStructures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmlPrint.Print
{
    public partial class cmlPrintDocument
    {
        private void PrintCell(PrintTableCell cell)
        {
            if (cell == null)
                return;
            DrawBoarders(cell);
            if (cell is PrintTableTextCell)
                PrintTextCell(cell as PrintTableTextCell);

            if (cell is PrintTable)
                PrintTable(cell as PrintTable);

            if (cell is PrintTableImageCell)
                PrintImageCell(cell as PrintTableImageCell);

            if (cell is PrintTableContainerCell)
                PrintCell((cell as PrintTableContainerCell).Content);
        }
        private void PrintTextCell(PrintTableTextCell cell)
        {
            cell.ResetPosition(); // Set the Current Postion of the Cell To Upper Left Corner
            float paragraphHeight = MeasureString(cell.Lines, cell.Font).Height;
            if (cell.ContentVerticalAlign == VerticalAlign.Center) // Middle Align
                cell.Y = cell.AbsolutePrintableArea.Top + ((cell.AbsolutePrintableArea.Height - paragraphHeight) / 2f);
            if (cell.ContentVerticalAlign == VerticalAlign.Bottom) // End Align
                cell.Y = cell.AbsolutePrintableArea.Top + cell.AbsolutePrintableArea.Height - paragraphHeight;
            foreach (string line in cell.Lines)
            {
                SizeF lineSize = MeasureString(line, cell.Font);

                if (cell.ContentHorizontalAlign == HorizontalAlign.Center)
                    cell.X = cell.AbsolutePrintableArea.Left + ((cell.AbsolutePrintableArea.Width - lineSize.Width) / 2f);
                if (cell.ContentHorizontalAlign == HorizontalAlign.Right)
                    cell.X = cell.AbsolutePrintableArea.Left + (cell.AbsolutePrintableArea.Width - lineSize.Width);

                Graphics.DrawString(line, cell.Font, new SolidBrush(cell.ForeColor), cell.Pos);

                cell.Y += lineSize.Height; // Increase Y
                cell.X = cell.AbsolutePrintableArea.Left; // Reset X
            }
        }
        private void PrintImageCell(PrintTableImageCell cell)
        {
            cell.ResetPosition();
            if (cell.ContentVerticalAlign == VerticalAlign.Center) // Middle Align
                cell.Y = cell.RelativePrintableArea.Top + ((cell.RelativePrintableArea.Height - cell.ImageHeight) / 2f);
            if (cell.ContentVerticalAlign == VerticalAlign.Bottom) // End Align
                cell.Y = cell.RelativePrintableArea.Top + cell.RelativePrintableArea.Height - cell.ImageHeight;
            if (cell.ContentHorizontalAlign == HorizontalAlign.Center)
                cell.X = cell.RelativePrintableArea.Left + ((cell.RelativePrintableArea.Width - cell.ImageWidth) / 2f);
            if (cell.ContentHorizontalAlign == HorizontalAlign.Right)
                cell.X = cell.RelativePrintableArea.Left + (cell.RelativePrintableArea.Width - cell.ImageWidth);
            Graphics.DrawImage(cell.Image, cell.Pos);
        }
        private void PrintTable(PrintTable table)
        {
            foreach (PrintTableRow row in table.Rows)
            {
                PrintCell(row);
                foreach (PrintTableCell cell in row.Cells)
                {
                    PrintCell(cell);
                }
            }
        }
        private void DrawBoarders(PrintTableCell cell)
        {
            if(cell.TopBorderThickness > 0) //Top
                DrawLine(cell.TopBorderColor, cell.TopBorderThickness, cell.Bounds.Location, 
                    Add(cell.Bounds.Location, cell.Bounds.Width, 0), cell.BorderStyle);
            if(cell.LeftBorderThickness > 0) //Left
                DrawLine(cell.LeftBorderColor, cell.LeftBorderThickness, cell.Bounds.Location, 
                    Add(cell.Bounds.Location, 0, cell.Bounds.Height), cell.BorderStyle);
            if (cell.BottomBorderThickness > 0) //Bottom
                DrawLine(cell.BottomBorderColor, cell.BottomBorderThickness, Add(cell.Bounds.Location, cell.Bounds.Width, 
                    cell.Bounds.Height), Add(cell.Bounds.Location, 0, cell.Bounds.Height), cell.BorderStyle);
            if (cell.RightBorderThickness > 0) //Right
                DrawLine(cell.TopBorderColor, cell.RightBorderThickness, Add(cell.Bounds.Location, cell.Bounds.Width, 0),
                    Add(cell.Bounds.Location, cell.Bounds.Width, cell.Bounds.Height), cell.BorderStyle);
            // Reset Bounds
            //RectangleF rect = cell.RelativePrintableArea;
            //Resize(ref rect, -cell.TopBorderThickness, -cell.LeftBorderThickness, -cell.BottomBorderThickness, -cell.RightBorderThickness);
        }
        private void DrawLine(Color color,float thickness, PointF p1,PointF p2,DashStyle borderStyle)
        {
            Pen pen = new Pen(color, thickness);
            pen.DashStyle = borderStyle;
            Graphics.DrawLine(pen, p1, p2);
        }
    }
}

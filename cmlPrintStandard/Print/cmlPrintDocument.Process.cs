using cmlPrint.TableStructures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmlPrint.Print
{
    public partial class cmlPrintDocument
    {
        private void ProcessStructure(PrintTableCell cell)
        {
            ProcessCell(cell);
            if(cell is PrintTableContainerCell && (cell as PrintTableContainerCell).Content != null)
            {
                (cell as PrintTableContainerCell).Content.SetBounds(cell.AbsolutePrintableArea);
                ProcessStructure((cell as PrintTableContainerCell).Content);
                cell.Processed = true;
            }
        }
        private bool ProcessCell(PrintTableCell cell)
        {
            if (cell is PrintTableTextCell)
               return ProcessTextCell(cell as PrintTableTextCell);
            if (cell is PrintTableImageCell)
                return ProcessImageCell(cell as PrintTableImageCell);
            if (cell is PrintTable)
                return ProcessTable(cell as PrintTable);
            return true;
        }
        private bool ProcessTextCell(PrintTableTextCell cell)
        {
            cell.Lines = new string[] { cell.Text };
            if (cell.AutoSize)
                cell.SetHeight(cell.MinHeight); // Reset Cell Height
            for(int x = 0; x < cell.Lines.Length; x++)
            {
                int length = cell.Lines[x].Length;
                SizeF size = MeasureString(cell.Lines[x], cell.Font);
                while (size.Width >= cell.RelativePrintableArea.Width && length > 1) 
                {
                    length--;
                    size = MeasureString(cell.Text.Substring(0,length), cell.Font);
                }
                if(length != cell.Lines[x].Length) // Has to be Trimmed
                {
                    // Set Length to Appropriate Position for Break
                    int breakPoint = cell.Lines[x].Substring(0,length).LastIndexOfAny(new char[] { ' '});
                    if (breakPoint != -1)
                        length = breakPoint + 1;
                    if(cell.Lines[x].Length <= length)
                        break;
                    Array.Resize(ref cell.Lines, cell.Lines.Length + 1);
                    cell.Lines[x + 1] = cell.Lines[x].Substring(length);
                    cell.Lines[x] = cell.Lines[x].Substring(0,length);
                }
                if(PrintPageEventArgs.MarginBounds.Bottom <= cell.Bounds.Bottom + size.Height)
                {
                    return false;
                }
                else if (cell.AutoSize)
                    cell.SetHeight(cell.Bounds.Height + size.Height);
            }
            cell.Processed = true;
            return true;
        }
        private bool ProcessImageCell(PrintTableImageCell cell)
        {
            if (PrintPageEventArgs.MarginBounds.Bottom <= cell.ImageHeight + cell.MinHeight)
            {
                return false;
            }
            cell.SetHeight(cell.ImageHeight + cell.MinHeight);
            cell.Processed = true;
            return true;
        }
        private bool ProcessTable(PrintTable table)
        {
            Validate(table);
            table.Height = table.MinHeight;
            table.ResetPosition(); // Set Current Pos
            foreach (PrintTableRow row in table.Rows)
            {
                float maxHeght = 0;
                row.Top = table.Y;
                row.Left = table.X;
                row.Width = table.AbsolutePrintableArea.Width;
                float unitWidth = row.AbsolutePrintableArea.Width / table.TotalWeight;
                row.ResetPosition();

                // Calculate Row Height
                for (int i = 0; i < row.Cells.Length; i++)
                {
                    PrintTableCell cell = row[i];
                    cell.SetWidth(unitWidth * table.ColumnWeights[i]); // Set Width
                    cell.Left = row.X;
                    cell.Top = row.Y;
                    if (!ProcessCell(cell))
                        return false;
                    maxHeght = Math.Max(maxHeght, cell.Bounds.Height);
                    row.X += cell.Bounds.Width;
                }
                // Reset Row Sizes
                for (int i = 0; i < row.Cells.Length; i++)
                {
                    PrintTableCell cell = row[i];
                    cell.Height = maxHeght;
                }
                row.Height = row.MinHeight + maxHeght;
                table.Height += row.Height;
                row.X = table.RelativePrintableArea.Left;
                row.Y += maxHeght;
                table.Y += row.Height;
                row.Processed = true;
            }
            table.Processed = true;
            return true;
        }
    }
}

using cmlPrint.TableStructures;
using System;
using System.Drawing;

namespace cmlPrint.Print
{
    public partial class cmlPrintDocument
    {
        private bool ProcessStructure(PrintTableCell cell)
        {
            if (!ProcessCell(cell))
                return false;
            if(cell is PrintTableContainerCell containerCell && containerCell.Content != null)
            {
                containerCell.Content.SetBounds(cell.AbsolutePrintableArea);
                if (!ProcessStructure(containerCell.Content))
                    return false;
                cell.SetProcessingStatus(ProcessingStatuses.Done, Page);
            }
            return true;
        }
        private bool ProcessCell(PrintTableCell cell)
        {
            if (cell.ProcessingStatus == ProcessingStatuses.Done)
                return true;
            if (cell is PrintTableTextCell textCell)
               return ProcessTextCell(textCell);
            if (cell is PrintTableImageCell imageCell)
                return ProcessImageCell(imageCell);
            if (cell is PrintTable tableCell)
                return ProcessTable(tableCell);
            return true;
        }
        private bool ProcessTextCell(PrintTableTextCell cell)
        {
            cell.Lines = new string[] { cell.Text };
            if (cell.AutoSize)
                cell.SetHeight(cell.MinHeight); // Reset Cell Height
            for (int x = 0; x < cell.Lines.Length; x++)
            {
                string line = cell.Lines[x];
                int length = line.Length;
                SizeF size = MeasureString(line, cell.Font, cell.Rotation);
                while (size.Width >= cell.RelativePrintableArea.Width && length > 1) 
                {
                    length--;
                    size = MeasureString(line.Substring(0, length), cell.Font, cell.Rotation);
                }
                if (length != line.Length)
                {
                    int breakPoint = line.Substring(0, length).LastIndexOfAny(new char[] { ' ' });
                    if (breakPoint != -1)
                        length = breakPoint + 1;
                    if (cell.Overflow == TextCellOverflow.Truncate)
                    {
                        if (length < line.Length)
                            cell.Lines[x] = line.Substring(0, length);
                        size = MeasureString(cell.Lines[x], cell.Font, cell.Rotation);
                    }
                    else
                    {
                        if (line.Length <= length)
                            break;
                        Array.Resize(ref cell.Lines, cell.Lines.Length + 1);
                        cell.Lines[x + 1] = line.Substring(length);
                        cell.Lines[x] = line.Substring(0, length);
                        size = MeasureString(cell.Lines[x], cell.Font, cell.Rotation);
                    }
                }
                if(PaperType == PaperTypes.SingleSheet && PrintPageEventArgs.MarginBounds.Bottom <= cell.Bounds.Bottom + size.Height)
                {
                    return false;
                }
                else if (cell.AutoSize)
                    cell.SetHeight(cell.Bounds.Height + size.Height);
            }
            cell.SetProcessingStatus(ProcessingStatuses.Done, Page);
            return true;
        }
        private bool ProcessImageCell(PrintTableImageCell cell)
        {
            if (PaperType == PaperTypes.SingleSheet && PrintPageEventArgs.MarginBounds.Bottom <= cell.ImageHeight + cell.MinHeight)
            {
                return false;
            }
            cell.SetHeight(cell.ImageHeight + cell.MinHeight);
            cell.SetProcessingStatus(ProcessingStatuses.Done, Page);
            return true;
        }
        private bool ProcessTable(PrintTable table)
        {
            Validate(table);
            table.Height = table.MinHeight;
            table.ResetPosition(); // Set Current Pos
            ProcessingStatuses processingStatus = ProcessingStatuses.Done;
            foreach (PrintTableRow row in table.Rows)
            {
                if (row.ProcessingStatus != ProcessingStatuses.Done)
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
                        bool processed = ProcessCell(cell);
                        if (processed || table.AllowRowSplitting)
                        {
                            maxHeght = Math.Max(maxHeght, cell.Bounds.Height);
                            row.X += cell.Bounds.Width;
                        }
                        else
                        {
                            processingStatus = ProcessingStatuses.Pending;
                            row.ResetProcessingStatus();
                        }
                        if (!processed && table.AllowRowSplitting)
                            processingStatus = ProcessingStatuses.Partially;
                        if (!processed)
                            i = row.Cells.Length; // break loop
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
                    row.SetProcessingStatus(processingStatus, Page);
                    if (processingStatus != ProcessingStatuses.Done)
                        return false;
                }
            }
            table.SetProcessingStatus(ProcessingStatuses.Done, Page);
            return true;
        }
    }
}

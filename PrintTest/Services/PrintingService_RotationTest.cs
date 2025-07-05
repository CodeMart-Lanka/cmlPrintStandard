using cmlPrint.Print;
using cmlPrint.TableStructures;
using PrintTest.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cmlPrint.Print.cmlPrintDocument;

namespace PrintTest.Services
{
    internal class PrintingService_RotationTest
    {
        private PrintDocument Doc { get; }
        public int ItemTableRowCount { get; set; }
        private int ItemTableIndex { get; set; }
        public PrintingService_RotationTest(PrintDocument doc, Graphics dummyGraphics)
        {
            Doc = doc;
            if (dummyGraphics != null)
            {
                PrintTableContainerCell dummy = new PrintTableContainerCell();
                dummy.SetContent(GetContainerTable());
                PrintDocument dummyDoc = new PrintDocument();
                Rectangle rec = new Rectangle()
                {
                    X = doc.PrinterSettings.DefaultPageSettings.Margins.Left,
                    Y = doc.PrinterSettings.DefaultPageSettings.Margins.Top,
                    Width = doc.PrinterSettings.DefaultPageSettings.PaperSize.Width - doc.PrinterSettings.DefaultPageSettings.Margins.Left -
                        doc.PrinterSettings.DefaultPageSettings.Margins.Right,
                    Height = doc.PrinterSettings.DefaultPageSettings.PaperSize.Height - doc.PrinterSettings.DefaultPageSettings.Margins.Top -
                        doc.PrinterSettings.DefaultPageSettings.Margins.Bottom
                };
                PrintPageEventArgs e = new PrintPageEventArgs(dummyGraphics, rec, rec, doc.DefaultPageSettings);
                new cmlPrintDocument(dummyDoc, dummy, paperType: PaperTypes.SingleSheet).PreProcess(e);
                ItemTableRowCount = GetRowCountInItemTable(dummy);
            }
            PrintTableContainerCell page = new PrintTableContainerCell();
            page.SetContent(GetContainerTable());

            new cmlPrintDocument(Doc, page, paperType: PaperTypes.SingleSheet);
        }
        private PrintTableCell GetContainerTable()
        {
            PrintTable container = new PrintTable(2);
            container.AllowRowSplitting = true;
            PrintTableRow[] rows = new PrintTableRow[9];

            rows[0] = new PrintTableRow();
            rows[0].Add(GetTextCell());
            rows[0].Add(GetTextCell());
            
            rows[1] = new PrintTableRow();
            rows[1].Add(GetTextCell());
            rows[1].Add(GetTextCell());
            
            rows[2] = new PrintTableRow();
            rows[2].Add(GetTextCell());
            rows[2].Add(GetTextCell());
            
            rows[3] = new PrintTableRow();
            rows[3].Add(GetTextCell());
            rows[3].Add(GetTextCell());
            
            rows[4] = new PrintTableRow();
            rows[4].Add(GetTextCell());
            rows[4].Add(GetRotatedTextCell());
            
            rows[5] = new PrintTableRow();
            rows[5].Add(GetTextCell());
            rows[5].Add(GetRotatedTextCell());
            
            rows[6] = new PrintTableRow();
            rows[6].Add(GetTextCell());
            rows[6].Add(GetTextCell());
            
            rows[7] = new PrintTableRow();
            rows[7].Add(GetTextCell());
            rows[7].Add(GetRotatedTextCell());
            
            rows[8] = new PrintTableRow();
            rows[8].Add(GetTextCell());
            rows[8].Add(GetRotatedTextCell());

            container.Rows.AddRange(rows);

            return container;
        }
        private PrintTableCell GetTextCell()
        {
            return new PrintTableTextCell()
            {
                Text = "Sample Text",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ContentHorizontalAlign = HorizontalAlign.Center,
                ContentVerticalAlign = VerticalAlign.Center,
                AutoSize = true,
                BorderThickness = 1
            };
        }
        private PrintTableCell GetRotatedTextCell()
        {
            var table = new PrintTable(1);
            var rows = new PrintTableRow[1];

            rows[0] = new PrintTableRow()
            {
                ContentHorizontalAlign = HorizontalAlign.Center,
                ContentVerticalAlign = VerticalAlign.Center
            };
            rows[0].Add(new PrintTableTextCell()
            {
                Text = "Text",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ContentHorizontalAlign = HorizontalAlign.Center,
                BorderThickness = 1,
                Rotation = Rotations.Vertical,
            });
            table.Rows.AddRange(rows);
            return table;
        }
        private int GetRowCountInItemTable(PrintTableCell dummy)
        {
            int count = 0;
            PrintTable? table = dummy.GetCellByIndex(ItemTableIndex) as PrintTable;
            if (table != null)
            {
                foreach (PrintTableRow row in table.Rows)
                {
                    if (row[1] is PrintTableTextCell)
                    {
                        count += ((row[1] as PrintTableTextCell)?.Lines?.Length ?? 0);
                    }
                }
            }
            return count;
        }
    }
}

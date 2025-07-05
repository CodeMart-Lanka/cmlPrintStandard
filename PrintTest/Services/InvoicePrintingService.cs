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
    internal class InvoicePrintingService
    {
        private PrintDocument Doc { get; }
        private List<Invoice> Invoices { get; }
        public int ItemTableRowCount { get; set; }
        private int ItemTableIndex { get; set; }
        public InvoicePrintingService(PrintDocument doc, List<Invoice> invoices, Graphics dummyGraphics)
        {
            Doc = doc;
            Invoices = invoices;
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

        #region Container Table
        private PrintTable GetContainerTable()
        {
            PrintTable container = new PrintTable(1);
            container.AllowRowSplitting = true;
            PrintTableRow[] rows = new PrintTableRow[1];

            // Header
            rows[0] = new PrintTableRow();
            rows[0].Add(GetInvoiceTable());

            container.Rows.AddRange(rows);

            return container;
        }
        #endregion

        #region Invioce table
        private PrintTable GetInvoiceTable()
        {
            PrintTable table = new PrintTable(5, new float[] { 15, 15, 15, 40, 15 });
            ItemTableIndex = table.Index;
            Invoices.ForEach(invoice => table.Rows.Add(GetInvoiceRow(invoice)));
            table.Rows.Add(GetFooterRow());
            return table;
        }
        private PrintTableRow GetInvoiceRow(Invoice invoice)
        {
            var row = new PrintTableRow();
            row.Add(new PrintTableTextCell()
            {
                Text = invoice.Id.ToString(),
                BorderThickness = 1
            });
            row.Add(new PrintTableTextCell()
            {
                Text = invoice.DateTime.ToString(),
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                RightBorderThickness = 1
            });
            row.Add(new PrintTableTextCell()
            {
                Text = invoice.Total.ToString(),
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                RightBorderThickness = 1
            });
            row.Add(new PrintTableTextCell()
            {
                Text = invoice.Description?.ToString(),
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                RightBorderThickness = 1
            });
            row.Add(GetInvoiceLineTable(invoice.Lines));
            return row;
        }
        public PrintTableRow GetFooterRow()
        {
            var row = new PrintTableRow();
            row.Add(GetFooterText(-90));
            row.Add(GetFooterText(0));
            row.Add(new PrintTableTextCell());
            row.Add(new PrintTableTextCell());
            row.Add(new PrintTableTextCell());
            return row;
        }
        public PrintTableCell GetFooterText(int rotation)
        {
            var cell = new PrintTableTextCell()
            {
                Text = "Footer Text",
                BorderThickness = 1,
            };
            return cell;
        }
        public PrintTable GetInvoiceLineTable(List<string> lines)
        {
            var table = new PrintTable(1);
            table.AllowRowSplitting = true;
            table.TopBorderThickness = 1;
            table.RightBorderThickness = 1;
            table.BottomBorderThickness = 1;
            
            lines.ForEach(line =>
            {
                var row = new PrintTableRow();
                row.Add(new PrintTableTextCell() { Text = line });
                table.Rows.Add(row);
            });

            return table;
        }
        #endregion

        #region Dummy
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
        #endregion
    }
}

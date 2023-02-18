using cmlPrint.Print;
using cmlPrint.TableStructures;
using PrintTest.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintTest.Services
{
    internal class InvoicePrintingService
    {
        /*private A5Printer A5P;
        private Settings PS = Data.PS;*/
        private PrintDocument Doc { get; }
        private List<Invoice> Invoices { get; }
        //public InvoicePrintingData PrintingData { get; }
        public int ItemTableRowCount { get; set; }
        private int ItemTableIndex { get; set; }
        public InvoicePrintingService(PrintDocument doc, List<Invoice> invoices, Graphics dummyGraphics)
        {
            //A5P = PS.A5P;
            Doc = doc;
            Invoices = invoices;
            //PrintingData = printingData;
            if (dummyGraphics != null)
            {
                PrintTableContainerCell dummy = new PrintTableContainerCell();
                dummy.SetContent(GetContainerTable());
                PrintDocument dummyDoc = new PrintDocument();
                // dummyDoc.PrinterSettings = doc.PrinterSettings;
                // dummyDoc.DefaultPageSettings = doc.DefaultPageSettings;
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
                new cmlPrintDocument(dummyDoc, dummy).PreProcess(e);
                ItemTableRowCount = GetRowCountInItemTable(dummy);
            }
            PrintTableContainerCell page = new PrintTableContainerCell();
            page.SetContent(GetContainerTable());

            new cmlPrintDocument(Doc, page);
            //new cmlPrintDocument(Doc, page);
        }

        #region Container Table
        private PrintTable GetContainerTable()
        {
            PrintTable container = new PrintTable(1);
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
            PrintTable table = new PrintTable(4, new float[] { 15, 15, 15, 55 });
            ItemTableIndex = table.Index;
            Invoices.ForEach(invoice => table.Rows.Add(GetInvoiceRow(invoice)));

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
            return row;
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

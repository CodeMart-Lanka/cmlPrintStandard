using PrintTest.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintTest.Services
{
    internal class InvoicePrinter
    {
        public Graphics DummyGraphics { get; }
        private PrintDocument PrintSetupSource { get; }

        public InvoicePrinter(Graphics dummyGraphics, PrintDocument printSetupSource)
        {
            DummyGraphics = dummyGraphics;
            PrintSetupSource = printSetupSource;
        }

        public void ApplySharedPrintSetup(PrintDocument doc)
        {
            doc.PrinterSettings = (PrinterSettings)PrintSetupSource.PrinterSettings.Clone();
            doc.DefaultPageSettings = (PageSettings)PrintSetupSource.DefaultPageSettings.Clone();
        }

        public void DisplayPrintPrevieDialog(List<Invoice> invoices)
        {
            var doc = new PrintDocument();
            ApplySharedPrintSetup(doc);
            var invoicePrintingService = new InvoicePrintingService(doc, invoices, DummyGraphics);   
            var previewDialog = new PrintPreviewDialog();
            previewDialog.Document = doc;
            previewDialog.ShowDialog();
        }
        public void PrintToPreviewControl(List<Invoice> invoices, PrintPreviewControl control)
        {
            var doc = new PrintDocument();
            ApplySharedPrintSetup(doc);
            var invoicePrintingService = new InvoicePrintingService(doc, invoices, DummyGraphics);
            control.Document = doc;
        }
        public void PrintRotatedTextOnPreviewDialog()
        {
            var doc = new PrintDocument();
            ApplySharedPrintSetup(doc);
            var invoicePrintingService = new PrintingService_RotationTest(doc, DummyGraphics);
            var previewDialog = new PrintPreviewDialog();
            previewDialog.Document = doc;
            previewDialog.ShowDialog();
        }
        public void PrintRotatedTextOnPreviewControl(PrintPreviewControl control)
        {
            var doc = new PrintDocument();
            ApplySharedPrintSetup(doc);
            var invoicePrintingService = new PrintingService_RotationTest(doc, DummyGraphics);
            control.Document = doc;
        }
    }
}

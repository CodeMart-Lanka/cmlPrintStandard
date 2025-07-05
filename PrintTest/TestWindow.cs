using PrintTest.Data;
using PrintTest.Models;
using PrintTest.Services;
using System.Drawing.Printing;

namespace PrintTest
{
    public partial class TestWindow : Form
    {
        InvoiceGenerator invoiceGenerator { get; }
        InvoicePrinter InvoicePrinter { get; }
        public TestWindow()
        {
            InitializeComponent();
            invoiceGenerator = new InvoiceGenerator();
            InvoicePrinter = new InvoicePrinter(dummyPanel.CreateGraphics());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var invoices = invoiceGenerator.GetInvoices(int.Parse(textBox1.Text));
            InvoicePrinter.PrintToPreviewControl(invoices, printPreviewControl1);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var invoices = invoiceGenerator.GetInvoices(int.Parse(textBox1.Text));
            InvoicePrinter.DisplayPrintPrevieDialog(invoices);
        }

        private void btn_TestPrint_Click(object sender, EventArgs _e)
        {
            var doc = new PrintDocument();
            //var previewDialog = new PrintPreviewDialog();
            //previewDialog.Document = doc;
            printPreviewControl1.Document = doc;

            doc.PrintPage += (s, e) =>
            {
                string text = "Vertical Text Example";
                Font font = new Font("Arial", 16);
                Graphics g = e.Graphics;

                // Measure string size
                SizeF size = g.MeasureString(text, font);

                // Swap width/height due to -90 rotation
                //float rotatedWidth = size.Height;
                //float rotatedHeight = size.Width;

                // Let's say we want to print at top-left margin (adjust as needed)
                //float x = e.MarginBounds.Left;
                //float y = e.MarginBounds.Top + rotatedHeight;

                var state = g.Save();

                // Translate and rotate
                g.TranslateTransform(100, 100);
                g.RotateTransform(180);

                // Draw
                g.DrawString(text, font, Brushes.Black, 0, 0);

                //g.Restore(state);
            };

            //previewDialog.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //InvoicePrinter.PrintRotatedTextOnPreviewDialog();
            InvoicePrinter.PrintRotatedTextOnPreviewControl(printPreviewControl1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button3_Click(sender, e);
        }
    }
}
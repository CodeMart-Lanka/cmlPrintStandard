using PrintTest.Data;
using PrintTest.Services;

namespace PrintTest
{
    public partial class Form1 : Form
    {
        InvoiceGenerator invoiceGenerator { get; }
        InvoicePrinter InvoicePrinter { get; }
        public Form1()
        {
            InitializeComponent();
            invoiceGenerator= new InvoiceGenerator();
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
    }
}
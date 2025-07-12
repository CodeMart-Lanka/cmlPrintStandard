using cmlPrint.TableStructures;
using System.Drawing;
using System.Drawing.Printing;

namespace cmlPrint.Print
{
    public partial class cmlPrintDocument
    {
        public cmlPrintDocument(PrintDocument document, PrintTableCell cell, PrintTableCell waterMark = null, PaperTypes paperType = default)
        {
            Document = document;
            Container = cell;
            WaterMark = waterMark;
            PaperType = paperType;
            Document.PrintPage += OnPrinting;
            Document.BeginPrint += BeginPrint;
            Document.EndPrint += EndPrint;
        }
        private void OnPrinting(object sender, PrintPageEventArgs e)
        {
            lock (this)
            {
                Page++;
                PrintPageEventArgs = e;
                Graphics = e.Graphics;
                Container.SetBounds(PrintableArea);
                if (WaterMark != null)
                    ProcessStructure(WaterMark);
                e.HasMorePages = !ProcessStructure(Container);
                PrintCell(WaterMark);
                PrintCell(Container);
            }
        }
        public void PreProcess(PrintPageEventArgs e)
        {
            lock (this)
            {
                Page++;
                IsPreProcessing = true;
                PrintPageEventArgs = e;
                Graphics = e.Graphics;
                Container.SetBounds(PrintableArea);
                if (WaterMark != null)
                    ProcessStructure(WaterMark);
                ProcessStructure(Container);
            }
        }
        public Bitmap ExportToBitmap(PrintPageEventArgs e)
        {
            lock (this)
            {
                Page++;
                PrintPageEventArgs = e;
                Bitmap bitmap = new Bitmap(e.PageBounds.Width, e.PageBounds.Height);
                Graphics = Graphics.FromImage(bitmap);
                Graphics.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);

                Container.SetBounds(PrintableArea);
                if (WaterMark != null)
                    ProcessStructure(WaterMark);
                ProcessStructure(Container);
                PrintCell(WaterMark);
                PrintCell(Container);
                return bitmap;
            }
        }
        private void BeginPrint(object sender, PrintEventArgs e)
        {
            
        }
        private void EndPrint(object sender, PrintEventArgs e)
        {

        }
        public PrintDocument Document { get; }
        public PrintTableCell Container { get; }
        public PrintTableCell WaterMark { get; }
        public RectangleF PrintableArea => PrintPageEventArgs.MarginBounds;
        public PrintPageEventArgs PrintPageEventArgs { get; private set; }
        public Graphics Graphics { get; private set; }
        public int Page { get; private set; } = 0;
        public bool IsPreProcessing { get; private set; }
        public PaperTypes PaperType { get; private set; }
        public enum PaperTypes { ContinuousPaper, SingleSheet}
    }
}

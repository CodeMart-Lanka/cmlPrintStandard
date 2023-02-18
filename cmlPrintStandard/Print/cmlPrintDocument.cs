using cmlPrint.TableStructures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmlPrint.Print
{
    public partial class cmlPrintDocument
    {
        public cmlPrintDocument(PrintDocument document, PrintTableCell cell, PrintTableCell waterMark = null)
        {
            Document = document;
            Page = cell;
            WaterMark = waterMark;
            Document.PrintPage += OnPrinting;
            Document.BeginPrint += BeginPrint;
            Document.EndPrint += EndPrint;
        }
        private void OnPrinting(object sender, PrintPageEventArgs e)
        {
            PrintPageEventArgs = e;
            Graphics = e.Graphics;
            Page.SetBounds(PrintableArea);
            if (PageNumber == 1)
            {
                if (WaterMark != null)
                    ProcessStructure(WaterMark);
                ProcessStructure(Page);
            }
            PrintCell(WaterMark);
            PrintCell(Page);
        }
        public void PreProcess(PrintPageEventArgs e)
        {
            PrintPageEventArgs = e;
            Graphics = e.Graphics;
            Page.SetBounds(PrintableArea);
            if (PageNumber == 1)
            {
                if (WaterMark != null)
                    ProcessStructure(WaterMark);
                ProcessStructure(Page);
            }
        }
        public Bitmap ExportToBitmap(PrintPageEventArgs e)
        {
            PrintPageEventArgs = e;
            Bitmap bitmap = new Bitmap(e.PageBounds.Width, e.PageBounds.Height);
            Graphics = Graphics.FromImage(bitmap);
            Graphics.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);

            Page.SetBounds(PrintableArea);
            if (PageNumber == 1)
            {
                if (WaterMark != null)
                    ProcessStructure(WaterMark);
                ProcessStructure(Page);
            }
            PrintCell(WaterMark);
            PrintCell(Page);
            return bitmap;
        }
        private void BeginPrint(object sender, PrintEventArgs e)
        {
            
        }
        private void EndPrint(object sender, PrintEventArgs e)
        {

        }
        public PrintDocument Document { get; }
        public PrintTableCell Page { get; }
        public PrintTableCell WaterMark { get; }
        public RectangleF PrintableArea => PrintPageEventArgs.MarginBounds;
        public PrintPageEventArgs PrintPageEventArgs { get; private set; }
        public Graphics Graphics { get; private set; }
        public int PageNumber { get; private set; } = 1;
    }
}

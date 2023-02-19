using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmlPrint.TableStructures
{
    public class PrintTableImageCell : PrintTableCell
    {
        public PrintTableImageCell(Image image)
        {
            Image = image;
        }
        public PrintTableImageCell(Image image, float height,float width, float resolution)
        {
            ImageWidth = width;
            ImageHeight = height;

            width *= (resolution / 96);
            height *= (resolution / 96);
            if (image == null)
                image = new Bitmap((int)width, (int)height);
            Bitmap bitmap = new Bitmap(image, (int)width, (int)height);
            bitmap.SetResolution(resolution, resolution);
            Image = bitmap;
            
        }
        public override string ToString()
        {
            return $"ImageCell[{Index}]  | Width = {ImageWidth}, Height = {ImageHeight}";
        }
        public float ImageWidth { get; set; }
        public float ImageHeight { get; set; }
        public Image Image { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace cmlPrintStandard.Extension
{
    public static class SizeFExtensions
    {
        public static SizeF Flip(this SizeF size)
        {
            return new SizeF(size.Height, size.Width);
        }
    }
}

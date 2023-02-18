using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmlPrint.TableStructures
{
    public abstract class PrintTableCell
    {
        public PrintTableCell()
        {
            Margins = new Margins(0, 0, 0, 0);
            Paddings = new Margins(0, 0, 0, 0);
            Pos = new PointF();
            Index = Counter++;
        }
        #region Grouped
        public int BorderThickness
        {
            get
            {
                if (TopBorderThickness != RightBorderThickness)
                    return -1;
                if (TopBorderThickness != BottomBorderThickness)
                    return -1;
                if (TopBorderThickness != LeftBorderThickness)
                    return -1;
                return TopBorderThickness;
            }
            set
            {
                LeftBorderThickness = value;
                BottomBorderThickness = value;
                RightBorderThickness = value;
                TopBorderThickness = value;
            }
        }
        public Color BorderColor
        {
            get
            {
                if (TopBorderColor != RightBorderColor)
                    return Color.Empty;
                if (TopBorderColor != BottomBorderColor)
                    return Color.Empty;
                if (TopBorderColor != LeftBorderColor)
                    return Color.Empty;
                return TopBorderColor;
            }
            set
            {
                LeftBorderColor = value;
                BottomBorderColor = value;
                RightBorderColor = value;
                TopBorderColor = value;
            }
        }
        public Margins Margins 
        {
            get => new Margins(MarginLeft, MarginRight, MarginTop, MarginBottom);
            set
            {
                MarginLeft = value.Left;
                MarginRight = value.Right;
                MarginTop = value.Top;
                MarginBottom = value.Bottom;
            }
        }
        public Margins Paddings 
        {
            get => new Margins(PaddingLeft, PaddingRight, PaddingTop, PaddingBottom);
            set
            {
                PaddingLeft = value.Left;
                PaddingRight = value.Right;
                PaddingTop = value.Top;
                PaddingBottom = value.Bottom;
            }
        }
        public RectangleF Bounds
        {
            get => new RectangleF(Left, Top, Width, Height);
            set
            {
                Left = value.Left;
                Top = value.Top;
                Width = value.Width;
                Height = value.Height;
            }
        }
        public PointF Pos
        {
            get => new PointF(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        #endregion

        #region Get
        public RectangleF AbsolutePrintableArea
        {
            get
            {
                return new RectangleF(
                    Bounds.Left + TopBorderThickness + Margins.Left + Paddings.Left,
                    Bounds.Top + LeftBorderThickness + Margins.Top + Paddings.Top,
                    Bounds.Width - LeftBorderThickness - RightBorderThickness -
                        Margins.Left - Margins.Right - Paddings.Left - Paddings.Right,
                    Bounds.Height - TopBorderThickness - BottomBorderThickness -
                        Margins.Top - Margins.Bottom - Paddings.Top - Paddings.Bottom
                );
            }
        }
        public RectangleF RelativePrintableArea
        {
            get
            {
                return new RectangleF(
                    TopBorderThickness + Margins.Left + Paddings.Left,
                    LeftBorderThickness + Margins.Top + Paddings.Top,
                    Bounds.Width - LeftBorderThickness - RightBorderThickness -
                        Margins.Left - Margins.Right - Paddings.Left - Paddings.Right,
                    Bounds.Height - TopBorderThickness - BottomBorderThickness -
                        Margins.Top - Margins.Bottom - Paddings.Top - Paddings.Bottom
                );
            }
        }
        public float MinHeight => TopBorderThickness + BottomBorderThickness + Margins.Top + Margins.Bottom + Paddings.Top + Paddings.Bottom;
        #endregion

        #region Functions
        public void ResetPosition()
        {
            X = AbsolutePrintableArea.Left;
            Y = AbsolutePrintableArea.Top;
        }
        
        public virtual void SetHeight(float height)
        {
            Height = height;
        }
        public virtual void SetWidth(float width)
        {
            Width = width;
        }
        public virtual void SetBounds(RectangleF bounds)
        {
            this.Bounds = bounds;
        }
        public override string ToString()
        {
            return $"PrintTableCell[{Index}] ";
        }
        public PrintTableCell GetCellByIndex(int index)
        {
            PrintTableCell cell = null;
            if (Index == index)
                return this;
            if (this is PrintTableContainerCell && (this as PrintTableContainerCell).Content != null)
                if ((cell = (this as PrintTableContainerCell).Content.GetCellByIndex(index)) != null)
                    return cell;
            if(this is PrintTable)
            {
                PrintTable table = this as PrintTable;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    PrintTableRow row = table.Rows[i];
                    for (int j = 0; j < row.Cells.Length; j++)
                    {
                        if ((cell = row.Cells[j].GetCellByIndex(index)) != null)
                            return cell;
                    }
                }
            }
            return null;
        }
        #endregion

        #region Margins
        public int MarginTop { get; set; }
        public int MarginRight { get; set; }
        public int MarginBottom { get; set; }
        public int MarginLeft { get; set; }
        #endregion

        #region Paddingss
        public int PaddingLeft { get; set; }
        public int PaddingRight { get; set; }
        public int PaddingTop { get; set; }
        public int PaddingBottom { get; set; }
        #endregion

        #region BorderThickness
        public int TopBorderThickness { get; set; }
        public int RightBorderThickness { get; set; }
        public int BottomBorderThickness { get; set; }
        public int LeftBorderThickness { get; set; }
        #endregion

        #region Border Colors
        public Color TopBorderColor { get; set; } = Color.Black;
        public Color RightBorderColor { get; set; } = Color.Black;
        public Color BottomBorderColor { get; set; } = Color.Black;
        public Color LeftBorderColor { get; set; } = Color.Black;
        #endregion

        #region Bounds
        public float Top { get; set; }
        public float Left { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        #endregion

        #region Position
        public float X { get; set; }
        public float Y { get; set; }
        #endregion

        public HorizontalAlign ContentHorizontalAlign { get; set; }
        public VerticalAlign ContentVerticalAlign { get; set; }
        public DashStyle BorderStyle { get; set; }
        public int RowSpan { get; set; }
        public int ColSpan { get; set; }
       // public PrintTableRow Row { get; set; }
        public Color ForeColor { get; set; } = Color.Black;
        public int PageNumber { get; set; } = 1;
        public bool AutoSize { get; set; } = true;
        public int Index { get; set; }
        public bool Processed { get; set; }   
        public bool Printed { get; set; }   
        public static int Counter { get; set; }
    }
    public enum VerticalAlign { Top, Center, Bottom}
    public enum HorizontalAlign { Left, Center, Right}
    public enum BorderStyle { Line, DashedLine }
}

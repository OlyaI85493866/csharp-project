using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FamilyDocumentsWinForms.Controls
{
    public class RoundedPanel : Panel
    {
        private int cornerRadius = 18;
        private Color borderColor = Color.FromArgb(225, 230, 235);
        private int borderThickness = 1;

        [DefaultValue(18)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int CornerRadius
        {
            get => cornerRadius;
            set
            {
                cornerRadius = value;
                Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color BorderColor
        {
            get => borderColor;
            set
            {
                borderColor = value;
                Invalidate();
            }
        }

        [DefaultValue(1)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderThickness
        {
            get => borderThickness;
            set
            {
                borderThickness = value;
                Invalidate();
            }
        }

        public RoundedPanel()
        {
            BackColor = Color.White;
            DoubleBuffered = true;
            Padding = new Padding(10);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle panelRectangle = new Rectangle(
                0,
                0,
                Width - 1,
                Height - 1
            );

            using GraphicsPath path = GetRoundedRectanglePath(panelRectangle, CornerRadius);

            Region = new Region(path);

            using SolidBrush brush = new SolidBrush(BackColor);
            e.Graphics.FillPath(brush, path);

            using Pen borderPen = new Pen(BorderColor, BorderThickness);
            e.Graphics.DrawPath(borderPen, path);
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            Invalidate();
        }

        private GraphicsPath GetRoundedRectanglePath(Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            int diameter = radius * 2;

            path.AddArc(rectangle.X, rectangle.Y, diameter, diameter, 180, 90);
            path.AddArc(rectangle.Right - diameter, rectangle.Y, diameter, diameter, 270, 90);
            path.AddArc(rectangle.Right - diameter, rectangle.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rectangle.X, rectangle.Bottom - diameter, diameter, diameter, 90, 90);

            path.CloseFigure();

            return path;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scheduling_Jh
{
    public class DropShadowPanel : Panel
    {
        protected override void OnControlAdded(ControlEventArgs e)
        {
            e.Control.Paint += new PaintEventHandler(Control_Paint);
            base.OnControlAdded(e);
        }

        void Control_Paint(object sender, PaintEventArgs e)
        {
            CheckDrawInnerShadow(sender as Control, e.Graphics);
        }

        private void CheckDrawInnerShadow(Control sender, Graphics g)
        {
            var dropShadowStruct = GetDropShadowStruct(sender);

            if (dropShadowStruct == null || !dropShadowStruct.Inset)
            {
                return;
            }

            DrawInsetShadow(sender as Control, g);

        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            e.Control.Paint -= new PaintEventHandler(Control_Paint);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawShadow(Controls.OfType<Control>().Where(c => c.Tag != null && c.Tag.ToString().StartsWith("DropShadow")), e.Graphics);
        }

        void DrawInsetShadow(Control control, Graphics g)
        {
            var dropShadowStruct = GetDropShadowStruct(control);

            var rInner = new Rectangle(Point.Empty, control.Size);

            var img = new Bitmap(rInner.Width, rInner.Height, g);
            var g2 = Graphics.FromImage(img);

            g2.CompositingMode = CompositingMode.SourceCopy;
            g2.FillRectangle(new SolidBrush(dropShadowStruct.Color), 0, 0, control.Width, control.Height);

            rInner.Offset(dropShadowStruct.HShadow, dropShadowStruct.VShadow);
            rInner.Inflate(dropShadowStruct.Blur, dropShadowStruct.Blur);
            rInner.Inflate(-dropShadowStruct.Spread, -dropShadowStruct.Spread);

            double blurSize = dropShadowStruct.Blur;
            double blurStartSize = blurSize;

            do
            {
                var transparency = blurSize / blurStartSize;
                var color = Color.FromArgb(((int)(255 * (transparency * transparency))), dropShadowStruct.Color);
                rInner.Inflate(-1, -1);
                DrawRoundedRectangle(g2, rInner, (int)blurSize, Pens.Transparent, color);
                blurSize--;
            } while (blurSize > 0);

            g.DrawImage(img, 0, 0);
            g.Flush();

            g2.Dispose();
            img.Dispose();
        }

        void DrawShadow(IEnumerable<Control> controls, Graphics g)
        {
            foreach (var control in controls)
            {
                var dropShadowStruct = GetDropShadowStruct(control);

                if (dropShadowStruct.Inset)
                {
                    continue; // must be handled by the control itself
                }

                DrawOutsetShadow(g, dropShadowStruct, control);
            }
        }

        // drawing the loop on an image because of speed
        private void DrawOutsetShadow(Graphics g, dynamic dropShadowStruct, Control control)
        {
            var rOuter = control.Bounds;
            var rInner = control.Bounds;
            rInner.Offset(dropShadowStruct.HShadow, dropShadowStruct.VShadow);
            rInner.Inflate(-dropShadowStruct.Blur, -dropShadowStruct.Blur);
            rOuter.Inflate(dropShadowStruct.Spread, dropShadowStruct.Spread);
            rOuter.Offset(dropShadowStruct.HShadow, dropShadowStruct.VShadow);
            var originalOuter = rOuter;

            var img = new Bitmap(originalOuter.Width, originalOuter.Height, g);
            var g2 = Graphics.FromImage(img);

            var currentBlur = 0;

            do
            {
                var transparency = (rOuter.Height - rInner.Height) / (double)(dropShadowStruct.Blur * 2 + dropShadowStruct.Spread * 2);
                var color = Color.FromArgb(((int)(255 * (transparency * transparency))), dropShadowStruct.Color);
                var rOutput = rInner;
                rOutput.Offset(-originalOuter.Left, -originalOuter.Top);
                DrawRoundedRectangle(g2, rOutput, currentBlur, Pens.Transparent, color);
                rInner.Inflate(1, 1);
                currentBlur = (int)((double)dropShadowStruct.Blur * (1 - (transparency * transparency)));
            } while (rOuter.Contains(rInner));

            g2.Flush();
            g2.Dispose();

            g.DrawImage(img, originalOuter);

            img.Dispose();
        }

        private static dynamic GetDropShadowStruct(Control control)
        {
            if (control.Tag == null || !(control.Tag is string) || !control.Tag.ToString().StartsWith("DropShadow"))
                return null;

            string[] dropShadowParams = control.Tag.ToString().Split(':')[1].Split(',');
            var dropShadowStruct = new
            {
                HShadow = Convert.ToInt32(dropShadowParams[0]),
                VShadow = Convert.ToInt32(dropShadowParams[1]),
                Blur = Convert.ToInt32(dropShadowParams[2]),
                Spread = Convert.ToInt32(dropShadowParams[3]),
                Color = ColorTranslator.FromHtml(dropShadowParams[4]),
                Inset = dropShadowParams[5].ToLowerInvariant() == "inset"
            };
            Console.WriteLine(dropShadowStruct.HShadow + "," + dropShadowStruct.VShadow);
            return dropShadowStruct;
        }

        private void DrawRoundedRectangle(Graphics gfx, Rectangle bounds, int cornerRadius, Pen drawPen, Color fillColor)
        {
            int strokeOffset = Convert.ToInt32(Math.Ceiling(drawPen.Width));
            bounds = Rectangle.Inflate(bounds, -strokeOffset, -strokeOffset);

            var gfxPath = new GraphicsPath();
            if (cornerRadius > 0)
            {
                gfxPath.AddArc(bounds.X, bounds.Y, cornerRadius, cornerRadius, 180, 90);
                gfxPath.AddArc(bounds.X + bounds.Width - cornerRadius, bounds.Y, cornerRadius, cornerRadius, 270, 90);
                gfxPath.AddArc(bounds.X + bounds.Width - cornerRadius, bounds.Y + bounds.Height - cornerRadius, cornerRadius,
                               cornerRadius, 0, 90);
                gfxPath.AddArc(bounds.X, bounds.Y + bounds.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            }
            else
            {
                gfxPath.AddRectangle(bounds);
            }
            gfxPath.CloseAllFigures();

            gfx.FillPath(new SolidBrush(fillColor), gfxPath);
            if (drawPen != Pens.Transparent)
            {
                var pen = new Pen(drawPen.Color);
                pen.EndCap = pen.StartCap = LineCap.Round;
                gfx.DrawPath(pen, gfxPath);
            }
        }
    }
}

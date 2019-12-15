using System.Drawing;

namespace TetrisLogic
{
    /// <summary>
    /// Panel
    /// </summary>
    public class Panel
    {
        /// <summary>
        /// Panel title
        /// </summary>
        private readonly string text;

        /// <summary>
        /// Panel position
        /// </summary>
        public PointF Posistion { set; get; }

        /// <summary>
        /// Panel size
        /// </summary>
        public SizeF Size { set; get; }

        /// <summary>
        /// Constructor class panel
        /// </summary>
        /// <param name="parText">Panel title</param>
        public Panel(string parText)
        {
            text = parText;
        }

        /// <summary>
        /// Setting the position and size of the panel
        /// </summary>
        /// <param name="parPos">Panel position</param>
        /// <param name="parSize">Panel size</param>
        public void SetBounds(PointF parPos, SizeF parSize)
        {
            Posistion = parPos;
            Size = parSize;
        }

        /// <summary>
        /// Panel update function
        /// </summary>
        /// <param name="graphics">Graphics</param>
        /// <param name="parCenter">Center element or not</param>
        public void Update(Graphics graphics, bool parCenter)
        {
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(180, 0, 0, 0)), Posistion.X, Posistion.Y, Size.Width, Size.Height);
            graphics.DrawRectangle(new Pen(Color.White, 2), Posistion.X, Posistion.Y, Size.Width, Size.Height);
            graphics.DrawString(text, new Font(FontFamily.GenericSansSerif, Size.Width / 12.0f / 2),
             Brushes.White, new RectangleF(Posistion.X, Posistion.Y + 5, Size.Width, Size.Height),
             new StringFormat { Alignment = StringAlignment.Center, LineAlignment = parCenter ? StringAlignment.Center : StringAlignment.Near });

        }

    }
}
using System.Drawing;

namespace TetrisLogic
{
    /// <summary>
    /// Entry field
    /// </summary>
    public class Input
    {
        /// <summary>
        /// Entry field position
        /// </summary>
        public PointF Position { set; get; }

        /// <summary>
        /// Entry field text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Entry field size
        /// </summary>
        public SizeF Size { get; set; }

        /// <summary>
        /// Entry field font size
        /// </summary>
        private const float FONT_SIZE = 20.0f;

        /// <summary>
        /// Constructor class entry field
        /// </summary>
        public Input()
        {
            Text = "";
        }

        /// <summary>
        /// Setting the position and size of the entry field
        /// </summary>
        /// <param name="parPosition">Entry field position</param>
        /// <param name="parSize">Entry field size</param>
        public void SetBounds(PointF parPosition, SizeF parSize)
        {
            Position = parPosition;
            Size = parSize;
        }

        /// <summary>
        /// Enter character
        /// </summary>
        /// <param name="parKey">Character</param>
        public void Push(char parKey)
        {
            if (Text.Length < 8)
                if ((parKey >= 'a' && parKey <= 'z') ||
                  (parKey >= 'A' && parKey <= 'Z'))
                    Text += parKey;
        }

        /// <summary>
        /// Erase a single character
        /// </summary>
        public void Backspace()
        {
            if (Text.Length > 0)
                Text = Text.Substring(0, Text.Length - 1);
        }

        /// <summary>
        /// Input field update
        /// </summary>
        /// <param name="refGraphics">Graphics</param>
        /// <param name="parGraphicsSize">Graphics size</param>
        public void Update(Graphics refGraphics, SizeF parGraphicsSize)
        {
            refGraphics.DrawRectangle(new Pen(Color.Black, 2), Position.X, Position.Y, Size.Width, Size.Height);
            refGraphics.FillRectangle(Brushes.White, Position.X + 1, Position.Y + 1, Size.Width - 2, Size.Height - 2);
            refGraphics.DrawString(Text, new Font("Fedra Sans", parGraphicsSize.Width / FONT_SIZE / 2),
                  Brushes.Black, new RectangleF(Position.X, Position.Y, Size.Width, Size.Height),
                  new StringFormat { LineAlignment = StringAlignment.Center });
        }
    }
}

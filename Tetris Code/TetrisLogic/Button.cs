using System.Drawing;

namespace TetrisLogic
{
    /// <summary>
    /// Buttton
    /// </summary>
    public class Button
    {
        /// <summary>
        /// Click function
        /// </summary>
        private readonly dСauseClick onClick;

        /// <summary>
        /// Text font size
        /// </summary>
        private const float FONT_SIZE = 20.0f;

        /// <summary>
        /// Button text
        /// </summary>
        public string Text;

        /// <summary>
        /// Delegate for click function
        /// </summary>
        public delegate void dСauseClick();

        /// <summary>
        /// Button position
        /// </summary>
        public PointF Posistion { set; get; }

        /// <summary>
        /// Buttion size
        /// </summary>
        public SizeF Size { set; get; }

        /// <summary>
        /// Button class constructor
        /// </summary>
        /// <param name="parText">Button text</param>
        /// <param name="parOnClick">Click function</param>
        public Button(string parText, dСauseClick parOnClick)
        {
            onClick = parOnClick;
            Text = parText;
        }

        /// <summary>
        /// Setting the position and size of the button
        /// </summary>
        /// <param name="parPos">Button position</param>
        /// <param name="parSize">Buttion size</param>
        public void SetBounds(PointF parPos, SizeF parSize)
        {
            Posistion = parPos;
            Size = parSize;
        }

        /// <summary>
        /// Click function call
        /// </summary>
        /// <param name="parMousePos">Mouse position</param>
        public void Click(Point parMousePos)
        {
            if (MouseOn(parMousePos))
                onClick();
        }

        /// <summary>
        /// Button update function
        /// </summary>
        /// <param name="refGraphics">Graphics</param>
        /// <param name="parMousePos">Mouse position</param>
        /// <param name="parGraphicsSize">Graphics size</param>
        public void Update(Graphics refGraphics, Point parMousePos, SizeF parGraphicsSize)
        {
            float w = 1 + (MouseOn(parMousePos) ? 4 : 2) * parGraphicsSize.Width / 800.0f;
            refGraphics.DrawRectangle(new Pen(Color.White, w), Posistion.X, Posistion.Y, Size.Width, Size.Height);
            StringFormat format = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            refGraphics.DrawString(Text, new Font("Times New Roman", parGraphicsSize.Width / (FONT_SIZE - w) / 2), Brushes.White,
            new RectangleF(Posistion.X, Posistion.Y, Size.Width, Size.Height), format);
        }

        /// <summary>
        /// Mouse hover function
        /// </summary>
        /// <param name="parMousePos"></param>
        /// <returns></returns>
        public bool MouseOn(Point parMousePos)
        {
            return parMousePos.X > Posistion.X && parMousePos.Y > Posistion.Y &&
             parMousePos.X < Posistion.X + Size.Width &&
             parMousePos.Y < Posistion.Y + Size.Height;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TetrisLogic
{
    /// <summary>
    /// Playing field
    /// </summary>
    public static class GameField
    {
        /// <summary>
        /// Cell size
        /// </summary>
        private static SizeF cellSize;

        /// <summary>
        /// Previous frame time
        /// </summary>
        private static long prewTime;

        /// <summary>
        /// Time between frames
        /// </summary>
        private static float deltaTime;

        /// <summary>
        /// Matrix
        /// </summary>
        private static bool[,] matrix;

        /// <summary>
        /// Lines marked for deletion
        /// </summary>
        private static List<int> lines;

        /// <summary>
        /// Color of delete lines
        /// </summary>
        private static Color color;

        /// <summary>
        /// End of the game
        /// </summary>
        public static bool GameOver { get; set; }

        /// <summary>
        /// Is the game currently happening
        /// </summary>
        public static bool Game { get; set; }

        /// <summary>
        /// Element drop animation
        /// </summary>
        public static bool Up { get; set; }

        /// <summary>
        /// Playing field width
        /// </summary>
        private const int WIDTH = 10;

        /// <summary>
        /// Playing field height
        /// </summary>
        private const int HEIGHT = 18;

        /// <summary>
        /// Constructor class playing field
        /// </summary>
        static GameField()
        {
            color = Color.White;
            lines = new List<int>();
            matrix = new bool[WIDTH, HEIGHT];
            prewTime = DateTime.Now.Ticks;
        }

        /// <summary>
        /// Game board update
        /// </summary>
        /// <param name="refGraphics">Graphics</param>
        /// <param name="parSize">Graphics size</param>
        public static void Update(Graphics refGraphics, SizeF parSize)
        {
            deltaTime = (DateTime.Now.Ticks - prewTime) / 100000.0f;
            cellSize = new SizeF(parSize.Width / WIDTH, parSize.Height / HEIGHT);
            for (int i = 0; i < WIDTH; i++)
                for (int j = 0; j < HEIGHT; j++)
                {
                    Brush brush = Brushes.White;
                    if (lines.IndexOf(j) != -1)
                        brush = new SolidBrush(color);
                    refGraphics.DrawRectangle(Pens.Gray, i * cellSize.Width, j * cellSize.Height,
                      cellSize.Width, cellSize.Height);
                    if (matrix[i, j])
                        refGraphics.FillRectangle(brush,
                          i * cellSize.Width + cellSize.Width * 0.05f,
                          j * cellSize.Height + cellSize.Height * 0.05f,
                          cellSize.Width * 0.95f, cellSize.Height * 0.95f);
                }
            if (Game)
            {
                if (color.R < 5)
                {
                    for (int i = lines.Count - 1; i >= 0; i--)
                    {
                        MoveDown(lines[i]);
                        lines.RemoveAt(i);
                        Renderer.Score += 1 * (i + 1);
                    }
                    color = Color.White;
                }

                if (lines.Count == 0)
                {
                    SearchLines();
                    if (!Piece.Down(matrix, deltaTime))
                    {
                        if (Piece.Stop(matrix))
                        {
                            Game = false;
                            GameOver = true;
                        }
                        Up = true;
                    }
                    Piece.Update(matrix, refGraphics, cellSize);
                }
                else
                    color = Color.FromArgb(color.R - 4, color.G - 4, color.B - 4);

                if (Up && Piece.EndAnim)
                {
                    Up = false;
                    Piece.Reset(WIDTH);
                }
            }

            prewTime = DateTime.Now.Ticks;
        }

        /// <summary>
        /// Search for filled strings
        /// </summary>
        private static void SearchLines()
        {
            for (int i = matrix.GetLength(1) - 1; i >= 0; i--)
            {
                bool fullLine = true;
                for (int j = 0; j < matrix.GetLength(0); j++)
                    if (!matrix[j, i])
                    {
                        fullLine = false;
                        break;
                    }
                if (fullLine)
                    lines.Add(i);
            }
        }

        /// <summary>
        /// Shift cells when deleting a row
        /// </summary>
        /// <param name="parNum">Number of steps</param>
        private static void MoveDown(int parNum)
        {
            for (int z = parNum; z > 0; z--)
                for (int y = 0; y < matrix.GetLength(0); y++)
                    matrix[y, z] = matrix[y, z - 1];
        }

        /// <summary>
        /// Keyboard release processing
        /// </summary>
        /// <param name="parKey">Key</param>
        public static void KeyUp(Keys parKey)
        {
            if (Game)
                switch (parKey)
                {
                    case Keys.Right:
                        Piece.Right(matrix);
                        break;
                    case Keys.Left:
                        Piece.Left(matrix);
                        break;
                }
        }

        /// <summary>
        /// Keyboard handling
        /// </summary>
        /// <param name="parKey">Key</param>
        public static void KeyDown(Keys parKey)
        {
            if (Game && parKey == Keys.Down)
                Piece.Speed = 0.35f;
        }

        /// <summary>
        /// Resetting the playing field
        /// </summary>
        public static void Reset()
        {
            color = Color.White;
            for (int i = 0; i < WIDTH; i++)
                for (int j = 0; j < HEIGHT; j++)
                    matrix[i, j] = false;
            Game = true;
            GameOver = false;
            Up = false;
            Piece.Reset(WIDTH);
        }
    }
}

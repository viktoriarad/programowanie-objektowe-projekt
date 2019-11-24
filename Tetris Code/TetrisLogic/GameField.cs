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
    }
}

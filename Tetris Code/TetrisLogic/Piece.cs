using System;
using System.Drawing;

namespace TetrisLogic
{
    /// <summary>
    /// Piece of tetris
    /// </summary>
    public static class Piece
    {
        /// <summary>
        /// Draw position
        /// </summary>
        private static PointF dPosition;

        /// <summary>
        /// Piece of tetris cells
        /// </summary>
        private static Point[][] points;

        /// <summary>
        /// Tetris figure type number
        /// </summary>
        private static int numPoints;

        /// <summary>
        /// Random number generator
        /// </summary>
        private static Random random;

        /// <summary>
        /// All kinds of tetris figures
        /// </summary>
        public static Point[][][] pointsVariants;

        /// <summary>
        /// Shift(right / left)
        /// </summary>
        public static int rot;

        /// <summary>
        /// Piece of tetris position
        /// </summary>
        public static PointF Position { get; set; }

        /// <summary>
        /// Is the animation finished
        /// </summary>
        public static bool EndAnim { get; set; }

        /// <summary>
        /// Down speed
        /// </summary>
        public static float Speed { get; set; }

        /// <summary>
        /// Creating all kinds of tetris shapes and other objects
        /// </summary>
        public static void Create()
        {
            rot = 0;
            numPoints = 0;
            pointsVariants = new Point[6][][];
            random = new Random(DateTime.Now.Millisecond);

            pointsVariants[0] = new Point[1][];
            pointsVariants[1] = new Point[4][];
            pointsVariants[2] = new Point[2][];
            pointsVariants[3] = new Point[2][];
            pointsVariants[4] = new Point[2][];
            pointsVariants[5] = new Point[4][];

            pointsVariants[0][0] = new Point[] { new Point(0, 0), new Point(0, 1), new Point(1, 0), new Point(1, 1) };

            pointsVariants[1][0] = new Point[] { new Point(1, 0), new Point(0, 1), new Point(1, 1), new Point(2, 1) };
            pointsVariants[1][1] = new Point[] { new Point(2, 0), new Point(2, 1), new Point(2, 2), new Point(1, 1) };
            pointsVariants[1][2] = new Point[] { new Point(1, 1), new Point(2, 0), new Point(1, 0), new Point(0, 0) };
            pointsVariants[1][3] = new Point[] { new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(1, 1) };

            pointsVariants[2][0] = new Point[] { new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(1, 3) };
            pointsVariants[2][1] = new Point[] { new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(3, 1) };

            pointsVariants[3][0] = new Point[] { new Point(0, 1), new Point(1, 1), new Point(1, 2), new Point(2, 2) };
            pointsVariants[3][1] = new Point[] { new Point(1, 1), new Point(1, 2), new Point(0, 2), new Point(0, 3) };

            pointsVariants[4][0] = new Point[] { new Point(0, 1), new Point(1, 1), new Point(1, 0), new Point(2, 0) };
            pointsVariants[4][1] = new Point[] { new Point(0, 0), new Point(0, 1), new Point(1, 1), new Point(1, 2) };

            pointsVariants[5][0] = new Point[] { new Point(2, 0), new Point(0, 1), new Point(1, 1), new Point(2, 1) };
            pointsVariants[5][1] = new Point[] { new Point(1, 2), new Point(1, 1), new Point(1, 0), new Point(0, 0) };
            pointsVariants[5][2] = new Point[] { new Point(0, 0), new Point(0, 1), new Point(1, 0), new Point(2, 0) };
            pointsVariants[5][3] = new Point[] { new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(1, 2) };

            points = pointsVariants[random.Next(0, pointsVariants.Length)];
        }

        /// <summary>
        /// The ability to shift the tetris shape to the right
        /// </summary>
        /// <param name="refMatrix">Matrix of game board</param>
        /// <param name="parPos">Position</param>
        /// <param name="refPoints">Points of tetris shape</param>
        /// <returns></returns>
        private static bool CanRight(bool[,] refMatrix, PointF parPos, Point[][] refPoints)
        {
            bool result = true;
            foreach (var elPoint in refPoints[numPoints])
                if (parPos.Y + elPoint.Y > 0)
                    if (parPos.X + elPoint.X + 1 < refMatrix.GetLength(0))
                    {
                        if (refMatrix[(int)(parPos.X + elPoint.X + 1), (int)(parPos.Y + elPoint.Y)])
                            result = false;
                    }
                    else
                        result = false;
            return result;
        }

        /// <summary>
        /// The ability to shift the tetris shape to the left
        /// </summary>
        /// <param name="refMatrix">Matrix of game board</param>
        /// <param name="parPos">Position</param>
        /// <param name="refPoints">Points of tetris shape</param>
        /// <returns></returns>
        private static bool CanLeft(bool[,] refMatrix, PointF parPos, Point[][] refPoints)
        {
            bool result = true;
            foreach (var elPoint in refPoints[numPoints])
                if (parPos.Y + elPoint.Y > 0)
                    if (parPos.X + elPoint.X - 1 >= 0)
                    {
                        if (refMatrix[(int)(parPos.X + elPoint.X - 1), (int)(parPos.Y + elPoint.Y)])
                            result = false;
                    }
                    else
                        result = false;
            return result;
        }

        /// <summary>
        /// Right shift
        /// </summary>
        /// <param name="refMatrix">Matrix of game board</param>
        public static void Right(bool[,] refMatrix)
        {
            if (CanRight(refMatrix, Position, points))
                Position = new PointF(Position.X + 1, Position.Y);
            else
              if (CanRight(refMatrix, new PointF(Position.X, Position.Y + 1), points))
                rot = 1;
        }

        /// <summary>
        /// Left shift
        /// </summary>
        /// <param name="refMatrix">Matrix of game board</param>
        public static void Left(bool[,] refMatrix)
        {
            if (CanLeft(refMatrix, Position, points))
                Position = new PointF(Position.X - 1, Position.Y);
            else
              if (CanLeft(refMatrix, new PointF(Position.X, Position.Y + 1), points))
                rot = -1;
        }

        /// <summary>
        /// Shift down
        /// </summary>
        /// <param name="refMatrix">Matrix of game board</param>
        /// <param name="parDeltaTime">Time between frames</param>
        /// <returns></returns>
        public static bool Down(bool[,] refMatrix, float parDeltaTime)
        {
            Position = new PointF(Position.X, Position.Y + Speed * parDeltaTime);
            bool result = true;
            foreach (var elPoint in points[numPoints])
                if (Position.Y + elPoint.Y > 0)
                    if (Position.Y + elPoint.Y + 1 < refMatrix.GetLength(1))
                    {
                        if (refMatrix[(int)(Position.X + elPoint.X), (int)(Position.Y + elPoint.Y + 1)])
                            result = false;
                    }
                    else
                        result = false;
            if (rot != 0)
                result = true;
            return result;
        }

    }
}

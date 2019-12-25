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

            Reset(10);
        }

        /// <summary>
        /// Reset
        /// </summary>
        /// <param name="parWidth">Game board width</param>
        public static void Reset(int parWidth)
        {
            Position = new Point(parWidth / 2 - MaxX() / 2, -MaxY() + 1);
            Speed = 0.015f;
            dPosition = new PointF(Position.X, Position.Y - 300);
            points = pointsVariants[random.Next(0, pointsVariants.Length)];
            numPoints = random.Next(0, points.Length);
        }

        /// <summary>
        /// Tetris figure updates
        /// </summary>
        /// <param name="refMatrix">Matrix of game board</param>
        /// <param name="refGraphics">Graphics></param>
        /// <param name="parCellSize">Graphics size</param>
        public static void Update(bool[,] refMatrix, Graphics refGraphics, SizeF parCellSize)
        {
            dPosition.X += (((int)(Position.X + 3) - 3) - (dPosition.X / parCellSize.Width)) / 1.5f * parCellSize.Width;
            dPosition.Y += (((int)(Position.Y + 3) - 3) - (dPosition.Y / parCellSize.Height)) / 3 * parCellSize.Height;

            EndAnim = (Position.Y / parCellSize.Width - dPosition.Y < 0.1f);

            for (int i = 0; i < points[numPoints].Length; i++)
            {
                refGraphics.FillRectangle(Brushes.White,
                  dPosition.X + (points[numPoints][i].X * parCellSize.Width) + parCellSize.Width * 0.05f,
                  dPosition.Y + (points[numPoints][i].Y * parCellSize.Height) + parCellSize.Height * 0.05f,
                  parCellSize.Width * 0.9f, parCellSize.Width * 0.9f);
            }

            if (rot == 1)
                if (CanRight(refMatrix, Position, points))
                {
                    Position = new PointF(Position.X + 1, Position.Y);
                    rot = 0;
                }

            if (rot == -1)
                if (CanLeft(refMatrix, Position, points))
                {
                    Position = new PointF(Position.X - 1, Position.Y);
                    rot = 0;
                }
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

        /// <summary>
        /// Overflow check
        /// </summary>
        /// <param name="refMatrix">Matrix of game board</param>
        /// <returns></returns>
        public static bool Stop(bool[,] refMatrix)
        {
            bool result = false;
            foreach (var elPoint in points[numPoints])
                if (Position.Y + elPoint.Y < 0)
                    result = true;
                else
                {
                    if (Position.Y + elPoint.Y < refMatrix.GetLength(1))
                        refMatrix[(int)(Position.X + elPoint.X), (int)(Position.Y + elPoint.Y)] = true;
                }
            return result;
        }

        /// <summary>
        /// Figure rotation
        /// </summary>
        /// <param name="refMatrix">Matrix of game board</param>
        public static void Rotate(bool[,] refMatrix)
        {
            numPoints++;
            if (numPoints >= points.Length)
                numPoints = 0;
            if (Position.X < 0)
                Position = new PointF(0, Position.Y);
            if (Position.X + MaxX() >= refMatrix.GetLength(0))
                Position = new PointF(refMatrix.GetLength(0) - MaxX(), Position.Y);
            bool coll = false;
            foreach (var elPoint in points[numPoints])
                if (Position.X + elPoint.X >= 0 && Position.X + elPoint.X < refMatrix.GetLength(0) &&
                  Position.Y + elPoint.Y >= 0 && Position.Y + elPoint.Y < refMatrix.GetLength(1))
                    if (refMatrix[(int)(Position.X + elPoint.X), (int)(Position.Y + elPoint.Y)])
                        coll = true;
            if (coll)
                numPoints--;
            if (numPoints < 0)
                numPoints = points.Length - 1;
        }

        /// <summary>
        /// Lowest cell
        /// </summary>
        /// <returns></returns>
        public static int MaxY()
        {
            int max = points[numPoints][0].Y;
            foreach (var elPoint in points[numPoints])
                if (elPoint.Y > max)
                    max = elPoint.Y;
            return max + 1;
        }

        /// <summary>
        /// Rightmost cell
        /// </summary>
        /// <returns></returns>
        public static int MaxX()
        {
            int max = points[numPoints][0].X;
            foreach (var elPoint in points[numPoints])
                if (elPoint.X > max)
                    max = elPoint.X;
            return max + 1;
        }
    }
}

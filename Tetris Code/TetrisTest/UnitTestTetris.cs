using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TetrisLogic;

namespace TetrisTest
{
    [TestClass]
    public class UnitTestTetris
    {
        [TestMethod]
        // Test the method of creating a tetris shape
        public void TestMethodPieceCreate()
        {
            Piece.Create();
            float expectedSpeed = 0.015f;

            float actualSpeed = Piece.Speed;
            PointF actualPosition = Piece.Position;

            Assert.AreEqual(expectedSpeed, actualSpeed);
        }

        [TestMethod]
        // Tetris playing field reset method test
        public void TestMethodGameFieldReset()
        {
            Piece.Create();

            bool expected = true;
            GameField.Reset();

            bool actual = GameField.Game;
            Assert.AreEqual(expected, actual);
        }
    }
}

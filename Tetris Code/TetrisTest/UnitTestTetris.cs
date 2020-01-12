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

        [TestMethod]
        // Tetris figure control method test
        public void TestMethodGameFieldKeyDown()
        {
            Piece.Create();

            float expected = 0.35f;
            GameField.Reset();
            GameField.KeyDown(System.Windows.Forms.Keys.Down);

            float actual = Piece.Speed;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        // Keyboard processing method test
        public void TestMethodInputPush()
        {
            string expected = "Hello";
            Input input = new Input();

            input.Push('H');
            input.Push('e');
            input.Push('l');
            input.Push('l');
            input.Push('o');
            string actual = input.Text;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        // Tetris button press test
        public void TestMethodButtonClick()
        {
            Piece.Create();

            bool expected = true;
            Button button = new Button("Hello", GameField.Reset);
            button.SetBounds(new Point(0, 0), new SizeF(100, 100));

            button.Click(new Point(50, 50));
            bool actual = GameField.Game;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        // Test the method of setting button bounds
        public void TestMethodButtonSetBounds()
        {
            Button button = new Button("Hello", null);
            button.SetBounds(new PointF(100, 200), new SizeF(300, 40));

            PointF expectedPosition = new PointF(100, 200);
            SizeF expectedSize = new SizeF(300, 40);

            PointF actualPosition = button.Posistion;
            SizeF actualSize = button.Size;

            Assert.AreEqual(expectedPosition, actualPosition);
            Assert.AreEqual(expectedSize, actualSize);
        }

        [TestMethod]
        // Test the method of button mouse on
        public void TestMethodButtonMouseOn()
        {
            Button button = new Button("Hello", null);
            button.SetBounds(new PointF(100, 200), new SizeF(300, 40));

            bool expected = true;
            bool actual = button.MouseOn(new Point(110, 210));

            Assert.AreEqual(expected, actual);
        }
    }
}

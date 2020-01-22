using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace TetrisLogic
{
    /// <summary>
    /// Game handler
    /// </summary>
    public static class Renderer
    {
        /// <summary>
        /// Mouse position
        /// </summary>
        private static Point mousePos;

        /// <summary>
        /// Play button
        /// </summary>
        private static Button buttonPlay;

        /// <summary>
        /// Pause button
        /// </summary>
        private static Button buttonPause;

        /// <summary>
        /// Exit button
        /// </summary>
        private static Button buttonExit;

        /// <summary>
        /// Menu button
        /// </summary>
        private static Button buttonScores;

        /// <summary>
        /// Restart button
        /// </summary>
        private static Button buttonRestart;

        /// <summary>
        /// dashboard
        /// </summary>
        private static Panel panelGameOver;

        /// <summary>
        /// Pause panel
        /// </summary>
        private static Panel panelPause;

        /// <summary>
        /// Player name input field
        /// </summary>
        private static Input userName;

        /// <summary>
        /// Graphics size
        /// </summary>
        public static Size GraphicsSize { get; set; }

        /// <summary>
        /// Score
        /// </summary>
        public static int Score { get; set; }

        /// <summary>
        /// Score visibility
        /// </summary>
        public static bool ScoresVisible { get; set; }

        /// <summary>
        /// Pause visibility
        /// </summary>
        public static bool PauseVisible { get; set; }

        /// <summary>
        /// Scores
        /// </summary>
        public static List<KeyValuePair<string, int>> Scores { get; set; }

        /// <summary>
        /// Initializing game objects
        /// </summary>
        public static void Create()
        {
            GameField.Reset();
            GameField.Game = false;
            ScoresVisible = true;
            Scores = new List<KeyValuePair<string, int>>();
            LoadScores();
            buttonPlay = new Button("Play", StartGame);
            buttonExit = new Button("Exit", Exit);
            buttonPause = new Button("Pause", Pause);
            buttonRestart = new Button("⟲", Restart);
            buttonScores = new Button("Menu", ShowScores);
            panelGameOver = new Panel("Game Over");
            panelPause = new Panel("Pause");
            userName = new Input();
            Score = 0;
        }

        /// <summary>
        /// Frame update
        /// </summary>
        /// <param name="refGraphics">Graphics</param>
        public static void Update(Graphics refGraphics)
        {
            refGraphics.Clear(Color.Black);
            GameField.Update(refGraphics, new SizeF(GraphicsSize.Width, GraphicsSize.Height / 19.5f * 18.0f));

            buttonPlay.SetBounds(new PointF(GraphicsSize.Width / 10.0f * 0.8f, GraphicsSize.Height / 19.5f * 18.1f),
              new SizeF(GraphicsSize.Width / 10.0f * 4f, GraphicsSize.Width / 10.0f * 1f));
            buttonExit.SetBounds(new PointF(GraphicsSize.Width / 10.0f * 5.3f, GraphicsSize.Height / 19.5f * 18.1f),
              new SizeF(GraphicsSize.Width / 10.0f * 4f, GraphicsSize.Width / 10.0f * 1f));
            panelGameOver.SetBounds(new PointF(GraphicsSize.Width / 10.0f * 0.8f, GraphicsSize.Height / 19.5f * 6f),
              new SizeF(GraphicsSize.Width / 10.0f * 8.4f, GraphicsSize.Width / 10.0f * 3f));
            buttonPause.SetBounds(new PointF(GraphicsSize.Width / 10.0f * 6.8f, GraphicsSize.Height / 19.5f * 18.2f),
              new SizeF(GraphicsSize.Width / 10.0f * 3f, GraphicsSize.Width / 10.0f * 1f));
            buttonRestart.SetBounds(new PointF(GraphicsSize.Width / 10.0f * 5.6f, GraphicsSize.Height / 19.5f * 18.2f),
              new SizeF(GraphicsSize.Width / 10.0f * 1f, GraphicsSize.Width / 10.0f * 1f));
            buttonScores.SetBounds(new PointF(GraphicsSize.Width / 10.0f * 3.6f, GraphicsSize.Height / 19.5f * 18.2f),
              new SizeF(GraphicsSize.Width / 10.0f * 3f, GraphicsSize.Width / 10.0f * 1f));
            panelPause.SetBounds(new PointF(GraphicsSize.Width / 10.0f * 0.8f, GraphicsSize.Height / 19.5f * 7f),
              new SizeF(GraphicsSize.Width / 10.0f * 8.4f, GraphicsSize.Width / 10.0f * 2f));
            userName.SetBounds(new PointF(GraphicsSize.Width / 10.0f * 5.0f, GraphicsSize.Height / 19.5f * 7.5f),
              new SizeF(GraphicsSize.Width / 10.0f * 3.5f, GraphicsSize.Width / 10.0f * 1f));

            if (GameField.Game)
            {
                DrawString(refGraphics, "Score: " + Score.ToString(), GraphicsSize.Width / 10.0f * 0.2f, GraphicsSize.Height / 19.5f * 18.3f, GraphicsSize.Width, GraphicsSize.Height / 19.5f, 19, false, true);
                buttonPause.Update(refGraphics, mousePos, GraphicsSize);
                if (!PauseVisible)
                    buttonRestart.Update(refGraphics, mousePos, GraphicsSize);
            }

            if (ScoresVisible)
            {
                refGraphics.Clear(Color.Black);
                DrawString(refGraphics, "Highscores", 0, 0, (float)GraphicsSize.Width, GraphicsSize.Height * 0.1f, 15, true, true);
                for (int i = 0; i < Math.Min(Scores.Count, 9); i++)
                {
                    refGraphics.DrawString(Scores[i].Key, new Font(FontFamily.GenericSansSerif, GraphicsSize.Width / 16.0f / 2),
                      Brushes.White, new RectangleF(GraphicsSize.Width * 0.05f, GraphicsSize.Height * 0.10f +
                      i * GraphicsSize.Height * 0.075f, GraphicsSize.Width, GraphicsSize.Height * 0.08f),
                      new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    refGraphics.DrawString(Scores[i].Value.ToString(), new Font(FontFamily.GenericSansSerif, GraphicsSize.Width / 16.0f / 2),
                      Brushes.White, new RectangleF(GraphicsSize.Width * 0.05f, GraphicsSize.Height * 0.10f +
                      i * GraphicsSize.Height * 0.075f, GraphicsSize.Width * 0.9f, GraphicsSize.Height * 0.08f),
                      new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });
                }
                for (int i = 9; i >= Scores.Count; i--)
                {
                    refGraphics.DrawString(".....", new Font(FontFamily.GenericSansSerif, GraphicsSize.Width / 16.0f / 2),
                     Brushes.White, new RectangleF(GraphicsSize.Width * 0.05f, GraphicsSize.Height * 0.10f +
                     i * GraphicsSize.Height * 0.075f, GraphicsSize.Width, GraphicsSize.Height * 0.08f),
                     new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    refGraphics.DrawString(".....", new Font(FontFamily.GenericSansSerif, GraphicsSize.Width / 16.0f / 2),
                      Brushes.White, new RectangleF(GraphicsSize.Width * 0.05f, GraphicsSize.Height * 0.10f +
                      i * GraphicsSize.Height * 0.075f, GraphicsSize.Width * 0.9f, GraphicsSize.Height * 0.08f),
                      new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });
                }
                buttonPlay.Update(refGraphics, mousePos, GraphicsSize);
                buttonExit.Update(refGraphics, mousePos, GraphicsSize);
            }
            if (GameField.GameOver)
            {
                panelGameOver.Update(refGraphics, false);
                userName.Update(refGraphics, GraphicsSize);
                DrawString(refGraphics, "Your name: ", GraphicsSize.Width / 10.0f * 1.3f, GraphicsSize.Height / 19.5f * 5.95f, GraphicsSize.Width / 10.0f * 4.0f, GraphicsSize.Width / 10.0f * 4.0f, 8, false, true);
                buttonRestart.Update(refGraphics, mousePos, GraphicsSize);
                buttonScores.Posistion = buttonPause.Posistion;
                buttonScores.Update(refGraphics, mousePos, GraphicsSize);
                DrawString(refGraphics, "Score: " + Score.ToString(), GraphicsSize.Width / 10.0f * 0.2f, GraphicsSize.Height / 19.5f * 18.3f, GraphicsSize.Width, GraphicsSize.Height / 19.5f, 19, false, true);
            }
            if (PauseVisible)
            {
                panelPause.Update(refGraphics, true);
                buttonScores.SetBounds(new PointF(GraphicsSize.Width / 10.0f * 3.6f, GraphicsSize.Height / 19.5f * 18.2f),
                   new SizeF(GraphicsSize.Width / 10.0f * 3f, GraphicsSize.Width / 10.0f * 1f));
                buttonScores.Update(refGraphics, mousePos, GraphicsSize);
            }
        }

        /// <summary>
        /// Receive character from keyboard
        /// </summary>
        /// <param name="parChar">Character</param>
        public static void KeyChar(char parChar)
        {
            if (GameField.GameOver) { 
                if (parChar == (char)13)
                {
                    ShowScores();
                    return;
                }
                userName.Push(parChar);
            }
        }

        /// <summary>
        /// Game start
        /// </summary>
        public static void StartGame()
        {
            userName.Text = "";
            ScoresVisible = false;
            buttonPause.Text = "Pause";
            GameField.Reset();
        }

        /// <summary>
        /// Adding a highscore
        /// </summary>
        private static void AddScore()
        {
            string name = userName.Text == "" ? "Anonymous" : userName.Text;
            if (Scores.Count == 0 || Score <= Scores[Scores.Count - 1].Value)
                Scores.Add(new KeyValuePair<string, int>(name, Score));
            else
                for (int i = Scores.Count - 1; i >= 0; i--)
                {
                    if (Score <= Scores[i].Value)
                    {
                        Scores.Insert(i, new KeyValuePair<string, int>(name, Score));
                        break;
                    }
                    if (Score > Scores[0].Value)
                    {
                        Scores.Insert(0, new KeyValuePair<string, int>(name, Score));
                        break;
                    }
                }

            Score = 0;
        }

        /// <summary>
        /// High scores
        /// </summary>
        private static void ShowScores()
        {
            ScoresVisible = true;
            PauseVisible = false;
            if (GameField.GameOver)
                AddScore();
            GameField.Reset();
        }

        /// <summary>
        /// Quit the game
        /// </summary>
        private static void Exit()
        {
            SaveScores();
            Environment.Exit(0);
        }

        /// <summary>
        /// Pause
        /// </summary>
        public static void Pause()
        {
            if (Piece.Speed != 0)
            {
                Piece.Speed = 0;
                PauseVisible = true;
                buttonPause.Text = "Go on";
            }
            else
            {
                Piece.Speed = 0.015f;
                PauseVisible = false;
                buttonPause.Text = "Pause";
            }
        }

        /// <summary>
        /// Restart
        /// </summary>
        private static void Restart()
        {
            if (GameField.GameOver)
                AddScore();
            GameField.Reset();
        }

        /// <summary>
        /// Text drawing
        /// </summary>
        /// <param name="refGraphics">Graphics</param>
        /// <param name="parText">Text</param>
        /// <param name="parX">X position</param>
        /// <param name="parY">Y position</param>
        /// <param name="parW">Width</param>
        /// <param name="parH">Height</param>
        /// <param name="parS">Font size</param>
        /// <param name="parC">Center or not</param>
        /// <param name="parNeg">Negtive color or not</param> 
        public static void DrawString(Graphics refGraphics, string parText, float parX, float parY, float parW,
          float parH, float parS, bool parC, bool parNeg)
        {
            refGraphics.DrawString(parText, new Font(FontFamily.GenericSansSerif, parW / parS / 2),
              parNeg ? Brushes.White : Brushes.Black, new RectangleF(parX, parY, parW, parH),
              new StringFormat
              {
                  Alignment = parC ? StringAlignment.Center : StringAlignment.Near,
                  LineAlignment = StringAlignment.Center
              });
        }

        /// <summary>
        /// Get mouse position
        /// </summary>
        /// <param name="parPos">Mouse position</param>
        public static void MousePos(Point parPos)
        {
            mousePos = parPos;
        }

        /// <summary>
        /// Click
        /// </summary>
        public static void Click()
        {
            if (ScoresVisible)
            {
                buttonPlay.Click(mousePos);
                buttonExit.Click(mousePos);
            }
            if (GameField.Game)
            {
                buttonPause.Click(mousePos);
                buttonRestart.Click(mousePos);
            }
            if (PauseVisible)
                buttonScores.Click(mousePos);
            if (GameField.GameOver)
            {
                buttonScores.Click(mousePos);
                buttonRestart.Click(mousePos);
            }
        }

        /// <summary>
        /// Keyboard handling
        /// </summary>
        /// <param name="parKey">Key</param>
        public static void KeyDown(Keys parKey)
        {
            if (parKey == Keys.Back)
                userName.Backspace();
        }

        /// <summary>
        /// Load highscores
        /// </summary>
        public static void LoadScores()
        {
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Tetris/scores.score"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Tetris/scores.score", FileMode.OpenOrCreate))
                {
                    Scores = (List<KeyValuePair<string, int>>)formatter.Deserialize(fs);
                }
            }
        }

        /// <summary>
        /// Save highscores
        /// </summary>
        public static void SaveScores()
        {
            try
            {
                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Tetris/"))
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Tetris/");
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Tetris/scores.score", FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, Scores);
                }
            }
            catch { };
        }
    }
}

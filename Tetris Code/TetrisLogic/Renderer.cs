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
        /// Receive character from keyboard
        /// </summary>
        /// <param name="parChar">Character</param>
        public static void KeyChar(char parChar)
        {
            userName.Push(parChar);
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

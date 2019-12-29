using System;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace TetrisLogic
{
	/// <summary>
	/// Drawing
	/// </summary>
	public partial class GameCanvas : UserControl
	{
		/// <summary>
		/// Previous frame time
		/// </summary>
		private static long prewTime;

		/// <summary>
		/// Time between frames
		/// </summary>
		private static float deltaTime;

		/// <summary>
		/// Constructor
		/// </summary>
		public GameCanvas()
		{
			InitializeComponent();
			Piece.Create();
			Renderer.Create();
			Renderer.GraphicsSize = ClientSize;
			prewTime = DateTime.Now.Ticks;
		}


		/// <summary>
		/// Game rendering
		/// </summary>
		/// <param name="refSender">Event sender</param>
		/// <param name="e">Parameners</param>
		private void GameCanvas_Paint(object refSender, PaintEventArgs e)
		{
			Renderer.GraphicsSize = new Size(ClientSize.Width - 1, ClientSize.Height - 1);
			Renderer.Update(e.Graphics);
		}

		/// <summary>
		/// Keyboard release processing
		/// </summary>
		/// <param name="refSender">Event sender</param>
		/// <param name="parE">Parameters</param>
		private void GameCanvas_KeyUp(object refSender, KeyEventArgs parE)
		{
			GameField.KeyUp(parE.KeyCode);
		}

		/// <summary>
		/// Keyboard рandling
		/// </summary>
		/// <param name="key">Key</param>
		public static void GameCanvas_KeyDown(Keys key)
		{
			GameField.KeyDown(key);
			Renderer.KeyDown(key);
		}

		/// <summary>
		/// Mouse movement processing
		/// </summary>
		/// <param name="ferSender">Event sender</param>
		/// <param name="parE">Parameters</param>
		private void GameCanvas_MouseMove(object ferSender, MouseEventArgs parE)
		{
			Renderer.MousePos(parE.Location);
		}

		/// <summary>
		/// Click processing
		/// </summary>
		/// <param name="refSender">Event sender</param>
		/// <param name="parE">Parameters</param>
		private void GameCanvas_Click(object refSender, EventArgs parE)
		{
			Renderer.Click();
		}

		/// <summary>
		/// Keyboard processing(characters)
		/// </summary>
		/// <param name="refSender">Event sender</param>
		/// <param name="parE">Parameters</param>
		private void GameCanvas_KeyPress(object refSender, KeyPressEventArgs parE)
		{
			Renderer.KeyChar(parE.KeyChar);
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			deltaTime = (DateTime.Now.Ticks - prewTime) / 100000.0f;
			Invalidate();
			prewTime = DateTime.Now.Ticks;
		}

		private void GameCanvas_Load(object sender, EventArgs e)
		{
			timer.Enabled = true;
		}
	}
}

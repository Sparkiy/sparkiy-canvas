using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SparkiyEngine.Graphics.Canvas.Fonts;
using SparkiyEngine.Graphics.Canvas.Shapes;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = SparkiyEngine.Graphics.Canvas.Shapes.Rectangle;

namespace SparkiyEngine.Graphics.Canvas
{
    public class SparkiyCanvas : DrawableGameComponent
    {
	    private FontManager fontManager;

	    private BasicEffect basicEffect;

		private const int MaxPrimitives = 21845;
		private readonly List<IColorPrimitive> primitivesList = new List<IColorPrimitive>();
	    private bool isBeginCalled;


	    /// <summary>
		/// Initializes a new instance of the <see cref="SparkiyCanvas"/> class.
		/// </summary>
		/// <param name="game">The game.</param>
	    public SparkiyCanvas(Game game) : base(game)
	    {

	    }


	    public override void Initialize()
	    {
		    this.Game.GraphicsDevice.DeviceLost += (sender, args) => this.Initialize();

			this.InitializeEffect();
			this.InitializeTransform();

			base.Initialize();
		}

	    private void InitializeEffect()
		{
			this.basicEffect = new BasicEffect(GraphicsDevice)
			{
				VertexColorEnabled = true,
			};
		}

	    private void InitializeTransform()
	    {
		    this.WorldMatrix = Matrix.Identity;
		    this.ViewMatrix = Matrix.CreateLookAt(new Vector3(0f, 0f, 1f), Vector3.Zero, Vector3.Up);
		    this.ProjectionMatrix = Matrix.CreateOrthographicOffCenter(
				0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0f, 1f);
	    }

		/// <summary>
		/// Draws primitives in list and clears that list.
		/// Use this in Draw.End call and when list has max number of allowed primitives.
		/// </summary>
		private void FlushPrimitives()
		{
			// If nothing to draw skip return
			if (primitivesList.Count == 0) return;

			// Apply effect
			this.basicEffect.CurrentTechnique.Passes[0].Apply();

			// Draw primitives from list
			foreach (var primitive in primitivesList)
				GraphicsDevice.DrawUserPrimitives(primitive.Type, primitive.Vertices, 0, 1);

			// Clear drawn primitives
			this.primitivesList.Clear();
		}

	    public override void Draw(GameTime gameTime)
	    {
		    this.ViewMatrix = Matrix.Identity;
			this.DrawSquare(50, 50, 100);
			this.DrawLine(150,150,250,250);
			this.FlushPrimitives();

		    base.Draw(gameTime);
	    }

	    internal void Draw<T>(T complex) where T : IShape
	    {
			// Check if this can be added to list or flush is needed.
			if (primitivesList.Count >= MaxPrimitives)
				this.FlushPrimitives();

			// If drawable contains primitives, add them to list.
			if (complex != null && complex.Primitives != null)
				primitivesList.AddRange(complex.Primitives);
	    }

		#region Shapes 

	    public void DrawLine(float x1, float y1, float x2, float y2)
	    {
		    this.Draw(new Line(new Vector3(x1, y1, 0), new Vector3(x2, y2, 0), Color.Red));
	    }

	    public void DrawRect(float x, float y, float width, float height)
	    {
			this.Draw(new Rectangle(new Vector3(x, y, 0), new Vector3(x + width, y + height, 0), Color.Red));
	    }

		public void DrawSquare(float x, float y, float size)
	    {
			this.Draw(new Rectangle(new Vector3(x, y, 0), size, Color.Red));
	    }

		#endregion

		public Matrix ViewMatrix
		{
			get { return this.basicEffect.View; }
			set { this.basicEffect.View = value; }
		}

		public Matrix WorldMatrix
		{
			get { return this.basicEffect.World; }
			set { this.basicEffect.World = value; }
		}

		public Matrix ProjectionMatrix
		{
			get { return this.basicEffect.Projection; }
			set { this.basicEffect.Projection = value; }
		}
    }
}

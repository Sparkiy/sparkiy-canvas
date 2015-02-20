using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SparkiyEngine.Graphics.Canvas.Fonts;
using SparkiyEngine.Graphics.Canvas.Shapes;

namespace SparkiyEngine.Graphics.Canvas
{
    public class SparkiyCanvas : DrawableGameComponent
    {
	    private FontManager fontManager;

	    private BasicEffect basicEffect;

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
		    WorldMatrix = Matrix.Identity;
		    ViewMatrix = Matrix.CreateLookAt(new Vector3(0f, 0f, 1f), Vector3.Zero, Vector3.Up);
		    ProjectionMatrix = Matrix.CreateOrthographicOffCenter(
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

		public void Begin()
		{
			this.CheckEndCall();
			this.isBeginCalled = true;

			this.ViewMatrix = Matrix.Identity;
		}

	    private void CheckBeginCall()
	    {
		    if (!this.isBeginCalled)
		    {
			    throw new InvalidOperationException("Begin call is needed before this call!");
		    }
	    }

	    public void End()
		{
			this.CheckBeginCall();

			// Iscrtava sve osnovne oblike
			this.FlushPrimitives();

			this.isBeginCalled = false;
		}

		private void CheckEndCall()
		{
			if (this.isBeginCalled)
			{
				throw new InvalidOperationException("End call is needed before this call!");
			}
		}

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

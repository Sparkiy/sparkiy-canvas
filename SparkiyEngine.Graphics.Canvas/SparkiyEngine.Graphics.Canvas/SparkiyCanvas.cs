using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
			this.StyleManager = new PushPopManagement<Style2D>();
	    }


		#region Initialization

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		public override void Initialize()
	    {
		    this.Game.GraphicsDevice.DeviceLost += (sender, args) => this.Initialize();

			this.InitializeEffect();
			this.InitializeTransform();

			base.Initialize();
		}

		/// <summary>
		/// Initializes the effect.
		/// </summary>
		private void InitializeEffect()
		{
			this.basicEffect = new BasicEffect(GraphicsDevice)
			{
				VertexColorEnabled = true,
			};
		}

		/// <summary>
		/// Initializes the transform.
		/// </summary>
		private void InitializeTransform()
	    {
		    this.WorldMatrix = Matrix.Identity;
		    this.ViewMatrix = Matrix.CreateLookAt(new Vector3(0f, 0f, 1f), Vector3.Zero, Vector3.Up);
		    this.ProjectionMatrix = Matrix.CreateOrthographicOffCenter(
				0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0f, 1f);
	    }

		#endregion /Initialization

		#region Draw

	    private void ResetScene()
	    {
			// Clear world transforms
		    this.ViewMatrix = Matrix.Identity;

			// Clear styles
			this.Style = new Style2D();
			this.StyleManager.Clear();
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
		    this.ResetScene();

			//RasterizerState rasterizerState = new RasterizerState();
			//rasterizerState.FillMode = FillMode.WireFrame;
			//this.GraphicsDevice.RasterizerState = rasterizerState;

			// Call draw primitives here
			this.FillDisable();
			this.SetStrokeThickness(10);
			this.DrawRect(100, 100, 200, 200);

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

		#endregion /Draw

		#region Shapes 

		public void DrawLine(float x1, float y1, float x2, float y2)
		{
			if (!this.Style.IsStrokeEnabled)
				return;

			if (this.Style.StrokeThickness > 1)
				this.DrawLineThick(x1, y1, x2, y2, this.Style.StrokeThickness);
			else this.Draw(new Line(new Vector3(x1, y1, 0), new Vector3(x2, y2, 0), this.Style.StrokeColor));
	    }

	    private void DrawLineThick(float x1, float y1, float x2, float y2, float thickness)
	    {
		    this.Draw(new LineThick(new Vector3(x1, y1, 0), new Vector3(x2, y2, 0), thickness, this.Style.StrokeColor));
	    }

	    public void DrawRect(float x, float y, float width, float height)
	    {
		    if (this.Style.IsStrokeEnabled)
			    this.Draw(new RectangleOutline(
					new Vector3(x, y, 0), new Vector3(x + width, y + height, 0), 
					this.Style.StrokeThickness, this.Style.FillColor));

			if (this.Style.IsFillEnabled)
				this.Draw(new Rectangle(
					new Vector3(x, y, 0), new Vector3(x + width, y + height, 0), 
					this.Style.FillColor));
	    }

		public void DrawSquare(float x, float y, float size)
	    {
			if (this.Style.IsStrokeEnabled)
				this.Draw(new RectangleOutline(
					new Vector3(x, y, 0), size,
					this.Style.StrokeThickness, this.Style.FillColor));

			if (this.Style.IsFillEnabled)
				this.Draw(new Rectangle(
					new Vector3(x, y, 0), size, 
					this.Style.FillColor));
	    }

	    public void DrawTriagle(float ax, float ay, float bx, float by, float cx, float cy)
	    {
		    this.Draw(new Triangle(
				new Vector3(ax, ay, 0),
				new Vector3(bx, by, 0),
				new Vector3(cx, cy, 0),
				this.Style.FillColor));
	    }

	    public void DrawEllipse(float x, float y, float width, float height)
	    {
		    if (this.Style.IsStrokeEnabled)
			    this.Draw(new EllipseOutline(
					new Vector3(x, y, 0), 
					width, height, 
					this.Style.StrokeThickness,
					this.Style.StrokeColor));

		    if (this.Style.IsFillEnabled)
			    this.Draw(new Ellipse(
				    new Vector3(x, y, 0),
				    width, height,
				    this.Style.FillColor));
	    }

		#endregion /Shapes


		#region Style

		public void SetStrokeColor(float red, float green, float blue)
		{
			this.Style.StrokeColor = new Color(new Vector3(red, green, blue));
			this.Style.IsStrokeEnabled = true;
		}

		public Color GetStrokeColor()
		{
			return this.Style.StrokeColor;
		}

		public void SetStrokeThickness(float thickness)
		{
			this.Style.StrokeThickness = thickness;
			this.Style.IsStrokeEnabled = true;
		}

		public double GetStrokeThickness()
		{
			return this.Style.StrokeThickness;
		}

		public void StrokeDisable()
		{
			this.Style.IsStrokeEnabled = false;
		}

		public void SetFill(float red, float green, float blue)
		{
			this.Style.FillColor = new Color(new Vector3(red, green, blue));
			this.Style.IsFillEnabled = true;
		}

		public Color GetFill()
		{
			return this.Style.FillColor;
		}

		public void FillDisable()
		{
			this.Style.IsFillEnabled = false;
		}

		public void PushStyle()
		{
			this.StyleManager.Push(this.Style);
		}

		public void PopStyle()
		{
			this.Style = this.StyleManager.Pop();
		}

		public void SaveStyle(string key)
		{
			this.StyleManager.Save(key, this.Style);
		}

		public void LoadStyle(string key)
		{
			this.StyleManager.Load(key);
		}

		public void ResetStyle()
		{
			this.Style = new Style2D();
		}

		#endregion


		#region Properties

		#region Style

		/// <summary>
		/// Gets or sets the style.
		/// </summary>
		/// <value>
		/// The style.
		/// </value>
		private Style2D Style { get; set; }

		/// <summary>
		/// Gets or sets the style manager.
		/// </summary>
		/// <value>
		/// The style manager.
		/// </value>
		private PushPopManagement<Style2D> StyleManager { get; set; }

		#endregion

		#region World Transform

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

		#endregion /World Transform

		#endregion /Properties
	}
}

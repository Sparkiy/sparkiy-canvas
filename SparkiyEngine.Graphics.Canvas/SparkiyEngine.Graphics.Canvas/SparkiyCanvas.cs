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
	public delegate void CanvasDrawEventHandler(object sender);

    public class SparkiyCanvas : DrawableGameComponent
    {
	    private FontManager fontManager;

	    private Matrix viewMatrix;
	    private Matrix projectionMatrix;
	    private Matrix worldMatrix;

	    private BasicEffect basicEffect;
	    private BasicEffect basicEffectTexture;

		private static readonly Color DefaultBackgroundColor = new Color(new Vector4(0.1843f, 0.6156f, 0.7843f, 1));
	    private Color backgroundColor;


		private const int MaxPrimitives = 21845;
		private readonly List<IColorPrimitive> primitivesList = new List<IColorPrimitive>();
	    private bool isBeginCalled;
	    private static readonly short[] texturedQuadIndices = {0, 3, 2, 0, 1, 3};

	    public event CanvasDrawEventHandler DrawReady;


	    /// <summary>
		/// Initializes a new instance of the <see cref="SparkiyCanvas"/> class.
		/// </summary>
		/// <param name="game">The game.</param>
	    public SparkiyCanvas(Game game) : base(game)
	    {
	    }


		#region Initialization

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		public override void Initialize()
	    {
		    this.Game.GraphicsDevice.DeviceLost += (sender, args) => this.Initialize();

			this.Reset();

			base.Initialize();
		}

		/// <summary>
		/// Initializes the effect.
		/// </summary>
		private void InitializeEffect()
		{
			this.basicEffect = new BasicEffect(this.GraphicsDevice)
			{
				VertexColorEnabled = true,
				FogEnabled = false,
				LightingEnabled = false,
			};

			this.basicEffectTexture = new BasicEffect(this.GraphicsDevice)
			{
				VertexColorEnabled = true,
				TextureEnabled = true,
				FogEnabled = false,
				LightingEnabled = false,
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

		public void Reset()
		{
			this.StyleManager = new PushPopManagement<Style2D>();
			this.TransformManager = new PushPopManagement<Matrix>();

			this.InitializeEffect();
			this.InitializeTransform();

			this.ResetScene();
		}

		#region Draw

	    private void ResetScene()
	    {
			// Clear world transforms
		    this.ViewMatrix = Matrix.Identity;

			// Reset background color
		    this.backgroundColor = DefaultBackgroundColor;

			// Clear styles
			this.ResetStyle();
			this.StyleManager.Clear();

			// Clear transform
			this.ResetTransform();
			this.TransformManager.Clear();
	    }

	    private void FlushTexture(TexturedQuad quad)
	    {
		    // If nothing to draw, skip
		    if (quad.Texture == null || quad.Vertices == null || quad.Vertices.Length == 0) return;

			// Assign texture
		    this.basicEffectTexture.Texture = quad.Texture;

			// Apply effect
			this.basicEffectTexture.CurrentTechnique.Passes[0].Apply();

			// Draw textured quad
		    this.GraphicsDevice.DrawUserIndexedPrimitives(quad.Type, quad.Vertices, 0, 4, TexturedQuadIndices, 0, 2);
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
			//rasterizerState.CullMode = CullMode.None;
			//this.GraphicsDevice.RasterizerState = rasterizerState;

			// Call draw primitives here
			this.FillDisable();

		    var x = Mouse.GetState().X;
			var y = Mouse.GetState().Y;

			this.SetStrokeThickness(10);
			this.SetStrokeColor(1, 0, 0);
			this.DrawLine(300, 300, x, y);

			this.DrawRect(12, 12, 50, 50);

			this.DrawEllipse(70, 50, 60, 30);

			this.SetStrokeThickness(1);
			this.SetStrokeColor(0, 0, 0);
			this.DrawLine(300, 300, x, y);

			// Get others know that they can call draw call now
			if (this.DrawReady != null)
				this.DrawReady(this);
			
			this.FlushPrimitives();

		    base.Draw(gameTime);
	    }

	    internal void Draw(TexturedQuad texture)
	    {
			// First draw primitives that need to be drawn underneath the texture
		    this.FlushPrimitives();

			// Draw the texture
			this.FlushTexture(texture);
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

		#region Textures

		/// <summary>
		/// Draws the texture.
		/// </summary>
		/// <param name="x">The x coordinate of upper-left position.</param>
		/// <param name="y">The y coordinate of upper-left position.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="texture">The texture.</param>
	    public void DrawTexture(float x, float y, float width, float height, Texture2D texture)
	    {
		    this.Draw(new TexturedQuad(new Vector3(x, y, 0), width, height, texture));
	    }

		#endregion /Textures

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

	    public void ClearBackground()
	    {
			this.GraphicsDevice.Clear(this.backgroundColor);
	    }

	    public void SetBackgroundColor(float red, float green, float blue)
	    {
		    this.backgroundColor = new Color(new Vector4(red, green, blue, 1));
	    }

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

		public float GetStrokeThickness()
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

		#region Transform

		public void PushTransform()
		{
			this.TransformManager.Push(this.Transform);
		}

		public void PopTransform()
		{
			this.Transform = this.TransformManager.Pop();
		}

		public void SaveTransform(string key)
		{
			this.TransformManager.Save(key, this.Transform);
		}

		public void LoadTransform(string key)
		{
			this.TransformManager.Load(key);
		}

		public void ResetTransform()
		{
			this.Transform = Matrix.Identity;
		}

		#endregion /Transform


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

		#endregion /Style

		#region World Transform

		/// <summary>
		/// Gets or sets the transform.
		/// </summary>
		/// <value>
		/// The transform.
		/// </value>
		private Matrix Transform { get; set; }

		/// <summary>
		/// Gets or sets the transform manager.
		/// </summary>
		/// <value>
		/// The transform manager.
		/// </value>
		private PushPopManagement<Matrix> TransformManager { get; set; }

		/// <summary>
		/// Assigns new transform matrices to basic effects.
		/// </summary>
	    private void TransformMatrixUpdated()
	    {
		    this.basicEffect.View = this.ViewMatrix;
			this.basicEffect.World = this.WorldMatrix;
			this.basicEffect.Projection = this.ProjectionMatrix;

			this.basicEffectTexture.View = this.ViewMatrix;
			this.basicEffectTexture.World = this.WorldMatrix;
			this.basicEffectTexture.Projection = this.ProjectionMatrix;
	    }

		/// <summary>
		/// Gets or sets the view matrix.
		/// </summary>
		/// <value>
		/// The view matrix.
		/// </value>
		public Matrix ViewMatrix
		{
			get { return this.viewMatrix; }
			set
			{
				this.viewMatrix = value;
				this.TransformMatrixUpdated();
			}
		}

		/// <summary>
		/// Gets or sets the world matrix.
		/// </summary>
		/// <value>
		/// The world matrix.
		/// </value>
		public Matrix WorldMatrix
		{
			get { return this.worldMatrix; }
			set
			{
				this.worldMatrix = value;
				this.TransformMatrixUpdated();
			}
		}

		/// <summary>
		/// Gets or sets the projection matrix.
		/// </summary>
		/// <value>
		/// The projection matrix.
		/// </value>
		public Matrix ProjectionMatrix
		{
			get { return this.projectionMatrix; }
			set
			{
				this.projectionMatrix = value;
				this.TransformMatrixUpdated();
			}
		}

		#endregion /World Transform

		/// <summary>
		/// Gets the textured quad indices.
		/// </summary>
		/// <value>
		/// The textured quad indices.
		/// </value>
		public short[] TexturedQuadIndices
		{
			get { return texturedQuadIndices; }
		}

		#endregion /Properties
    }
}

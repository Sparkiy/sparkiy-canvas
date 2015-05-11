using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// ReSharper disable once CheckNamespace
namespace SparkiyEngine.Graphics.Canvas.Shapes
{
	internal interface IShape
	{
		IColorPrimitive[] Primitives { get; }
	}

	internal interface ITexturedPrimitive
	{
		VertexPositionColorTexture[] Vertices { get; }

		PrimitiveType Type { get; }

		Texture2D Texture { get; }
	}

	internal interface IColorPrimitive
	{
		VertexPositionColor[] Vertices { get; }

		PrimitiveType Type { get; }
	}

	internal struct TexturedQuad : ITexturedPrimitive
	{
		private readonly VertexPositionColorTexture[] vertices;
		private readonly Texture2D texture;


		/// <summary>
		/// Initializes a new instance of the <see cref="TexturedQuad"/> struct.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="texture">The texture.</param>
		public TexturedQuad(Vector3 position, float width, float height, Texture2D texture)
			: this(position, width, height, texture, Color.White)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TexturedQuad"/> struct.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="texture">The texture.</param>
		/// <param name="color">The color.</param>
		public TexturedQuad(Vector3 position, float width, float height, Texture2D texture, Color color)
			: this(
				new Vector3(position.X, position.Y, position.Z),
				new Vector3(position.X + width, position.Y, position.Z),
				new Vector3(position.X, position.Y + height, position.Z),
				new Vector3(position.X + width, position.Y + height, position.Z),
				texture, color)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TexturedQuad"/> struct.
		/// </summary>
		/// <param name="ulPosition">The upper-left position.</param>
		/// <param name="urPosition">The upper-right position.</param>
		/// <param name="llPosition">The lower-left position.</param>
		/// <param name="lrPosition">The lower-right position.</param>
		/// <param name="texture">The texture.</param>
		public TexturedQuad(
			Vector3 ulPosition, Vector3 urPosition, Vector3 llPosition, Vector3 lrPosition,
			Texture2D texture)
			: this(ulPosition, urPosition, llPosition, lrPosition, texture, Color.White, Color.White, Color.White, Color.White)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TexturedQuad"/> struct.
		/// </summary>
		/// <param name="ulPosition">The upper-left position.</param>
		/// <param name="urPosition">The upper-right position.</param>
		/// <param name="llPosition">The lower-left position.</param>
		/// <param name="lrPosition">The lower-right position.</param>
		/// <param name="texture">The texture.</param>
		/// <param name="color">The color.</param>
		public TexturedQuad(
			Vector3 ulPosition, Vector3 urPosition, Vector3 llPosition, Vector3 lrPosition,
			Texture2D texture,
			Color color) : this(ulPosition, urPosition, llPosition, lrPosition, texture, color, color, color, color)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TexturedQuad"/> struct.
		/// </summary>
		/// <param name="ulPosition">The upper-left position.</param>
		/// <param name="urPosition">The upper-right position.</param>
		/// <param name="llPosition">The lower-left position.</param>
		/// <param name="lrPosition">The lower-right position.</param>
		/// <param name="texture">The texture.</param>
		/// <param name="ulColor">Color of the upper-left.</param>
		/// <param name="urColor">Color of the upper-right.</param>
		/// <param name="llColor">Color of the lower-left.</param>
		/// <param name="lrColor">Color of the lower-right.</param>
		public TexturedQuad(
			Vector3 ulPosition, Vector3 urPosition, Vector3 llPosition, Vector3 lrPosition, 
			Texture2D texture, 
			Color ulColor, Color urColor, Color llColor, Color lrColor)
		{
			this.texture = texture;
			this.vertices = new[]
			{
				new VertexPositionColorTexture(ulPosition, ulColor, Vector2.Zero),
				new VertexPositionColorTexture(urPosition, urColor, Vector2.UnitX),
				new VertexPositionColorTexture(llPosition, llColor, Vector2.UnitY),
				new VertexPositionColorTexture(lrPosition, lrColor, Vector2.One)
			};
		}


		/// <summary>
		/// Gets the vertices.
		/// </summary>
		/// <value>
		/// The vertices.
		/// </value>
		public VertexPositionColorTexture[] Vertices
		{
			get { return this.vertices; }
		}

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		public PrimitiveType Type
		{
			get { return PrimitiveType.TriangleList; }
		}

		/// <summary>
		/// Gets the texture.
		/// </summary>
		/// <value>
		/// The texture.
		/// </value>
		public Texture2D Texture
		{
			get { return this.texture; }
		}
	}

	internal struct Rectangle : IShape
	{
		private readonly IColorPrimitive[] primitives;

		/// <summary>
		/// Gets the primitives.
		/// </summary>
		/// <value>
		/// The primitives.
		/// </value>
		public IColorPrimitive[] Primitives
		{
			get { return this.primitives; }
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="Rectangle"/> struct.
		/// </summary>
		/// <param name="upperLeft">The upper left.</param>
		/// <param name="size">The size.</param>
		/// <param name="color">The color.</param>
		public Rectangle(Vector3 upperLeft, float size, Color color)
			: this(upperLeft, size, color, color, color, color)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Rectangle"/> struct.
		/// </summary>
		/// <param name="upperLeft">The upper left.</param>
		/// <param name="size">The size.</param>
		/// <param name="colorA">The color a.</param>
		/// <param name="colorB">The color b.</param>
		/// <param name="colorC">The color c.</param>
		/// <param name="colorD">The color d.</param>
		public Rectangle(Vector3 upperLeft, float size, Color colorA, Color colorB, Color colorC, Color colorD)
			: this(upperLeft, new Vector3(upperLeft.X + size, upperLeft.Y + size, upperLeft.Z), colorA, colorB, colorC, colorD)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Rectangle"/> struct.
		/// </summary>
		/// <param name="upperLeft">The upper left.</param>
		/// <param name="lowerRight">The lower right.</param>
		/// <param name="color">The color.</param>
		public Rectangle(Vector3 upperLeft, Vector3 lowerRight, Color color)
			: this(upperLeft, lowerRight, color, color, color, color)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Rectangle"/> struct.
		/// </summary>
		/// <param name="upperLeft">The upper left.</param>
		/// <param name="lowerRight">The lower right.</param>
		/// <param name="colorA">The color a.</param>
		/// <param name="colorB">The color b.</param>
		/// <param name="colorC">The color c.</param>
		/// <param name="colorD">The color d.</param>
		public Rectangle(Vector3 upperLeft, Vector3 lowerRight, Color colorA, Color colorB, Color colorC, Color colorD)
		{
			IShape rectangleQuad = new Quad(upperLeft,
											  new Vector3(lowerRight.X, upperLeft.Y,
														  Math.Abs(lowerRight.Z - 0) < 0.01 && Math.Abs(upperLeft.Z - 0) < 0.01
															? 0
															: lowerRight.Z + (lowerRight.Z - upperLeft.Z) / 2),
											  lowerRight,
											  new Vector3(upperLeft.X, lowerRight.Y,
														  Math.Abs(lowerRight.Z - 0) < 0.01 && Math.Abs(upperLeft.Z - 0) < 0.01
															? 0
															: lowerRight.Z + (lowerRight.Z - upperLeft.Z) / 2),
											  colorA, colorB, colorC, colorD);
			this.primitives = rectangleQuad.Primitives;
		}
	}

	internal struct RectangleOutline : IShape
	{
		private readonly IColorPrimitive[] primitives;

		/// <summary>
		/// Gets the primitives.
		/// </summary>
		/// <value>
		/// The primitives.
		/// </value>
		public IColorPrimitive[] Primitives
		{
			get { return this.primitives; }
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="RectangleOutline"/> struct.
		/// </summary>
		/// <param name="upperLeft">The upper left.</param>
		/// <param name="size">The size.</param>
		/// <param name="thickness">The thickness.</param>
		/// <param name="color">The color.</param>
		public RectangleOutline(Vector3 upperLeft, float size, float thickness, Color color)
			: this(upperLeft, size, thickness, color, color, color, color)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RectangleOutline"/> struct.
		/// </summary>
		/// <param name="upperLeft">The upper left.</param>
		/// <param name="size">The size.</param>
		/// <param name="thickness">The thickness.</param>
		/// <param name="colorA">The color a.</param>
		/// <param name="colorB">The color b.</param>
		/// <param name="colorC">The color c.</param>
		/// <param name="colorD">The color d.</param>
		public RectangleOutline(Vector3 upperLeft, float size, float thickness, Color colorA, Color colorB, Color colorC, Color colorD)
			: this(upperLeft, new Vector3(upperLeft.X + size, upperLeft.Y + size, upperLeft.Z), thickness, colorA, colorB, colorC, colorD)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RectangleOutline"/> struct.
		/// </summary>
		/// <param name="upperLeft">The upper left.</param>
		/// <param name="lowerRight">The lower right.</param>
		/// <param name="thickness">The thickness.</param>
		/// <param name="color">The color.</param>
		public RectangleOutline(Vector3 upperLeft, Vector3 lowerRight, float thickness, Color color)
			: this(upperLeft, lowerRight, thickness, color, color, color, color)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RectangleOutline"/> struct.
		/// </summary>
		/// <param name="upperLeft">The upper left.</param>
		/// <param name="lowerRight">The lower right.</param>
		/// <param name="thickness">The thickness.</param>
		/// <param name="colorA">The color a.</param>
		/// <param name="colorB">The color b.</param>
		/// <param name="colorC">The color c.</param>
		/// <param name="colorD">The color d.</param>
		public RectangleOutline(Vector3 upperLeft, Vector3 lowerRight, float thickness, Color colorA, Color colorB, Color colorC, Color colorD)
		{
			Vector3 lowerLeft = new Vector3(upperLeft.X, lowerRight.Y,
				Math.Abs(lowerRight.Z - 0) < 0.01 && Math.Abs(upperLeft.Z - 0) < 0.01
					? 0
					: lowerRight.Z + (lowerRight.Z - upperLeft.Z)/2);
			Vector3 upperRight = new Vector3(lowerRight.X, upperLeft.Y,
				Math.Abs(lowerRight.Z - 0) < 0.01 && Math.Abs(upperLeft.Z - 0) < 0.01
					? 0
					: lowerRight.Z + (lowerRight.Z - upperLeft.Z)/2);

			var primitives = new List<IColorPrimitive>();
			if (thickness > 1)
			{
				primitives.AddRange((new LineThick(upperLeft, upperRight, thickness, colorA)).Primitives);
				primitives.AddRange((new LineThick(upperRight, lowerRight, thickness, colorB)).Primitives);
				primitives.AddRange((new LineThick(lowerRight, lowerLeft, thickness, colorC)).Primitives);
				primitives.AddRange((new LineThick(lowerLeft, upperLeft, thickness, colorD)).Primitives);
			}
			else
			{
				primitives.AddRange((new Line(upperLeft, upperRight, colorA)).Primitives);
				primitives.AddRange((new Line(upperRight, lowerRight, colorB)).Primitives);
				primitives.AddRange((new Line(lowerRight, lowerLeft, colorC)).Primitives);
				primitives.AddRange((new Line(lowerLeft, upperLeft, colorD)).Primitives);
			}

			this.primitives = primitives.ToArray();
		}
	}

	internal struct EllipseOutline : IShape
	{
		private readonly IColorPrimitive[] primitives;

		/// <summary>
		/// Gets the primitives.
		/// </summary>
		/// <value>
		/// The primitives.
		/// </value>
		public IColorPrimitive[] Primitives
		{
			get { return this.primitives; }
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="EllipseOutline"/> struct.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="thickness">The thickness.</param>
		/// <param name="color">The color.</param>
		public EllipseOutline(Vector3 position, float width, float height, float thickness, Color color)
			: this(position, width, height, thickness, color, EllipseHelper.CalculateNumberOfSides(width, height), 0)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EllipseOutline"/> struct.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="thickness">The thickness.</param>
		/// <param name="color">The color.</param>
		/// <param name="sides">The number of sides.</param>
		/// <param name="rotation">The rotation.</param>
		public EllipseOutline(Vector3 position, float width, float height, float thickness, Color color, int sides, float rotation)
		{
			Vector3[] points = EllipseHelper.GetEllipsePoints(width, height, rotation, sides);

			if (thickness > 1f)
			{
				var primitives = new List<IColorPrimitive>();
				for (int index = 0; index < sides - 1; index++)
					primitives.AddRange((new LineThick(position + points[index], position + points[index + 1], thickness, color)).Primitives);
				primitives.AddRange((new LineThick(position + points[sides - 1], position + points[0], thickness, color)).Primitives);
				this.primitives = primitives.ToArray();
			}
			else
			{
				this.primitives = new IColorPrimitive[sides];
				for (int index = 0; index < sides - 1; index++)
					this.primitives[index] = new Line(position + points[index], position + points[index + 1], color);
				this.primitives[sides - 1] = new Line(position + points[sides - 1], position + points[0], color);
            }
		}
	}

	internal struct Ellipse : IShape
	{
		private readonly IColorPrimitive[] primitives;

		/// <summary>
		/// Gets the primitives.
		/// </summary>
		/// <value>
		/// The primitives.
		/// </value>
		public IColorPrimitive[] Primitives
		{
			get { return this.primitives; }
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="Ellipse"/> struct.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="color">The color.</param>
		public Ellipse(Vector3 position, float width, float height, Color color) 
			: this(position, width, height, color, EllipseHelper.CalculateNumberOfSides(width, height), 0)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Ellipse"/> struct.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="color">The color.</param>
		/// <param name="sides">The number of sides.</param>
		/// <param name="rotation">The rotation.</param>
		public Ellipse(Vector3 position, float width, float height, Color color, int sides, float rotation)
		{
			this.primitives = new IColorPrimitive[sides];
			var points = EllipseHelper.GetEllipsePoints(width, height, rotation, sides);

			for (int index = 0; index < sides - 1; index++)
			{
				this.primitives[index] = new Triangle(position, position + points[index], position + points[index + 1], color);
			}
			this.primitives[sides - 1] = new Triangle(position, position + points[sides - 1], position + points[0], color);
		}
	}

	internal static class EllipseHelper
	{
		public static int CalculateNumberOfSides(float width, float height)
		{
			return width + height < 25 ? 12 : (int)((width + height) / 2);
		}

		public static Vector3[] GetEllipsePoints(float width, float height, float rotation, int sides)
		{
			const float pi2 = (float)Math.PI * 2f;

			Vector3[] points = new Vector3[sides];

			float step = pi2 / sides;
			int currentPoint = 0;

			for (float t = 0f; currentPoint < sides; t += step)
			{
				points[currentPoint++] = new Vector3((float)(width * Math.Cos(t)), (float)(height * Math.Sin(t)), 0f);
			}

			if (Math.Abs(rotation - 0) > float.Epsilon)
			{
				Matrix transform = Matrix.CreateRotationY(rotation);
				for (int i = 0; i < sides; i++)
				{
					points[i] = Vector3.Transform(points[i], transform);
				}
			}

			return points;
		}
	}

	internal struct LineThick : IShape
	{
		private readonly Quad lineQuad;

		/// <summary>
		/// Gets the primitives.
		/// </summary>
		/// <value>
		/// The primitives.
		/// </value>
		public IColorPrimitive[] Primitives
		{
			get { return this.lineQuad.Primitives; }
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="LineThick"/> struct.
		/// </summary>
		/// <param name="startPoint">The start point.</param>
		/// <param name="endPoint">The end point.</param>
		/// <param name="thickness">The thickness.</param>
		/// <param name="color">The color.</param>
		public LineThick(Vector3 startPoint, Vector3 endPoint, float thickness, Color color)
		{
			Vector3 lineDirection = endPoint - startPoint;
			Vector3 lineCross = Vector3.Cross(lineDirection, Vector3.Backward);
			Vector3 lineNormal = Vector3.Normalize(lineCross);
			Vector3 lineNormalHalf = lineNormal * thickness * 0.5f;

			Vector3 upperLeft = new Vector3(startPoint.X + lineNormalHalf.X + lineNormalHalf.Y, startPoint.Y + lineNormalHalf.Y - lineNormalHalf.X, 0);
			Vector3 lowerLeft = new Vector3(startPoint.X - lineNormalHalf.X + lineNormalHalf.Y, startPoint.Y - lineNormalHalf.Y - lineNormalHalf.X, 0);
			Vector3 upperRight = new Vector3(endPoint.X + lineNormalHalf.X - lineNormalHalf.Y, endPoint.Y + lineNormalHalf.Y + lineNormalHalf.X, 0);
			Vector3 lowerRight = new Vector3(endPoint.X - lineNormalHalf.X - lineNormalHalf.Y, endPoint.Y - lineNormalHalf.Y + lineNormalHalf.X, 0);

			this.lineQuad = new Quad(upperLeft, upperRight, lowerRight, lowerLeft, color);
		}
	}

	internal struct Quad : IShape
	{
		private readonly IColorPrimitive[] primitives;

		/// <summary>
		/// Gets the primitives.
		/// </summary>
		/// <value>
		/// The primitives.
		/// </value>
		public IColorPrimitive[] Primitives
		{
			get { return this.primitives; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Quad"/> struct.
		/// </summary>
		/// <param name="pointA">The point a. Upper-left</param>
		/// <param name="pointB">The point b. Upper-right</param>
		/// <param name="pointC">The point c. Lower-right</param>
		/// <param name="pointD">The point d. Lower-left</param>
		/// <param name="color">The color.</param>
		public Quad(Vector3 pointA, Vector3 pointB, Vector3 pointC, Vector3 pointD, Color color)
			: this(pointA, pointB, pointC, pointD, color, color, color, color)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Quad"/> struct.
		/// </summary>
		/// <param name="pointA">The point a. Upper-left</param>
		/// <param name="pointB">The point b. Upper-right</param>
		/// <param name="pointC">The point c. Lower-right</param>
		/// <param name="pointD">The point d. Lower-left</param>
		/// <param name="colorA">The color a.</param>
		/// <param name="colorB">The color b.</param>
		/// <param name="colorC">The color c.</param>
		/// <param name="colorD">The color d.</param>
		public Quad(Vector3 pointA, Vector3 pointB, Vector3 pointC, Vector3 pointD, Color colorA, Color colorB, Color colorC,
			Color colorD)
			: this()
		{
			this.primitives = new IColorPrimitive[]
			{
				new Triangle(pointA, pointB, pointD, colorA, colorB, colorD),
				new Triangle(pointB, pointC, pointD, colorB, colorC, colorD)
			};
		}
	}

	/// <summary>
	/// Basic line primitive.
	/// Line primitive doesn't have thickness (1px thick) and may only be colored using two colors.
	/// </summary>
	internal struct Line : IColorPrimitive, IShape
	{
		private readonly VertexPositionColor[] vertices;

		/// <summary>
		/// Gets the primitives.
		/// </summary>
		/// <value>
		/// The primitives.
		/// </value>
		public IColorPrimitive[] Primitives
		{
			get { return new[] {(IColorPrimitive) this}; }
		}

		/// <summary>
		/// Gets the vertices.
		/// </summary>
		/// <value>
		/// The vertices.
		/// </value>
		public VertexPositionColor[] Vertices
		{
			get { return this.vertices; }
		}

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		public PrimitiveType Type
		{
			get { return PrimitiveType.LineList; }
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="Line"/> struct.
		/// </summary>
		/// <param name="pointA">The point A position.</param>
		/// <param name="pointB">The point B position.</param>
		/// <param name="color">The color of both point A and point B.</param>
		public Line(Vector3 pointA, Vector3 pointB, Color color)
			: this(pointA, pointB, color, color)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Line"/> struct.
		/// </summary>
		/// <param name="pointA">The point A position.</param>
		/// <param name="pointB">The point B position.</param>
		/// <param name="colorA">The color of point A.</param>
		/// <param name="colorB">The color of point B.</param>
		public Line(Vector3 pointA, Vector3 pointB, Color colorA, Color colorB)
		{
			this.vertices = new[]
			{
				new VertexPositionColor(pointA, colorA),
				new VertexPositionColor(pointB, colorB)
			};
		}
	}

	/// <summary>
	/// Basic triangle primitive.
	/// Triangle primitive doesn't have thickness (1px thick) and my only be colored using three colors.
	/// </summary>
	internal struct Triangle : IColorPrimitive, IShape
	{
		private readonly VertexPositionColor[] vertices;

		/// <summary>
		/// Gets the primitives.
		/// </summary>
		/// <value>
		/// The primitives.
		/// </value>
		public IColorPrimitive[] Primitives
		{
			get { return new[] { (IColorPrimitive)this }; }
		}

		/// <summary>
		/// Gets the vertices.
		/// </summary>
		/// <value>
		/// The vertices.
		/// </value>
		public VertexPositionColor[] Vertices
		{
			get { return this.vertices; }
		}

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		public PrimitiveType Type
		{
			get { return PrimitiveType.TriangleList; }
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="Triangle"/> struct.
		/// </summary>
		/// <param name="pointA">The point A position.</param>
		/// <param name="pointB">The point B position.</param>
		/// <param name="pointC">The point C position.</param>
		/// <param name="color">The color of all three points (point A, point B and point C).</param>
		public Triangle(Vector3 pointA, Vector3 pointB, Vector3 pointC, Color color)
			: this(pointA, pointB, pointC, color, color, color)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Triangle"/> struct.
		/// </summary>
		/// <param name="pointA">The point A position.</param>
		/// <param name="pointB">The point B position.</param>
		/// <param name="pointC">The point C position.</param>
		/// <param name="colorA">The color of point A.</param>
		/// <param name="colorB">The color of point B.</param>
		/// <param name="colorC">The color of point C.</param>
		public Triangle(Vector3 pointA, Vector3 pointB, Vector3 pointC, Color colorA, Color colorB, Color colorC)
		{
			this.vertices = new[]
			{
				new VertexPositionColor(pointA, colorA),
				new VertexPositionColor(pointB, colorB),
				new VertexPositionColor(pointC, colorC)
			};
		}
	}
}

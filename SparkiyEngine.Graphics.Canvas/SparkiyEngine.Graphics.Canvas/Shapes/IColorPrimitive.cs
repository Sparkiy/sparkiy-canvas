using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SparkiyEngine.Graphics.Canvas.Shapes
{
	internal interface IShape
	{
		IColorPrimitive[] Primitives { get; }
	}

	internal interface IColorPrimitive
	{
		VertexPositionColor[] Vertices { get; }

		PrimitiveType Type { get; }
	}

	internal struct Rectangle : IShape
	{
		private readonly IColorPrimitive[] primitives;

		public IColorPrimitive[] Primitives
		{
			get { return this.primitives; }
		}


		public Rectangle(Vector3 upperLeft, float size, Color color)
			: this(upperLeft, size, color, color, color, color)
		{
		}

		public Rectangle(Vector3 upperLeft, float size, Color colorA, Color colorB, Color colorC, Color colorD)
			: this(upperLeft, new Vector3(upperLeft.X + size, upperLeft.Y + size, upperLeft.Z), colorA, colorB, colorC, colorD)
		{
		}

		public Rectangle(Vector3 upperLeft, Vector3 lowerRight, Color color)
			: this(upperLeft, lowerRight, color, color, color, color)
		{
		}

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

	internal struct Quad : IShape
	{
		private readonly IColorPrimitive[] primitives;

		public IColorPrimitive[] Primitives
		{
			get { return this.primitives; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Quad"/> struct.
		/// </summary>
		/// <param name="pointA">The point a.</param>
		/// <param name="pointB">The point b.</param>
		/// <param name="pointC">The point c.</param>
		/// <param name="pointD">The point d.</param>
		/// <param name="color">The color.</param>
		public Quad(Vector3 pointA, Vector3 pointB, Vector3 pointC, Vector3 pointD, Color color)
			: this(pointA, pointB, pointC, pointD, color, color, color, color)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Quad"/> struct.
		/// </summary>
		/// <param name="pointA">The point a.</param>
		/// <param name="pointB">The point b.</param>
		/// <param name="pointC">The point c.</param>
		/// <param name="pointD">The point d.</param>
		/// <param name="colorA">The color a.</param>
		/// <param name="colorB">The color b.</param>
		/// <param name="colorC">The color c.</param>
		/// <param name="colorD">The color d.</param>
		public Quad(Vector3 pointA, Vector3 pointB, Vector3 pointC, Vector3 pointD, Color colorA, Color colorB, Color colorC,
			Color colorD)
			: this()
		{
			this.primitives = new IColorPrimitive[2]
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
		private const Int32 VerticesCount = 2;
		private readonly VertexPositionColor[] vertices;

		public IColorPrimitive[] Primitives
		{
			get { return new[] {(IColorPrimitive) this}; }
		}

		public VertexPositionColor[] Vertices
		{
			get { return this.vertices; }
		}

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
			this.vertices = new VertexPositionColor[VerticesCount]
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
		private const Int32 PrimitivesCount = 1;
		private const Int32 VerticesCount = 3;
		private readonly VertexPositionColor[] vertices;

		public IColorPrimitive[] Primitives
		{
			get { return new[] { (IColorPrimitive)this }; }
		}

		public VertexPositionColor[] Vertices
		{
			get { return this.vertices; }
		}

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
			this.vertices = new VertexPositionColor[VerticesCount]
			{
				new VertexPositionColor(pointA, colorA),
				new VertexPositionColor(pointB, colorB),
				new VertexPositionColor(pointC, colorC)
			};
		}
	}
}

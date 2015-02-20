using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SparkiyEngine.Graphics.Canvas.Shapes
{
	internal interface IColorComplex
	{
		IColorPrimitive[] Primitives { get; }
	}

	internal interface IColorPrimitive
	{
		VertexPositionColor[] Vertices { get; }

		PrimitiveType Type { get; }
	}

	internal struct Quad : IColorComplex
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
	internal struct Line : IColorPrimitive
	{
		private const Int32 PrimitivesCount = 1;
		private const Int32 VerticesCount = 2;
		private readonly VertexPositionColor[] vertices;

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
	internal struct Triangle : IColorPrimitive
	{
		private const Int32 PrimitivesCount = 1;
		private const Int32 VerticesCount = 3;
		private readonly VertexPositionColor[] vertices;

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

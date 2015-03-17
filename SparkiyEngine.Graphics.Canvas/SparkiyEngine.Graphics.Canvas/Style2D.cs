using Microsoft.Xna.Framework;

namespace SparkiyEngine.Graphics.Canvas
{
	internal class Style2D
	{
		private static readonly Color DefaultStrokeColor = new Color(new Vector4(0, 0, 0, 1));
		private static readonly Color DefaultFillColor = new Color(new Vector4(0, 0, 0, 1));
		private static readonly Color DefaultFontColor = new Color(new Vector4(0, 0, 0, 1));


		/// <summary>
		/// Initializes a new instance of the <see cref="Style2D"/> class.
		/// </summary>
		public Style2D()
		{
			this.StrokeColor = DefaultStrokeColor;
			this.StrokeThickness = 2f;
			this.IsStrokeEnabled = false;

			this.FillColor = DefaultFillColor;
			this.IsFillEnabled = true;

			this.FontFamily = "Segoe UI";
			this.FontSize = 24f;
			this.FontColor = DefaultFontColor;
		}


		/// <summary>
		/// Gets or sets the color of the stroke.
		/// </summary>
		/// <value>
		/// The color of the stroke.
		/// </value>
		public Color StrokeColor { get; set; }

		/// <summary>
		/// Gets or sets the stroke thickness.
		/// </summary>
		/// <value>
		/// The stroke thickness.
		/// </value>
		public float StrokeThickness { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether stroke is enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if stroke is enabled; otherwise, <c>false</c>.
		/// </value>
		public bool IsStrokeEnabled { get; set; }

		/// <summary>
		/// Gets or sets the color of the fill.
		/// </summary>
		/// <value>
		/// The color of the fill.
		/// </value>
		public Color FillColor { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether fill is enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if fill is enabled; otherwise, <c>false</c>.
		/// </value>
		public bool IsFillEnabled { get; set; }

		/// <summary>
		/// Gets or sets the font family.
		/// </summary>
		/// <value>
		/// The font family.
		/// </value>
		public string FontFamily { get; set; }

		/// <summary>
		/// Gets or sets the size of the font.
		/// </summary>
		/// <value>
		/// The size of the font.
		/// </value>
		public float FontSize { get; set; }

		/// <summary>
		/// Gets or sets the color of the font.
		/// </summary>
		/// <value>
		/// The color of the font.
		/// </value>
		public Color FontColor { get; set; }
	}
}
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SparkiyEngine.Graphics.Canvas.Fonts
{
	internal class FontManager
    {
	    protected readonly bool IsFontGeneratorAvailable;
	    protected readonly IFontGenerator Generator;

		protected readonly Dictionary<FontDescription, Texture2D> Fonts = new Dictionary<FontDescription, Texture2D>();


		/// <summary>
		/// Initializes a new instance of the <see cref="FontManager"/> class.
		/// </summary>
		/// <param name="generator">
		/// The font generator instance. 
		/// If this is <c>null</c>, font manager will be unable to generate new fonts and will throw <see cref="FontGeneratorUnavailableException"/>
		/// </param>
	    public FontManager(IFontGenerator generator)
	    {
		    this.Generator = generator;
		    this.IsFontGeneratorAvailable = this.Generator != null;
	    }


		/// <summary>
		/// Gets the font matching given description.
		/// </summary>
		/// <param name="description">The font description.</param>
		/// <returns>Returns requested font</returns>
		/// <remarks>
		/// This method will generate font only if font generator is available.
		/// If font generator is not available, it will throw <see cref="FontGeneratorUnavailableException"/>.
		/// </remarks>
		public Texture2D GetFont(FontDescription description)
		{
			if (!this.Fonts.ContainsKey(description))
			{
				this.CheckFontGeneratorAvailable();

				var font = this.Generator.GenerateFont(description);
				this.RegisterFont(description, font);
			}

			return this.Fonts[description];
		}

		/// <summary>
		/// Registers the font.
		/// </summary>
		/// <param name="description">The description.</param>
		/// <param name="font">The font.</param>
		public void RegisterFont(FontDescription description, Texture2D font)
		{
			this.Fonts[description] = font;
		}

		/// <summary>
		/// Generates the fonts.
		/// </summary>
		/// <param name="descriptions">The font descriptions.</param>
		/// <remarks>
		/// This method will generate fonts only if font generator is available.
		/// If font generator is not available, it will throw <see cref="FontGeneratorUnavailableException"/>.
		/// </remarks>
		public void GenerateFonts(IEnumerable<FontDescription> descriptions)
		{
			this.CheckFontGeneratorAvailable();

			foreach (var fontDescription in descriptions)
				this.RegisterFont(
					fontDescription,
					this.Generator.GenerateFont(fontDescription));
		}

		/// <summary>
		/// Checks whether the font generator is available; if not, it will throw exception and execution will not continue.
		/// </summary>
		/// <exception cref="FontGeneratorUnavailableException">Font generator instance was not passed to object constructor. Font manager can't generate new fonts.</exception>
		protected void CheckFontGeneratorAvailable()
		{
			if (!this.IsFontGeneratorAvailable)
				throw new FontGeneratorUnavailableException();
		}
    }
}

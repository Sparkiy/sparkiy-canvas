using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SparkiyEngine.Graphics.Canvas.Fonts
{
	internal interface IFontGenerator
	{
		Texture2D GenerateFont(FontDescription description);
	}
}

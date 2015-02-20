using System;

namespace SparkiyEngine.Graphics.Canvas.Fonts
{
	internal struct FontDescription : IEquatable<FontDescription>
	{
		/// <summary>
		/// Gets or sets the font family.
		/// </summary>
		/// <value>
		/// The family.
		/// </value>
		public string Family { get; set; }

		/// <summary>
		/// Gets or sets the font size.
		/// </summary>
		/// <value>
		/// The size.
		/// </value>
		public double Size { get; set; }

		/// <summary>
		/// Gets or sets the font weight.
		/// </summary>
		/// <value>
		/// The weight.
		/// </value>
		public FontWeights Weight { get; set; }

		/// <summary>
		/// Gets or sets the font style.
		/// </summary>
		/// <value>
		/// The style.
		/// </value>
		public FontStyles Style { get; set; }


		#region | IEquatable implementation |

		/// <summary>
		/// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is FontDescription && Equals((FontDescription) obj);
		}

		/// <summary>
		/// Equalses the specified other.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <returns></returns>
		public bool Equals(FontDescription other)
		{
			return string.Equals(Family, other.Family) && Size.Equals(other.Size) && Weight == other.Weight && Style == other.Style;
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (Family != null ? Family.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ Size.GetHashCode();
				hashCode = (hashCode * 397) ^ (int)Weight;
				hashCode = (hashCode * 397) ^ (int)Style;
				return hashCode;
			}
		}

		#endregion | IEquatable implementation |
	}
}
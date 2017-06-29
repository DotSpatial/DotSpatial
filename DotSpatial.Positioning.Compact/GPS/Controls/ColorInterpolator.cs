using System;
using System.Drawing;
#if !PocketPC || DesignTime
using System.ComponentModel;
#endif
using System.Reflection;
using System.Xml.Serialization;

namespace DotSpatial.Positioning
{
	/// <summary>Calculates intermediate colors between two other colors.</summary>
	/// <remarks>
	/// 	<para>This class is used to create a smooth transition from one color to another.
	///     After specifying a start color, end color, and number of intervals, the indexer
	///     will return a calculated <strong>Color</strong>. Specifying a greater number of
	///     intervals creates a smoother color gradient.</para>
	/// 	<para>Instances of this class are guaranteed to be thread-safe because the class
	///     uses thread synchronization.</para>
	/// 	<para>On the .NET Compact Framework, the alpha channel is not supported.</para>
	/// </remarks>
	/// <example>
	///     This example uses a <strong>ColorInterpolator</strong> to calculate ten colors
	///     between (and including) <strong>Blue</strong> and <strong>Red</strong> . 
	///     <code lang="VB" title="[New Example]">
	/// ' Create a New color interpolator
	/// Dim Interpolator As New ColorInterpolator(Color.Blue, Color.Red, 10)
	/// ' Output Each calculated color
	/// Dim i As Integer
	/// For i = 0 To 9
	///     ' Get the Next color In the sequence
	///     Dim NewColor As Color = Interpolator(i)
	///     ' Output RGB values of this color
	///     Debug.Write(NewColor.R.ToString() + ",")
	///     Debug.Write(NewColor.G.ToString() + ",")
	///     Debug.WriteLine(NewColor.B.ToString())
	/// Next i
	///     </code>
	/// 	<code lang="CS" title="[New Example]">
	/// // Create a new color interpolator
	/// ColorInterpolator Interpolator = new ColorInterpolator(Color.Blue, Color.Red, 10);
	/// // Output each calculated color
	/// for(int i = 0; i &lt; 10; i++)
	/// {
	///     // Get the next color in the sequence
	///     Color NewColor = Interpolator[i];
	///     // Output RGB values of this color
	///     Console.Write(NewColor.R.ToString() + ",");
	///     Console.Write(NewColor.G.ToString() + ",");
	///     Console.WriteLine(NewColor.B.ToString());
	/// }
	///     </code>
	/// </example>
#if !PocketPC || DesignTime
#if Framework20
    [Obfuscation(Feature = "renaming", Exclude = false, ApplyToMembers = true)]
    [Obfuscation(Feature = "controlflow", Exclude = true, ApplyToMembers = true)]
    [Obfuscation(Feature = "stringencryption", Exclude = false, ApplyToMembers = true)]
#endif
	[TypeConverter(typeof(ExpandableObjectConverter))]
    [ImmutableObject(false)]
    [Serializable()]
#endif
	public sealed class ColorInterpolator
    {
#if !PocketPC
		private Interpolator A = new Interpolator();
#endif
        private Interpolator R = new Interpolator();
		private Interpolator G = new Interpolator();
		private Interpolator B = new Interpolator();

		/// <summary>Creates a new instance.</summary>
		/// <param name="startColor">A <strong>Color</strong> at the start of the sequence.</param>
		/// <param name="endColor">A <strong>Color</strong> at the end of the sequence.</param>
		/// <param name="count">
		/// The total number of colors in the sequence, including the start and end
		/// colors.
		/// </param>
		public ColorInterpolator(Color startColor, Color endColor, int count)
		{
			this.Count = count;
			this.StartColor = startColor;
			this.EndColor = endColor;
		}

		/// <summary>Returns a calculated color in the sequence.</summary>
		/// <value>A <strong>Color</strong> value representing a calculated color.</value>
		/// <example>
		///     This example creates a new color interpolator between blue and red, then accesses
		///     the sixth item in the sequence. 
		///     <code lang="VB" title="[New Example]">
		/// ' Create a New color interpolator
		/// Dim Interpolator As New ColorInterpolator(Color.Blue, Color.Red, 10)
		/// ' Access the sixth item
		/// Color CalculatedColor = Interpolator(5);
		///     </code>
		/// 	<code lang="CS" title="[New Example]">
		/// // Create a New color interpolator
		/// ColorInterpolator Interpolator = new ColorInterpolator(Color.Blue, Color.Red, 10);
		/// // Access the sixth item
		/// Color CalculatedColor = Interpolator[5];
		///     </code>
		/// </example>
		/// <param name="index">
		/// An <strong>Integer</strong> between 0 and <strong>Count</strong> minus
		/// one.
		/// </param>
		public Color this[int index]
		{
			get
			{
#if PocketPC
                return Color.FromArgb((byte)R[index], (byte)G[index], (byte)B[index]);
#else
				return Color.FromArgb((byte)A[index], (byte)R[index], (byte)G[index], (byte)B[index]);
#endif
			}
		}

		/// <summary>
		/// Controls the interpolation technique used to calculate intermediate
		/// colors.
		/// </summary>
		/// <value>
		/// An <strong>InterpolationMethod</strong> value indicating the interpolation
		/// technique. Default is <strong>Linear</strong>.
		/// </value>
		/// <remarks>
		/// This property controls the rate at which the start color transitions to the end
		/// color. Values other than Linear can "accelerate" and/or "decelerate" towards the end
		/// color.
		/// </remarks>
		public InterpolationMethod InterpolationMethod
		{
			get
			{
				return R.InterpolationMethod;
			}
			set
			{
#if !PocketPC
				A.InterpolationMethod = value;
#endif
				R.InterpolationMethod = value;
				G.InterpolationMethod = value;
				B.InterpolationMethod = value;
			}
		}

		/// <summary>Controls the first color in the sequence.</summary>
		/// <value>
		/// A <strong>Color</strong> object representing the first color in the
		/// sequence.
		/// </value>
		/// <remarks>Changing this property causes the entire sequence to be recalculated.</remarks>
		/// <example>
		/// This example changes the start color from Green to Orange.
		/// </example>
		public Color StartColor
		{
			get
			{
#if PocketPC
                return Color.FromArgb((byte)R.Minimum, (byte)G.Minimum, (byte)B.Minimum);
#else
				return Color.FromArgb((byte)A.Minimum, (byte)R.Minimum, (byte)G.Minimum, (byte)B.Minimum);
#endif
			}
			set
			{
#if !PocketPC
				A.Minimum = value.A;
#endif
				R.Minimum = value.R;
				G.Minimum = value.G;
				B.Minimum = value.B;
			}
		}

		/// <value>
		/// A <strong>Color</strong> object representing the last color in the
		/// sequence.
		/// </value>
		/// <summary>Controls the last color in the sequence.</summary>
		/// <remarks>Changing this property causes the entire sequence to be recalculated.</remarks>
		public Color EndColor
		{
			get
			{
#if PocketPC
				return Color.FromArgb((byte)R.Maximum, (byte)G.Maximum, (byte)B.Maximum);
#else
				return Color.FromArgb((byte)A.Maximum, (byte)R.Maximum, (byte)G.Maximum, (byte)B.Maximum);
#endif
            }
            set
			{
#if !PocketPC
				A.Maximum = value.A;
#endif
				R.Maximum = value.R;
				G.Maximum = value.G;
				B.Maximum = value.B;
			}
		}

		/// <summary>Controls the number of colors in the sequence.</summary>
		/// <remarks>Changing this property causes the entire sequence to be recalculated.</remarks>
		/// <value>
		/// An <strong>Integer</strong> indicating the total number of colors, including the
		/// start and end colors.
		/// </value>
		public int Count
		{
			get
			{
				return R.Count;
			}
			set
			{
#if !PocketPC
				A.Count = value;
#endif
				R.Count = value;
				G.Count = value;
				B.Count = value;
			}
		}
	}
}

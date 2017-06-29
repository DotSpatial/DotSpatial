// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.Forms.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from http://gps3.codeplex.com/ version 3.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GPS.Net 3.0
// | Shade1974 (Ted Dunsford) | 10/22/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************
using System;
using System.Drawing;

#if !PocketPC || DesignTime

using System.ComponentModel;

#endif

using System.Reflection;

namespace DotSpatial.Positioning.Forms
{
#if !PocketPC || DesignTime
#if Framework20

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
    /// for (int i = 0; i &lt; 10; i++)
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
    [Obfuscation(Feature = "renaming", Exclude = false, ApplyToMembers = true)]
    [Obfuscation(Feature = "controlflow", Exclude = true, ApplyToMembers = true)]
    [Obfuscation(Feature = "stringencryption", Exclude = false, ApplyToMembers = true)]
#endif
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [ImmutableObject(false)]
    [Serializable]
#endif
    public sealed class ColorInterpolator
    {
#if !PocketPC
        private readonly Interpolator _a = new Interpolator();
#endif
        private readonly Interpolator _r = new Interpolator();
        private readonly Interpolator _g = new Interpolator();
        private readonly Interpolator _b = new Interpolator();

        /// <summary>Creates a new instance.</summary>
        /// <param name="startColor">A <strong>Color</strong> at the start of the sequence.</param>
        /// <param name="endColor">A <strong>Color</strong> at the end of the sequence.</param>
        /// <param name="count">
        /// The total number of colors in the sequence, including the start and end
        /// colors.
        /// </param>
        public ColorInterpolator(Color startColor, Color endColor, int count)
        {
            Count = count;
            StartColor = startColor;
            EndColor = endColor;
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
                return Color.FromArgb((byte)_a[index], (byte)_r[index], (byte)_g[index], (byte)_b[index]);
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
                return _r.InterpolationMethod;
            }
            set
            {
#if !PocketPC
                _a.InterpolationMethod = value;
#endif
                _r.InterpolationMethod = value;
                _g.InterpolationMethod = value;
                _b.InterpolationMethod = value;
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
                return Color.FromArgb((byte)_a.Minimum, (byte)_r.Minimum, (byte)_g.Minimum, (byte)_b.Minimum);
#endif
            }
            set
            {
#if !PocketPC
                _a.Minimum = value.A;
#endif
                _r.Minimum = value.R;
                _g.Minimum = value.G;
                _b.Minimum = value.B;
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
                return Color.FromArgb((byte)_a.Maximum, (byte)_r.Maximum, (byte)_g.Maximum, (byte)_b.Maximum);
#endif
            }
            set
            {
#if !PocketPC
                _a.Maximum = value.A;
#endif
                _r.Maximum = value.R;
                _g.Maximum = value.G;
                _b.Maximum = value.B;
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
                return _r.Count;
            }
            set
            {
#if !PocketPC
                _a.Count = value;
#endif
                _r.Count = value;
                _g.Count = value;
                _b.Count = value;
            }
        }
    }
}
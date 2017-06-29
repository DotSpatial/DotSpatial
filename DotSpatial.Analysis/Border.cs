// *******************************************************************************************************
// Product: DotSpatial.Analysis.Border.cs
// Description:  Simple struct for holding a line segment. May be redundant with segments defined in 
// DotSpatial.Topology. 
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// Dan Ames           |  3/2013            |  Adding this header.  
// *******************************************************************************************************
namespace DotSpatial.Analysis
{
    /// <summary>
    /// Used to represent a line segment.
    /// </summary>
    public struct Border
    {
        /// <summary>
        /// Gets or sets the x1.
        /// </summary>
        /// <value>
        /// The x1.
        /// </value>
        public double X1 { get; set; }

        /// <summary>
        /// Gets or sets the x2.
        /// </summary>
        /// <value>
        /// The x2.
        /// </value>
        public double X2 { get; set; }

        /// <summary>
        /// Gets or sets the M.
        /// </summary>
        /// <value>
        /// The M.
        /// </value>
        public double M { get; set; }

        /// <summary>
        /// Gets or sets the Q.
        /// </summary>
        /// <value>
        /// The Q.
        /// </value>
        public double Q { get; set; }
    }
}
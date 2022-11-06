// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Projections;

namespace DotSpatial.Plugins.CoordinateConverter
{
    public partial class FormCoordConverter : Form
    {
        AppManager appManager;

        public FormCoordConverter()
        {
            InitializeComponent();
        }

        public FormCoordConverter(AppManager app)
        {
            InitializeComponent();
            appManager = app;
        }

        private void FormCoordConverter_Load(object sender, EventArgs e)
        {

        }

        private void btnConvert_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //''' <summary>
        //''' this routine converts extents from a source projection to WGS84 returns an Extents object.
        //''' </summary>
        //''' <param name="coordBl">bottom left coordinate object</param>
        //''' <param name="coordUr">upper right coordinate object</param>
        //''' <param name="sourceProj">the source projection</param>
        //''' <returns>reprojected extents</returns>
        //''' <remarks></remarks>
        //Public Function ConvertExtentsToWgs84(ByVal coordBl As Coordinate, ByVal coordUr As Coordinate, ByVal sourceProj As ProjectionInfo) As Extent
        //    'create arrays of the input coordinates
        //    Dim xy() As Double = { coordBl.X, coordBl.Y, coordUr.X, coordUr.Y }
        //    Dim z() As Double = { coordBl.Z, coordUr.Z }

        //    'now we reproject the coordinates from the source projection to WGS84
        //    Reproject.ReprojectPoints(xy, z, sourceProj, prjWgs84, 0, 2)

        //    'create reprojected coordinates for ease of use
        //    Dim blRpj As New Coordinate(xy(0), xy(1), z(0))
        //    Dim urRpj As New Coordinate(xy(2), xy(3), z(1))

        //    'create new extents with the reprojected coordinates and return as an object
        //    Dim ext As Extent = New Extent(blRpj.X, blRpj.Y, urRpj.X, urRpj.Y)
        //    Return ext
        //End Function


        /////''' <summary>
        /////''' this routine converts extents from a source projection to WGS84 returns an Extents object.
        /////''' </summary>
        /////''' <param name="coordBl">bottom left coordinate object</param>
        /////''' <param name="coordUr">upper right coordinate object</param>
        /////''' <param name="sourceProj">the source projection</param>
        /////''' <returns>reprojected extents</returns>
        //////''' <remarks></remarks>
        
        /// <summary>
        /// 
        /// </summary>
        private void ConvertCoordinates(double xSource, double ySource, double zSource, ProjectionInfo projSource, double xTarget, double yTarget, double zTarget, ProjectionInfo projTarget)
        {

        }





















    }
}

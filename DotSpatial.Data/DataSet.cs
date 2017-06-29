// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/28/2009 12:00:28 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Linq;
using DotSpatial.Projections;

namespace DotSpatial.Data
{
    /// <summary>
    /// DataSet
    /// </summary>
    public class DataSet : DisposeBase, IDataSet
    {
        #region Private Variables

        private volatile static bool _projectionLibraryTested;
        private volatile static bool _canProject;
        private IProgressHandler _progressHandler;
        private ProgressMeter _progressMeter;
        private string _proj4String;

        #endregion

        /// <summary>
        /// Gets whether or not projection is based on having the libraries available.
        /// </summary>
        /// <returns></returns>
        public static bool ProjectionSupported()
        {
            if (_projectionLibraryTested)
            {
                return _canProject;
            }
            _projectionLibraryTested = true;
            _canProject = AppDomain.CurrentDomain.GetAssemblies().Any(d => d.GetName().Name == "DotSpatial.Projections");
            return _canProject;
        }

        #region Constructors

        /// <summary>
        /// Creates a new instance of DataSet
        /// </summary>
        protected DataSet()
        {
            _progressHandler = DataManager.DefaultDataManager.ProgressHandler;
        }

        #endregion

        #region Methods

        /// <summary>
        /// This can be overridden in specific classes if necessary
        /// </summary>
        public virtual void Close()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cached extent variable.  The public Extent is the virtual accessor,
        /// and should not be used from a constructor.  MyExtent is protected, not virtual,
        /// and is only visible to inheriting classes, and can be safely set in the constructor.
        /// </summary>
        protected Extent MyExtent { get; set; }

        /// <summary>
        /// This is an internal place holder to make it easier to pass around a single progress meter
        /// between methods.  This will use lazy instantiation if it is requested before one has
        /// been created.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected ProgressMeter ProgressMeter
        {
            get { return _progressMeter ?? (_progressMeter = new ProgressMeter(ProgressHandler)); }
            set { _progressMeter = value; }
        }

        /// <summary>
        /// Gets or sets the raw projection string for this dataset.  This handles both the
        /// case where projection is unavailable but a projection string needs to
        /// be passed around, and the case when a string is not recognized by the
        /// DotSpatial.Projections library.  This is not format restricted, but should match
        /// the original data source as closely as possible.  Setting this will also set
        ///  the Projection if the Projection library is available and the format successfully
        /// defines a transform by either treating it as an Esri string or a proj4 string.
        /// </summary>
        public string ProjectionString
        {
            get
            {
                if (!String.IsNullOrEmpty(_proj4String)) return _proj4String;
                if (CanReproject) return Projection.ToProj4String();
                return _proj4String;
            }
            set
            {
                if (_proj4String == value) return;
                _proj4String = value;

                var test = ProjectionInfo.FromProj4String(value);
                if (test.Transform == null)
                {
                    // regardless of the result, the "Transform" will be null if this fails.
                    test.TryParseEsriString(value);
                }
                if (test.Transform != null)
                {
                    Projection = test;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the DotSpatial.Projections assembly is loaded
        /// </summary>
        /// <returns>Boolean, true if the value can reproject.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanReproject
        {
            get { return ProjectionSupported() && Projection != null && Projection.IsValid; }
        }

        /// <summary>
        /// Reprojects all of the in-ram vertices of featuresets, or else this
        /// simply updates the "Bounds" of the image object.
        /// This will also update the projection to be the specified projection.
        /// </summary>
        /// <param name="targetProjection">
        /// The projection information to reproject the coordinates to.
        /// </param>
        public virtual void Reproject(ProjectionInfo targetProjection)
        {
        }

        /// <summary>
        /// Gets or sets the string name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the extent for the dataset.  Usages to Envelope were replaced
        /// as they required an explicit using to DotSpatial.Topology which is not
        /// as intuitive.  Extent.ToEnvelope() and new Extent(myEnvelope) convert them.
        /// This is designed to be a virtual member to be overridden by subclasses,
        /// and should not be called directly by the constructor of inheriting classes.
        /// </summary>
        public virtual Extent Extent
        {
            get
            {
                return MyExtent;
            }
            set
            {
                MyExtent = value;
            }
        }

        /// <summary>
        /// Gets or set the projection string
        /// </summary>
        public ProjectionInfo Projection { get; set; }

        /// <summary>
        /// Gets an enumeration specifying if this data supports time, space, both or neither.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SpaceTimeSupport SpaceTimeSupport { get; set; }

        /// <summary>
        /// Gets or sets the string type name that identifies this dataset
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the progress handler to use for internal actions taken by this dataset.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IProgressHandler ProgressHandler
        {
            get
            {
                return _progressHandler;
            }
            set
            {
                _progressHandler = value;
                if (_progressMeter == null && value != null)
                {
                    _progressMeter = new ProgressMeter(_progressHandler);
                }
            }
        }

        /// <summary>
        /// Allows overriding the dispose behavior to handle any resources in addition to what are handled in the
        /// image data class.
        /// </summary>
        /// <param name="disposeManagedResources">A Boolean value that indicates whether the overriding method
        /// should dispose managed resources, or just the unmanaged ones.</param>
        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                Name = null;
                Projection = null;
                TypeName = null;
                _progressHandler = null;
                _progressMeter = null;
            }
            base.Dispose(disposeManagedResources);
        }

        #endregion
    }
}
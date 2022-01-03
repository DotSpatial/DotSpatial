// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using DotSpatial.Projections;
using DotSpatial.Serialization;

namespace DotSpatial.Data
{
    /// <summary>
    /// DataSet.
    /// </summary>
    public class DataSet : DisposeBase, IDataSet
    {
        #region Fields

        private static volatile bool canProject;
        private static volatile bool projectionLibraryTested;
        private string _fileName;
        private IProgressHandler _progressHandler;
        private ProgressMeter _progressMeter;
        private string _proj4String;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSet"/> class.
        /// </summary>
        protected DataSet()
        {
            _progressHandler = DataManager.DefaultDataManager.ProgressHandler;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the DotSpatial.Projections assembly is loaded.
        /// </summary>
        /// <returns>Boolean, true if the value can reproject.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanReproject => ProjectionSupported() && Projection != null && Projection.IsValid;

        /// <summary>
        /// Gets or sets the extent for the dataset. Usages to Envelope were replaced
        /// as they required an explicit using to DotSpatial.Topology which is not
        /// as intuitive. Extent.ToEnvelope() and new Extent(myEnvelope) convert them.
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
        /// Gets or sets the file name of a file based data set. The file name should be the absolute path including
        /// the file extension. For data sets coming from a database or a web service, the Filename property is NULL.
        /// </summary>
        public virtual string Filename
        {
            get
            {
                return _fileName;
            }

            set
            {
                _fileName = string.IsNullOrEmpty(value) ? null : Path.GetFullPath(value);
            }
        }

        /// <summary>
        /// Gets or sets the current file path. This is the relative path relative to the current project folder.
        /// For data sets coming from a database or a web service, the FilePath property is NULL.
        /// This property is used when saving source file information to a DSPX project.
        /// </summary>
        [Serialize("FilePath", ConstructorArgumentIndex = 0, UseCase = SerializeAttribute.UseCases.Both)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string FilePath
        {
            get
            {
                // do not construct FilePath for DataSets without a Filename
                return string.IsNullOrEmpty(Filename) ? null : FilePathUtils.RelativePathTo(Filename);
            }

            set
            {
                Filename = value;
            }
        }

        /// <summary>
        /// Gets or sets the string name.
        /// </summary>
        public string Name { get; set; }

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
        /// Gets or sets the projection string.
        /// </summary>
        public ProjectionInfo Projection { get; set; }

        /// <summary>
        /// Gets or sets the raw projection string for this dataset. This handles both the
        /// case where projection is unavailable but a projection string needs to
        /// be passed around, and the case when a string is not recognized by the
        /// DotSpatial.Projections library. This is not format restricted, but should match
        /// the original data source as closely as possible. Setting this will also set
        ///  the Projection if the Projection library is available and the format successfully
        /// defines a transform by either treating it as an Esri string or a proj4 string.
        /// </summary>
        public string ProjectionString
        {
            get
            {
                if (!string.IsNullOrEmpty(_proj4String)) return _proj4String;
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
        /// Gets or sets the cached extent variable. The public Extent is the virtual accessor,
        /// and should not be used from a constructor. MyExtent is protected, not virtual,
        /// and is only visible to inheriting classes, and can be safely set in the constructor.
        /// </summary>
        protected Extent MyExtent { get; set; }

        /// <summary>
        /// Gets or sets the progress meter. This is an internal place holder to make it easier to pass around a single progress meter
        /// between methods. This will use lazy instantiation if it is requested before one has been created.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected ProgressMeter ProgressMeter
        {
            get
            {
                return _progressMeter ?? (_progressMeter = new ProgressMeter(ProgressHandler));
            }

            set
            {
                _progressMeter = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets whether or not projection is based on having the libraries available.
        /// </summary>
        /// <returns>True, if the projection library was found.</returns>
        public static bool ProjectionSupported()
        {
            if (projectionLibraryTested)
            {
                return canProject;
            }

            projectionLibraryTested = true;
            canProject = AppDomain.CurrentDomain.GetAssemblies().Any(d => d.GetName().Name == "DotSpatial.Projections");
            return canProject;
        }

        /// <summary>
        /// This can be overridden in specific classes if necessary.
        /// </summary>
        public virtual void Close()
        {
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
                _progressHandler = null;
                _progressMeter = null;
            }

            base.Dispose(disposeManagedResources);
        }

        #endregion
    }
}
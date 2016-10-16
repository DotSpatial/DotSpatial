// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/28/2009 12:00:28 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using DotSpatial.Projections;
using DotSpatial.Serialization;

namespace DotSpatial.Data
{
    /// <summary>
    /// DataSet
    /// </summary>
    public class DataSet : DisposeBase, IDataSet
    {
        #region Fields

        private static volatile bool _canProject;
        private static volatile bool _projectionLibraryTested;
        private string _fileName;
        private IProgressHandler _progressHandler;
        private ProgressMeter _progressMeter;
        private string _proj4String;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DataSet
        /// </summary>
        protected DataSet()
        {
            _progressHandler = DataManager.DefaultDataManager.ProgressHandler;
        }

        #endregion

        #region Properties

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
        /// Gets the current file path. This is the relative path relative to the current project folder. 
        /// For data sets coming from a database or a web service, the FilePath property is NULL.
        /// This property is used when saving source file information to a DSPX project.
        /// </summary>
        [Serialize("FilePath", ConstructorArgumentIndex = 0)]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string FilePath
        {
            get
            {
                //do not construct FilePath for DataSets without a Filename
                return string.IsNullOrEmpty(Filename) ? null : RelativePathTo(Filename);
            }
            set
            {
                Filename = value;
            }
        }

        /// <summary>
        /// Gets or sets the cached extent variable.  The public Extent is the virtual accessor,
        /// and should not be used from a constructor.  MyExtent is protected, not virtual,
        /// and is only visible to inheriting classes, and can be safely set in the constructor.
        /// </summary>
        protected Extent MyExtent { get; set; }

        /// <summary>
        /// Gets or sets the string name
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
        /// Gets or set the projection string
        /// </summary>
        public ProjectionInfo Projection { get; set; }

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
        /// Gets an enumeration specifying if this data supports time, space, both or neither.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SpaceTimeSupport SpaceTimeSupport { get; set; }

        /// <summary>
        /// Gets or sets the string type name that identifies this dataset
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TypeName { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This can be overridden in specific classes if necessary
        /// </summary>
        public virtual void Close()
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
                TypeName = null;
                _progressHandler = null;
                _progressMeter = null;
            }
            base.Dispose(disposeManagedResources);
        }

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

        /// <summary>
        /// Creates a relative path from one file or folder to another.
        /// </summary>
        /// <param name="toPath">
        /// Contains the path that defines the endpoint of the relative path.
        /// </param>
        /// <returns>
        /// The relative path from the start directory to the end path.
        /// </returns>
        /// <exception cref="ArgumentNullException">Occurs when the toPath is NULL</exception>
        //http://weblogs.asp.net/pwelter34/archive/2006/02/08/create-a-relative-path-code-snippet.aspx
        public static string RelativePathTo(string toPath)
        {
            string fromDirectory = Directory.GetCurrentDirectory();

            if (toPath == null)
                throw new ArgumentNullException("toPath");

            if (Path.IsPathRooted(fromDirectory) && Path.IsPathRooted(toPath))
            {
                if (string.Compare(Path.GetPathRoot(fromDirectory), Path.GetPathRoot(toPath), true) != 0)
                    return toPath;
            }

            StringCollection relativePath = new StringCollection();
            string[] fromDirectories = fromDirectory.Split(Path.DirectorySeparatorChar);

            string[] toDirectories = toPath.Split(Path.DirectorySeparatorChar);

            int length = Math.Min(fromDirectories.Length, toDirectories.Length);

            int lastCommonRoot = -1;

            // find common root
            for (int x = 0; x < length; x++)
            {
                if (string.Compare(fromDirectories[x], toDirectories[x], true) != 0)
                    break;

                lastCommonRoot = x;
            }
            if (lastCommonRoot == -1)
                return toPath;

            // add relative folders in from path
            for (int x = lastCommonRoot + 1; x < fromDirectories.Length; x++)
                if (fromDirectories[x].Length > 0)
                    relativePath.Add("..");

            // add to folders to path
            for (int x = lastCommonRoot + 1; x < toDirectories.Length; x++)
                relativePath.Add(toDirectories[x]);

            // create relative path
            string[] relativeParts = new string[relativePath.Count];
            relativePath.CopyTo(relativeParts, 0);

            return string.Join(Path.DirectorySeparatorChar.ToString(), relativeParts);
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

        #endregion
    }
}
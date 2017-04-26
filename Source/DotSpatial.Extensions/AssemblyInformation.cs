using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace DotSpatial.Extensions
{
    /// <summary>
    /// Information about the assembly containing the extension.
    /// </summary>
    public abstract class AssemblyInformation
    {
        #region Fields

        private Type _classType;

        private FileVersionInfo _file;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the assembly and class.
        /// </summary>
        public virtual string AssemblyQualifiedName
        {
            get
            {
                string fullName = string.Empty;
                try
                {
                    fullName = ReferenceType.AssemblyQualifiedName;
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }

                return fullName;
            }
        }

        /// <summary>
        /// Gets or sets the plugins author.
        /// </summary>
        /// <remarks>This setter should be overriden by a derived class (if needed).</remarks>
        public virtual string Author
        {
            get
            {
                string author = string.Empty;
                try
                {
                    author = ReferenceFile.CompanyName;
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }

                return author;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the build date.
        /// </summary>
        public string BuildDate
        {
            get
            {
                string buildDate = string.Empty;
                try
                {
                    if (ReferenceAssembly.Location == null)
                        throw new ArgumentNullException(nameof(ReferenceAssembly.Location), Messages.ReferenceAssemblyLocationMayNotBeNull);
                    buildDate = File.GetLastWriteTime(ReferenceAssembly.Location).ToLongDateString();
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }

                return buildDate;
            }
        }

        /// <summary>
        /// Gets or sets the short description of the plugin.
        /// </summary>
        /// <remarks>This setter should be overriden by a derived class (if needed).</remarks>
        public virtual string Description
        {
            get
            {
                string desc = string.Empty;
                try
                {
                    desc = ReferenceFile.Comments;
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }

                return desc;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets or sets the name of the plugin.
        /// </summary>
        /// <remarks>This setter should be overriden by a derived class (if needed).</remarks>
        public virtual string Name
        {
            get
            {
                string name = string.Empty;
                try
                {
                    name = ReferenceFile.ProductName;
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }

                return name;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets or sets the plugin version.
        /// </summary>
        /// <remarks>This setter should be overriden by a derived class (if needed).</remarks>
        public virtual string Version
        {
            get
            {
                string version = string.Empty;
                try
                {
                    version = ReferenceFile.FileVersion;
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }

                return version;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the reference assembly.
        /// </summary>
        protected Assembly ReferenceAssembly
        {
            get
            {
                if (_classType == null) _classType = GetType();

                return _classType.Assembly;
            }
        }

        /// <summary>
        /// Gets the reference file.
        /// </summary>
        protected FileVersionInfo ReferenceFile
        {
            get
            {
                if (ReferenceAssembly.Location == null)
                    throw new ArgumentNullException(nameof(ReferenceAssembly.Location), Messages.ReferenceAssemblyLocationMayNotBeNull);
                return _file ?? (_file = FileVersionInfo.GetVersionInfo(ReferenceAssembly.Location));
            }
        }

        /// <summary>
        /// Gets the type of the referenced class.
        /// </summary>
        protected Type ReferenceType => _classType ?? (_classType = GetType());

        #endregion
    }
}
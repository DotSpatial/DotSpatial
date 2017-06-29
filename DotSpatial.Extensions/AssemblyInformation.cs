using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace DotSpatial.Extensions
{
    /// <summary>
    /// Information about the assembly containing the extension
    /// </summary>
    public abstract class AssemblyInformation
    {
        private Type _classType;
        private FileVersionInfo _file;

        /// <summary>
        /// The type of the referenced class
        /// </summary>
        protected Type ReferenceType
        {
            get
            {
                if (_classType == null)
                    _classType = this.GetType();

                return _classType;
            }
        }

        /// <summary>
        /// Gets the reference assembly.
        /// </summary>
        protected Assembly ReferenceAssembly
        {
            get
            {
                if (_classType == null)
                    _classType = this.GetType();

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
                return this._file ?? (this._file = FileVersionInfo.GetVersionInfo(this.ReferenceAssembly.Location));
            }
        }

        /// <summary>
        /// Author of the plugin.
        /// </summary>
        /// <remarks>This setter should be overriden by a derived class (if needed).</remarks>
        public virtual string Author
        {
            get
            {
                string author = String.Empty;
                try
                {
                    author = this.ReferenceFile.CompanyName;
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
        /// Build date.
        /// </summary>
        public string BuildDate
        {
            get
            {
                string buildDate = String.Empty;
                try
                {
                    buildDate = File.GetLastWriteTime(this.ReferenceAssembly.Location).ToLongDateString();
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }
                return buildDate;
            }
        }

        /// <summary>
        /// Short description of the plugin.
        /// </summary>
        /// <remarks>This setter should be overriden by a derived class (if needed).</remarks>
        public virtual string Description
        {
            get
            {
                string desc = String.Empty;
                try
                {
                    desc = this.ReferenceFile.Comments;
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
        /// Name of the plugin.
        /// </summary>
        /// <remarks>This setter should be overriden by a derived class (if needed).</remarks>
        public virtual string Name
        {
            get
            {
                string name = String.Empty;
                try
                {
                    name = this.ReferenceFile.ProductName;
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
        /// Plugin version.
        /// </summary>
        /// <remarks>This setter should be overriden by a derived class (if needed).</remarks>
        public virtual string Version
        {
            get
            {
                string version = String.Empty;
                try
                {
                    version = this.ReferenceFile.FileVersion;
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
        /// Gets the name of the assembly and class.
        /// </summary>
        public virtual string AssemblyQualifiedName
        {
            get
            {
                string fullName = String.Empty;
                try
                {
                    fullName = this.ReferenceType.AssemblyQualifiedName;
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }

                return fullName;
            }
        }
    }
}
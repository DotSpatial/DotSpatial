using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Data;

namespace DotSpatial.Data.LiDAR
{
    class LiDARData:ILiDARData,IDataSet
    {
        public void ReadFile(string filename)
        {
        }

        public Extent Extent
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsDisposed
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IProgressHandler ProgressHandler
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public SpaceTimeSupport SpaceTimeSupport
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string TypeName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void LockDispose()
        {
            throw new NotImplementedException();
        }

        public bool IsDisposeLocked
        {
            get { throw new NotImplementedException(); }
        }

        public void UnlockDispose()
        {
            throw new NotImplementedException();
        }

        public bool CanReproject
        {
            get { throw new NotImplementedException(); }
        }

        public DotSpatial.Projections.ProjectionInfo Projection
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ProjectionString
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Reproject(DotSpatial.Projections.ProjectionInfo targetProjection)
        {
            throw new NotImplementedException();
        }
    }
}

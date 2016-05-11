using DotSpatial.Data;

namespace DotSpatial.Plugins.SpatiaLite
{
    public class SpatiaLiteFeatureSet : FeatureSet
    {
        public SpatiaLiteFeatureSet(FeatureType fType)
            :base(fType)
        {
        }

        public override IFeature GetFeature(int index)
        {
            var res =  base.GetFeature(index);
            if (res.DataRow == null)
            {
                res.DataRow = DataTable.Rows[index];
            }
            return res;
        }
    }
}
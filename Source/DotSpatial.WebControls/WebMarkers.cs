using System;
using System.Linq;
using DotSpatial.Data;
using GeoAPI.Geometries;

namespace DotSpatial.WebControls
{
    /// <summary>
    /// WebMarkers
    /// </summary>
    public class WebMarkers
    {
        private Coordinate[] _crds = new Coordinate[0];
        private string[] _imgs = new string[0];
        private string[] _onClick = new string[0];
        private string[] _aRef = new string[0];
        private string[] _onMouseOver = new string[0];
        private string[] _onMouseOut = new string[0];

        System.Drawing.Point[] _dxy = new System.Drawing.Point[0];

        private int GetNum()
        {
            int n = _crds.Count();

            return n;
        }

        public void AddMark(Coordinate c, string img, string onClick, string aRef, string onMouseOver, string onMouseOut, int dx, int dy)
        {
            int n = GetNum() + 1;

            SetMarkNumber(n);

            SetMarker(n - 1, c, img, onClick, aRef, onMouseOver, onMouseOut, dx, dy);
        }

        public void SetMarkNumber(int n)
        {
            Array.Resize(ref _crds, n);
            Array.Resize(ref _imgs, n);
            Array.Resize(ref _onClick, n);
            Array.Resize(ref _aRef, n);
            Array.Resize(ref _onMouseOver, n);
            Array.Resize(ref _onMouseOut, n);
            Array.Resize(ref _dxy, n);
        }

        public void SetMarker(int i, Coordinate c, string img, string onClick, string aRef, string onMouseOver, string onMouseOut, int dx, int dy)
        {
            _crds[i] = c;
            _imgs[i] = img;
            _onClick[i] = onClick;
            _aRef[i] = aRef;
            _onMouseOver[i] = onMouseOver;
            _onMouseOut[i] = onMouseOut;
            _dxy[i].X = dx;
            _dxy[i].Y = dy;
        }

        public void CreateFromFeatureSet(FeatureSet fs, string imgField, string onClickField, string aRefField, string onMouseOverField, string onMouseOutField, string dxField, string dyField)
        {

            SetMarkNumber(fs.Features.Count);

            int i = 0;

            foreach (Feature f in fs.Features)
            {
                if (f.Geometry.Coordinates.Length > 0)
                {
                    _crds[i] = f.Geometry.Coordinates[0];
                    _imgs[i] = Convert.ToString(f.DataRow[imgField]);

                    if (onClickField != "" & onClickField != null) _onClick[i] = Convert.ToString(f.DataRow[onClickField]);
                    if (aRefField != "" & aRefField != null) _aRef[i] = Convert.ToString(f.DataRow[aRefField]);
                    if (onMouseOverField != "" & onMouseOverField != null) _onMouseOver[i] = Convert.ToString(f.DataRow[onMouseOverField]);
                    if (onMouseOutField != "" & onMouseOutField != null) _onMouseOut[i] = Convert.ToString(f.DataRow[onMouseOutField]);

                    _dxy[i].X = Convert.ToInt32(f.DataRow[dxField]);
                    _dxy[i].Y = Convert.ToInt32(f.DataRow[dyField]);
                }

                i++;
            }

        }

        public string ToHtml(ref GDIMap m)
        {
            string htm = "";
            int n = GetNum();


            for (int i = 0; i < n; i++)
            {
                System.Drawing.Point pt = m.ProjToPixel(_crds[i]);

                pt.X += _dxy[i].X;
                pt.Y += _dxy[i].Y;

                if (_aRef[i] != "" & _aRef[i] != null)
                {
                    htm += "<a href=\"" + _aRef[i] + "\">";
                }

                htm += "<img alt=\"\" src=\"" + _imgs[i] + "\" style=\"border:0px; cursor:pointer; position:absolute; left:" + pt.X.ToString() + "px; top:" + pt.Y.ToString() + "px; z-index:400\"";

                if (_onClick[i] != "" & _onClick[i] != null) htm += " onclick=\"" + _onClick[i] + "\"";
                if (_onMouseOver[i] != "" & _onMouseOver[i] != null) htm += " onmouseover=\"" + _onMouseOver[i] + "\"";
                if (_onMouseOut[i] != "" & _onMouseOut[i] != null) htm += " onmouseout=\"" + _onMouseOut[i] + "\"";

                htm += " />";

                if (_aRef[i] != "" & _aRef[i] != null)
                {
                    htm += "</a>";
                }


            }

            return htm;
        }

    }
}

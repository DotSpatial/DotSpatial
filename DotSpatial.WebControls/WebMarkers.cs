using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Topology;
using DotSpatial.Data;
using DotSpatial.Controls;

namespace DotSpatial.WebControls
{
    /// <summary>
    /// WebMarkers
    /// </summary>
    public class WebMarkers
    {
        private Coordinate[] _crds = new Coordinate[0];
        private string[] _imgs = new string[0];
        private string[] _OnClick = new string[0];
        private string[] _aRef = new string[0];
        private string[] _OnMouseOver = new string[0];
        private string[] _OnMouseOut = new string[0];

        System.Drawing.Point[] _dxy = new System.Drawing.Point[0];

        private int GetNum()
        {
            int n = _crds.Count();

            return n;
        }

        public void AddMark(Coordinate c, string img, string OnClick, string aRef, string OnMouseOver, string OnMouseOut, int dx, int dy)
        {
            int n = GetNum() + 1;

            SetMarkNumber(n);

            SetMarker(n - 1, c, img, OnClick,aRef,OnMouseOver,OnMouseOut, dx, dy);
        }

        public void SetMarkNumber(int n)
        {
            Array.Resize(ref _crds, n);
            Array.Resize(ref _imgs, n);
            Array.Resize(ref _OnClick, n);
            Array.Resize(ref _aRef, n);
            Array.Resize(ref _OnMouseOver, n);
            Array.Resize(ref _OnMouseOut, n);            
            Array.Resize(ref _dxy, n);
        }

        public void SetMarker(int i, Coordinate c, string img, string OnClick, string aRef, string OnMouseOver, string OnMouseOut, int dx, int dy)
        {
            _crds[i] = c;
            _imgs[i] = img;
            _OnClick[i] = OnClick;
            _aRef[i] = aRef;
            _OnMouseOver[i] = OnMouseOver;
            _OnMouseOut[i] = OnMouseOut;
            _dxy[i].X = dx;
            _dxy[i].Y = dy;
        }

        public void CreateFromFeatureSet(FeatureSet fs, string imgField, string OnCLickFiled,string aRefField, string OnMouseOverField, string OnMouseOutField, string dxField, string dyField)
        {

            SetMarkNumber(fs.Features.Count);

            int i = 0;

            foreach (Feature f in fs.Features)
            {
                if (f.Coordinates.Count>0)
                {
                    _crds[i] = f.Coordinates[0];
                    _imgs[i] = Convert.ToString(f.DataRow[imgField]);
                    
                    if (OnCLickFiled != "" & OnCLickFiled != null) _OnClick[i] = Convert.ToString(f.DataRow[OnCLickFiled]);
                    if (aRefField != "" & aRefField != null) _aRef[i] = Convert.ToString(f.DataRow[aRefField]);
                    if (OnMouseOverField != "" & OnMouseOverField != null) _OnMouseOver[i] = Convert.ToString(f.DataRow[OnMouseOverField]);
                    if (OnMouseOutField != "" & OnMouseOutField != null) _OnMouseOut[i] = Convert.ToString(f.DataRow[OnMouseOutField]);

                    _dxy[i].X = Convert.ToInt32(f.DataRow[dxField]);
                    _dxy[i].Y = Convert.ToInt32(f.DataRow[dyField]);
                }

                i++;
            }

        }

        public string ToHtml(ref GDIMap m)
        {
            string htm = "";
            int n=GetNum();


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

                if (_OnClick[i] != "" & _OnClick[i] != null) htm += " onclick=\"" + _OnClick[i] + "\"";
                if (_OnMouseOver[i] != "" & _OnMouseOver[i] != null) htm += " onmouseover=\"" + _OnMouseOver[i] + "\"";
                if (_OnMouseOut[i] != "" & _OnMouseOut[i] != null) htm += " onmouseout=\"" + _OnMouseOut[i] + "\"";

                htm+=" />";
                
                if (_aRef[i] != "" & _aRef[i] != null)
                {
                    htm += "</a>";
                }


            }

            return htm;
        }

    }
}

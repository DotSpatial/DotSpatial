// -----------------------------------------------------------------------
// <copyright file="Lmi.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DotSpatial.Controls.Core
{
    using GeoAPI.Geometries;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;

    //******************************************************************************************************************
    public class CGX_MaskBounds
    {
        //========================================================
        #region Properties / Attributes

        private RectangleF _bounds;
        private Coordinate _center;
        private double _rotation;
        private double _width;
        private double _height;

        #endregion

        //========================================================
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="rotation"></param>
        public CGX_MaskBounds(RectangleF bounds, double rotation)
        {
            _bounds = bounds;
            _center = null;
            _width = -1;
            _height = -1;
            _rotation = rotation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="rotation"></param>
        public CGX_MaskBounds(Coordinate center, double rotation, double dWidth, double dHeight)
        {
            _bounds = RectangleF.Empty;
            _center = center;
            _width = dWidth;
            _height = dHeight;
            _rotation = rotation;
        }

        #endregion

        //========================================================
        #region Accessor

        public RectangleF Bounds
        {
            get { return _bounds; }
        }

        public Coordinate Center
        {
            get { return _center; }
        }

        public double Width
        {
            get { return _width; }
        }

        public double Height
        {
            get { return _height; }
        }

        public double Rotation
        {
            get { return _rotation; }
        }

        #endregion
    }

    //******************************************************************************************************************
    public class CGX_Mask
    {
        #region Properties / Attributes

        private string _layerName;
        private List<CGX_MaskBounds> _masks;
        private List<Tuple<string, string>> _parents;
        private double _offsetTop;
        private double _offsetBottom;
        private double _offsetLeft;
        private double _offsetRight;

        #endregion

        //========================================================
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public CGX_Mask()
        {
            _layerName = "";
            _masks = new List<CGX_MaskBounds>();
            _parents = new List<Tuple<string, string>>();
        }

        #endregion

        //========================================================
        #region Accessor

        public string LayerName
        {
            get { return _layerName; }
            set { _layerName = value; }
        }

        public List<CGX_MaskBounds> Masks
        {
            get { return _masks; }
        }

        public List<Tuple<string, string>> Parents
        {
            get { return _parents; }
        }

        public double OffsetTop
        {
            get { return _offsetTop; }
            set { _offsetTop = value; }
        }

        public double OffsetBottom
        {
            get { return _offsetBottom; }
            set { _offsetBottom = value; }
        }

        public double OffsetLeft
        {
            get { return _offsetLeft; }
            set { _offsetLeft = value; }
        }

        public double OffsetRight
        {
            get { return _offsetRight; }
            set { _offsetRight = value; }
        }

        #endregion

        //========================================================
        #region Methods

        public void ResetBounds()
        {
            _masks = new List<CGX_MaskBounds>();
        }

        #endregion
    }

    //******************************************************************************************************************
    public class CGX_Mask_List
    {
        //========================================================
        #region Properties / Attributes

        private static List<CGX_Mask> Masks;

        #endregion

        //========================================================
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newMask"></param>
        public static void AddMask(CGX_Mask newMask)
        {
            try
            {
                if (Masks == null)
                {
                    Masks = new List<CGX_Mask>();
                }

                Masks.Add(newMask);
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
        }

        #endregion

        //========================================================
        #region Methods

        public static void AddMask(string[] layersName, Tuple<string, string> parentName, RectangleF bounds, double angle, Tuple<double, double, double, double> margins)
        {
            try
            {
                if (Masks == null)
                {
                    Masks = new List<CGX_Mask>();
                }

                foreach (string slayer in layersName)
                {
                    CGX_Mask mask = GetOrCreateMask(slayer);
                    mask.OffsetTop = margins.Item1;
                    mask.OffsetRight = margins.Item2;
                    mask.OffsetLeft = margins.Item3;
                    mask.OffsetBottom = margins.Item4;
                    Tuple<string, string> sGet = mask.Parents.FindLast(
                            delegate (Tuple<string, string> maskParent)
                            { return ((maskParent.Item1 == parentName.Item1) && (maskParent.Item2 == parentName.Item2)); });
                    if (sGet == null)
                    {
                        mask.Parents.Add(parentName);
                    }

                    mask.Masks.Add(new CGX_MaskBounds(bounds, angle));
                }
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
        }

        public static void AddMask(string[] layersName, Tuple<string, string> parentName, Coordinate center, double dWidth, double dHeight, double angle, Tuple<double, double, double, double> margins)
        {
            try
            {
                if (Masks == null)
                {
                    Masks = new List<CGX_Mask>();
                }

                foreach (string slayer in layersName)
                {
                    CGX_Mask mask = GetOrCreateMask(slayer);
                    mask.OffsetTop = margins.Item1;
                    mask.OffsetRight = margins.Item2;
                    mask.OffsetLeft = margins.Item3;
                    mask.OffsetBottom = margins.Item4;
                    Tuple<string, string> sGet = mask.Parents.FindLast(
                            delegate (Tuple<string, string> maskParent)
                            { return ((maskParent.Item1 == parentName.Item1) && (maskParent.Item2 == parentName.Item2)); });
                    if (sGet == null)
                    {
                        mask.Parents.Add(parentName);
                    }

                    mask.Masks.Add(new CGX_MaskBounds(center, angle, dWidth, dHeight));
                }
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
        }

        public static CGX_Mask GetMasks(string slayername)
        {
            try
            {
                if (Masks == null)
                {
                    return null;
                }

                return Masks.FindLast(
                        delegate (CGX_Mask bk)
                        { return bk.LayerName == slayername; });
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

            return null;
        }

        public static List<CGX_Mask> GetMasks()
        {
            if (Masks == null)
            {
                Masks = new List<CGX_Mask>();
            }

            return Masks;
        }

        public static void Reset()
        {

            if (Masks == null)
            {
                Masks = new List<CGX_Mask>();
            }

            Masks.Clear();
        }

        public static void ResetMasks(string[] sLayersName, Tuple<string, string> sParent)
        {
            try
            {
                foreach (string sLayerName in sLayersName)
                {
                    ResetMask(sLayerName, sParent);
                }
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
        }

        public static void ResetMask(string sLayerName, Tuple<string, string> sParent)
        {
            try
            {
                if (Masks != null)
                {
                    CGX_Mask foundMask = Masks.FindLast(
                        delegate (CGX_Mask bk)
                        {
                            return bk.LayerName == sLayerName;
                        });

                    if (foundMask != null)
                    {
                        Tuple<string, string> sGet = foundMask.Parents.FindLast(
                                delegate (Tuple<string, string> maskParent)
                                { return ((maskParent.Item1 == sParent.Item1) && (maskParent.Item2 == sParent.Item2)); });
                        if (sGet != null)
                        {
                            foundMask.Masks.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
        }

        public static void RemoveMasks(Tuple<string, string> sParent)
        {
            try
            {
                if (Masks != null)
                {
                    List<CGX_Mask> masksToRemove = new List<CGX_Mask>();
                    foreach (CGX_Mask mask in Masks)
                    {
                        mask.Parents.Remove(sParent);
                        if (mask.Parents.Count == 0)
                        {
                            masksToRemove.Add(mask);
                        }
                    }

                    foreach (CGX_Mask maskToRemove in masksToRemove)
                    {
                        Masks.Remove(maskToRemove);
                    }
                }
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
        }

        private static CGX_Mask GetOrCreateMask(string sLayerName)
        {
            CGX_Mask newMask = null;

            try
            {
                if (Masks != null)
                {
                    newMask = Masks.FindLast(
                        delegate (CGX_Mask bk)
                        {
                            return bk.LayerName == sLayerName;
                        });
                }


                // Create a new Mask if not found
                if (newMask == null)
                {
                    newMask = new CGX_Mask();
                    newMask.LayerName = sLayerName;
                    Masks.Add(newMask);
                }
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

            return newMask;
        }

        #endregion
    }
}
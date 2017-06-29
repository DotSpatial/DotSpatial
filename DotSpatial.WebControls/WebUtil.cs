using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Globalization;

namespace DotSpatial.WebControls
{
    public static class WebUtil
    {
        public static string UnitFormat(Unit u)
        {
            
            string v = u.Value.ToString(CultureInfo.InvariantCulture);

            string t = "px";
            switch (u.Type)
            {
                case UnitType.Cm:
                    {
                        t = "cm";
                        break;
                    }

                case UnitType.Em:
                    {
                        t = "em";
                        break;
                    }

                case UnitType.Ex:
                    {
                        t = "ex";
                        break;
                    }

                case UnitType.Inch:
                    {
                        t = "in";
                        break;
                    }

                case UnitType.Mm:
                    {
                        t = "mm";
                        break;
                    }

                case UnitType.Percentage:
                    {
                        t = "%";
                        break;
                    }

                case UnitType.Pica:
                    {
                        t = "pi";
                        break;
                    }

                case UnitType.Pixel:
                    {
                        t = "px";
                        break;
                    }

                case UnitType.Point:
                    {
                        t = "pt";
                        break;
                    }

            }

            return v + t;

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace DotSpatial.WebControls
{
    public enum WebTool : int
    {
        PanAndClick = -5,
        None = -1,
        //ZoomOut = 0,
        //ZoomAll = 1,
        //ZoomIn = 2,
        ZoomRect = 3,
        Pan = 4,
        Info = 5,
        Select = 6,
        DataGrid = 7,
        Edit = 8,

        //MoveWest = 9,
        //MoveNorth = 10,
        //MoveEast = 11,
        //MoveSouth = 12,
    }

    public enum WebButtonType : int
    {
        Space = 0,
        Custom = 1,
        ZoomOUT = 2,
        ZoomALL = 3,
        ZoomIN = 4,
        ZoomRECT = 5,
        Pan = 6,
        Info = 7,
        Select = 8,
        DataGrid = 9,
        Edit = 10,

        ToolsMoveLeft = 11,
        ToolsMoveUp = 12,
        ToolsMoveRigth = 13,
        ToolsMoveDown = 14,

        ToolsMoveZoomIn = 15,
        ToolsMoveZoomAll = 16,
        ToolsMoveZoomOut = 17,

    }

    public class WebButton
    {

        public WebButtonType Type;
        public string ImageURL = "";
        public string Command = "";

        public string style;
    }

    [DefaultProperty("Text")]
    [ToolboxData("<{0}:WebToolBar runat=server></{0}:WebToolBar>")]
    public class WebToolBar : WebControl
    {
        private string Name
        {
            get
            {
                string name = "";

                Object o = ViewState["name"];

                if (o == null)
                {
                    name = Guid.NewGuid().ToString();
                    ViewState["name"] = name;
                }
                else
                {
                    name = (string)ViewState["name"];
                }

                return name;
            }

        }

        #region StandardButtonsValues
        private string[] _images =
        {
            "DotSpatial.WebControls.Images.Tr.gif",
            "DotSpatial.WebControls.Images.ToolsCustom.gif",
            "DotSpatial.WebControls.Images.Tools0ZoomOUT.gif",
            "DotSpatial.WebControls.Images.Tools1ZoomALL.gif",
            "DotSpatial.WebControls.Images.Tools2ZoomIN.gif",
            "DotSpatial.WebControls.Images.Tools3ZoomRect.gif",
            "DotSpatial.WebControls.Images.Tools4Pan.gif",
            "DotSpatial.WebControls.Images.Tools5Info.gif",
            "DotSpatial.WebControls.Images.Tools6Select.gif",
            "DotSpatial.WebControls.Images.Tools7DataGrid.gif",
            "DotSpatial.WebControls.Images.Tools8Edit.gif",

            "DotSpatial.WebControls.Images.ToolsMoveLeft.gif",
            "DotSpatial.WebControls.Images.ToolsMoveUp.gif",
            "DotSpatial.WebControls.Images.ToolsMoveRight.gif",
            "DotSpatial.WebControls.Images.ToolsMoveDown.gif",

            "DotSpatial.WebControls.Images.ToolsMoveZoomIN.gif",
            "DotSpatial.WebControls.Images.ToolsMoveZoomALL.gif",
            "DotSpatial.WebControls.Images.ToolsMoveZoomOUT.gif"
        };


        private string[] _commands =
        {
            "",
            "alert('custom')",
            "WebToolBarClickTool('<WebMapID>',ToolZoomOut,'<index>')",
            "WebToolBarClickTool('<WebMapID>',ToolZoomAll,'<index>')",
            "WebToolBarClickTool('<WebMapID>',ToolZoomIn,'<index>')",
            "WebToolBarClickTool('<WebMapID>',ToolZoomRect,'<index>')",
            "WebToolBarClickTool('<WebMapID>',ToolPan,'<index>')",
            "WebToolBarClickTool('<WebMapID>',ToolInfo,'<index>')",
            "WebToolBarClickTool('<WebMapID>',ToolSelect,'<index>')",
            "WebToolBarClickTool('<WebMapID>',ToolDataGrid,'<index>')",
            "WebToolBarClickTool('<WebMapID>',ToolEdit,'<index>')",

            "WebToolBarClickTool('<WebMapID>',ToolMoveWest,'<index>')",
            "WebToolBarClickTool('<WebMapID>',ToolMoveNorth,'<index>')",
            "WebToolBarClickTool('<WebMapID>',ToolMoveEast,'<index>')",
            "WebToolBarClickTool('<WebMapID>',ToolMoveSouth,'<index>')",

            "WebToolBarClickTool('<WebMapID>',ToolZoomIn,'<index>')",
            "WebToolBarClickTool('<WebMapID>',ToolZoomAll,'<index>')",
            "WebToolBarClickTool('<WebMapID>',ToolZoomOut,'<index>')",

        };

        private string[] _stiles =
        {

            "cursor:pointer; position:static; width:3px",
            "cursor:pointer; position:static; z-index:1000;",
            "cursor:pointer; position:static; z-index:1000;",
            "cursor:pointer; position:static; z-index:1000;",
            "cursor:pointer; position:static; z-index:1000;",
            "cursor:pointer; position:static; z-index:1000; background-color:white;",
            "cursor:pointer; position:static; z-index:1000; background-color:white;",
            "cursor:pointer; position:static; z-index:1000; background-color:white;",
            "cursor:pointer; position:static; z-index:1000; background-color:white;",
            "cursor:pointer; position:static; z-index:1000; background-color:white;",
            "cursor:pointer; position:static; z-index:1000; background-color:white;",

            "cursor:pointer; position:absolute; z-index:1000; left:3px;  top:21px;",
            "cursor:pointer; position:absolute; z-index:1000; left:21px; top:3px;",
            "cursor:pointer; position:absolute; z-index:1000; left:39px; top:21px",
            "cursor:pointer; position:absolute; z-index:1000; left:21px; top:39px",

            "cursor:pointer; position:absolute; z-index:1000; left:21px; top:60px;",
            "cursor:pointer; position:absolute; z-index:1000; left:21px; top:78px;",
            "cursor:pointer; position:absolute; z-index:1000; left:21px; top:96px;"

        };
        #endregion

        private List<WebButton> Buttons
        {
            get
            {
                Object o = System.Web.HttpContext.Current.Session["Buttons_" + Name];

                if (o == null)
                {
                    return new List<WebButton>();
                }

                return (List<WebButton>)o;
            }
            set
            {
                System.Web.HttpContext.Current.Session["Buttons_" + Name] = value;
            }
        }

        public string WebMapID
        {
            get
            {
                String s = (String)ViewState["WebMapControlID"];
                return s;
            }

            set
            {
                ViewState["WebMapControlID"] = value;
            }
        }

        public void CreateStandardButtons()
        {
            AddButton(WebButtonType.ZoomOUT);
            AddButton(WebButtonType.Space);

            AddButton(WebButtonType.ZoomALL);
            AddButton(WebButtonType.Space);

            AddButton(WebButtonType.ZoomIN);
            AddButton(WebButtonType.Space);

            AddButton(WebButtonType.ZoomRECT);
            AddButton(WebButtonType.Space);

            AddButton(WebButtonType.Pan);
            AddButton(WebButtonType.Space);

            AddButton(WebButtonType.Info);
            AddButton(WebButtonType.Space);

            AddButton(WebButtonType.Select);
            AddButton(WebButtonType.Space);

            AddButton(WebButtonType.DataGrid);
            AddButton(WebButtonType.Space);

            AddButton(WebButtonType.Edit);
                    
        }

        public void CreateSmartNavigation()
        {
            AddButton(WebButtonType.ToolsMoveLeft);
            AddButton(WebButtonType.ToolsMoveUp);
            AddButton(WebButtonType.ToolsMoveRigth);
            AddButton(WebButtonType.ToolsMoveDown);

            AddButton(WebButtonType.ToolsMoveZoomIn);
            AddButton(WebButtonType.ToolsMoveZoomAll);
            AddButton(WebButtonType.ToolsMoveZoomOut);
        }

        public void AddButton(WebButtonType type, string Command = "", string imageURL = "",  string style="" )
        {
            WebButton b = new WebButton();

            b.Type = type;

            
            if (imageURL != "")
            {
                b.ImageURL = imageURL;
            }
            else
            {
                b.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), _images[(int)type]);
            }

            if (Command != "")
            {
                b.Command = Command;
            }
            else
            {
                b.Command = _commands[(int)type];
            }

            if (style != "")
            {
                b.style = style;
            }
            else
            {
                b.style =  _stiles[(int)type];
            }


            List<WebButton> btns = Buttons;
            btns.Add(b);
            Buttons = btns;
              
        }

        protected override void RenderContents(HtmlTextWriter output)
        {

            if (this.DesignMode)
            {
                    output.Write("<table style='border-color:Black;background-color:" + BackColor.ToString() + "'><tr><td style='vertical-align:middle;text-align:center'> ToolBar: " + ClientID + "</td></tr></table>");
            }
            else
            {
                List<WebButton> _buttons = Buttons;

                string htm = "";
                string MapID = WebMapID;
                string ControlID;

                for (int i = 0; i < _buttons.Count; i++)
                {
                    WebButton b = _buttons[i];


                    if (b.Type != WebButtonType.Space)
                    {
                        ControlID = "Tool_" + MapID + i.ToString(); 
                    }
                    else
                    {
                        ControlID = "Space_" + MapID + i.ToString();
                    }

                    string command = _buttons[i].Command.Replace("<WebMapID>", MapID);

                    command = command.Replace("<index>", ControlID);

                    htm += "<img id=\"" + ControlID + "\" onclick=\"" + command + "\"  ondrag(event)=\"return false\" onSelectStart=\"return false\" src=\"" + b.ImageURL + "\" style=\"" + b.style + "\" />";

                }

                output.Write(htm);

            }
        }

    }
}

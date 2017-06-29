var ToolCustom = -2;
var defaultTool = -1;
var ToolPanAndClick = -5;
var ToolNone = -1;
var ToolZoomOut = 0;
var ToolZoomAll = 1;
var ToolZoomIn = 2;
var ToolZoomRect = 3;
var ToolPan = 4;
var ToolInfo = 5;
var ToolSelect = 6;
var ToolDataGrid = 7;
var ToolEdit = 8;

var ToolMoveWest = 9;
var ToolMoveNorth = 10;
var ToolMoveEast = 11;
var ToolMoveSouth = 12;


function WebToolBarClickTool(MapID, ToolType, ToolID) {

    switch (ToolType) {
        case ToolNone:
            {
            }
            break;
        case ToolZoomOut:
            {
                Wait(MapID);
                CallServer(MapID,"WheelOUT|0|0");
                return;
            }
            break;
        case ToolZoomAll:
            {
                Wait(MapID);
                CallServer(MapID,"ZoomAll");
                return;
            }
            break;
        case ToolZoomIn:
            {
                Wait(MapID);
                CallServer(MapID,"WheelIN|0|0");
                return;
            }
            break;
        case ToolDataGrid:
            {
                CallServer(MapID, "DataGrid");
                return;
            }
            break;

        case ToolMoveWest:
        case ToolMoveNorth:
        case ToolMoveEast:
        case ToolMoveSouth:
            {
                MoveNSEW(ToolType, MapID);
                return;
            }
            break;
        default:
            {
                //ColorOnTool(index);
                SetActualTool(MapID, ToolType, ToolID)
                ColorOnTool(MapID);

                return;
            }
    }
}

function MoveNSEW(ToolType, MapID) {

    var canvas = document.getElementById("Canvas_" + MapID);
    var buffer = document.getElementById("Buffer_" + MapID);
    var back = document.getElementById("Back_" + MapID);
    var mark = document.getElementById("Markers_" + MapID);

    var w = parseInt(canvas.offsetWidth / 3);
    var h = parseInt(canvas.offsetHeight / 3);

    buffer.src = canvas.src;
    buffer.style.width = canvas.style.width;
    buffer.style.height = canvas.style.height;

    canvas.src = " ";
    canvas.style.display = "none";

    var dx = 0;
    var dy = 0;

    switch (ToolType) {
        case ToolMoveWest:
            {
                dx = -w;
            }
            break;
        case ToolMoveNorth:
            {
                dy = -h;
            }
            break;
        case ToolMoveEast:
            {
                dx = w;
            }
            break;
        case ToolMoveSouth:
            {
                dy = h;
            }
            break;
    }

    buffer.style.left = -dx.toString() + 'px';
    buffer.style.top = -dy.toString() + 'px';

    if (back != null) {
        back.style.left = -dx.toString() + 'px';
        back.style.top = -dy.toString() + 'px';
    }

    if (mark != null) {
        mark.style.left = -dx.toString() + 'px';
        mark.style.top = -dy.toString() + 'px';
    }
    
    var ret = "Pan|" + dx.toString() + "|" + dy.toString();

    Wait(MapID);
    CallServer(MapID, ret);

}


function GetActualToolType(MapID) {

    var t = readCookie("ActualToolType_" + MapID)

    if (t == null) {
        return ToolNone;
    }

    var tool = parseInt(t);

    return tool;

}

function GetActualToolID(MapID) {

    var t = readCookie("ActualToolID_" + MapID); 
    return t.toString();

}

function WriteTool(MapID, ToolType) {
    
    writeCookie("ActualToolType_" + MapID, ToolType, 6000);

}


function SetActualTool(MapID, ToolType, ToolID) {

    writeCookie("ActualToolType_" + MapID, ToolType, 6000);
    writeCookie("ActualToolID_" + MapID, ToolID, 6000);
}

function ColorOnTool(MapID) 
{

    var i=0;

    var tl = document.getElementById("Tool_" + MapID + i.toString());
    var sp = document.getElementById("Space_" + MapID + i.toString());

    var ActualToolID = GetActualToolID(MapID);


    while (tl != null | sp!=null) {

        tl = document.getElementById("Tool_" + MapID + i.toString());
        sp = document.getElementById("Space_" + MapID + i.toString());

        if (tl != null) {

            if (ActualToolID == "Tool_" + MapID + i.toString()) {

                if (tl.style.backgroundColor != "lightgray") {
                    tl.style.backgroundColor = "yellow"
                }
                else {
                    SetActualTool(defaultTool);
                    ColorOnTool(defaultTool);
                }
            }
            else {
                if (tl.style.backgroundColor != "lightgray" & tl.style.backgroundColor != "") {
                    tl.style.backgroundColor = "white"
                }
            }
        }

        i++;
    }

}
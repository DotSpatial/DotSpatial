var StartPoint = null;
var froze = false;
var IsWait = false;

var NumTools = 9;

var defaultTool = -1;
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

var LayerTypeNone = -1;
var LayerTypeOther = 0;
var LayerTypePoint = 1;
var LayerTypeArc = 2;
var LayerTypePoly = 3;

var x_crds = new Array();
var y_crds = new Array();
var crds_num = 0;

var x_buf = new Array();
var y_buf = new Array();

var ActualPoint = new Object();
var FirstMoved = true;

var jg = null;

function Exists(obj) {
    if (null == obj) { return false; }
    if ('undefined' == typeof (obj)) { return false; }
    return true;
}

function GetActualTool() {
    var t = readCookie("ActualTool")
    
    if (t == null) {
        alert('AT null');
        return ToolNone;
    }

    var tool = parseInt(t);

    return tool;

}

function SetActualTool(Tool) {
    writeCookie("ActualTool", Tool.toString(), 6000);
}

function OnTool(tool,ID) {


}

function GetActualLayerType(ID) {
    var lt = document.getElementById("LayerType_" + ID);
    return parseFloat(lt.innerHTML);
}

function CheckMouseOnTool(obj, p) {

    var tlbx = document.getElementById("ToolBox_" + obj.id);


    for (var i = 0; i < NumTools; i++) {

        var tl = document.getElementById("Tool_" + obj.id + i.toString());

        if (tl != null) {

            if ( tl.style.backgroundColor != "lightgray" & p.x > tl.offsetLeft & p.x < (tlbx.offsetLeft + tl.offsetLeft + tl.offsetWidth) &
            p.y > tl.offsetTop & p.y < (tlbx.offsetTop + tl.offsetTop + tl.offsetHeight)) {

                //SetActualTool(i);
                UnTip();
                return i;
            }
        }
    }

    return ToolNone;
}

function ColorOnTool(obj, Tool) {

    if (Tool == null) {
        return;
    }

    var tlbx = document.getElementById("ToolBox_" + obj.id);

    var ActualTool = GetActualTool();

    for (var i = 3; i < NumTools; i++) {

        var tl = document.getElementById("Tool_" + obj.id + i.toString());
        if (tl != null) {
            if (i == Tool) {

                if (tl.style.backgroundColor != "lightgray") {
                    tl.style.backgroundColor = "yellow"
                }
                else {
                    SetActualTool(defaultTool);
                    ColorOnTool(obj, defaultTool);
                }
            }
            else {
                if (tl.style.backgroundColor != "lightgray") {
                    tl.style.backgroundColor = "white"
                }
            }
        }
    }

}



function Initialize(ID) {

    //Richiama il resize
    OnResize(ID);

    //lo connette all'evvento...
    window.onresize = function () { OnResize(ID) };

    var elm = new Object();
    elm = document.getElementById(ID);

    if (elm.addEventListener)
        elm.addEventListener('DOMMouseScroll', wheel, false);

    elm.onmousewheel = wheel;

}

function OnResize(ID) {

    var obj = document.getElementById(ID);

    if (obj == null) {
        return;
    }

    var ret = "Resize|" + obj.offsetWidth.toString() + "|" + obj.offsetHeight.toString();

    Wait(obj.id);
    CallServer(ret);
}

function Wait(ID) {
    
    IsWait = true;

    var elm = document.getElementById("Wait_" + ID);
    if (elm != null) {
        elm.style.visibility = "visible";
    }
}

function EndWait(ID) {
    var elm = document.getElementById("Wait_" + ID);
    if (elm != null) {
        elm.style.visibility = "hidden";
    }

    IsWait = false;
}

function OnMouseDown(obj, event) 
{
    if (IsWait == true) return;

    var ID = obj.id;
    ID = ID.toString();

    event = event || window.event;

    StartPoint = GetObjectPosition(event.clientX, event.clientY, obj);

    var tl = CheckMouseOnTool(obj, StartPoint);

    if (tl > -1) {
        StartPoint = null;
        froze = true;
        return; 
    }

    obj.ondragstart = function () { return false; };
    obj.oncontextmenu = function () { return false; };

    var ActualTool = GetActualTool();

    switch(ActualTool)
    {
        case (ToolZoomRect):
        case (ToolSelect):
            {

                var FLT = "Floater_" + obj.id;
                var elm = document.getElementById(FLT);

                elm.style.left = StartPoint.x + "px";
                elm.style.top = StartPoint.y + "px";
                elm.style.width = "0px";
                elm.style.height = "0px";
                elm.style.visibility = "visible";

                SetOpacity(elm, 25);
            }
            break;
    }

    return false;
}



function OnMouseMove(obj, event) {

    if (IsWait == true) return;

    ActualPoint = GetObjectPosition(event.clientX, event.clientY, obj);

    //DisplayXY(obj);

    if (froze == true) return;

    event = event || window.event;

    var ActualTool = GetActualTool();

    switch (ActualTool)
    {
        case (ToolEdit):
            {
                if (jg == null) {
                    jg = new jsGraphics("EditCanvas_" + obj.id);
                }

                var ActualLayerType = GetActualLayerType(obj.id);
                switch (ActualLayerType) {

                    case (LayerTypeArc):
                        {
                            CreateBufferFromList(ActualPoint);
                            if (crds_num > 0) {

                                //document.getElementById("EditCanvas_" + obj.id).innerHTML = "";
                                jg.clear();
                                jg.drawPolyline(x_buf, y_buf);
                                jg.paint();
                            }
                        }
                        break;

                    case (LayerTypePoly):
                        {
                            CreateBufferFromList(ActualPoint);

                            if (crds_num > 0) {

                                //document.getElementById("EditCanvas_" + obj.id).innerHTML = "";
                                jg.clear();
                                jg.drawPolygon(x_buf, y_buf);
                                jg.paint();
                            }

                        }
                        break;
                }

            }
            break;
        case (ToolPan):
            {

                if (Exists(StartPoint)) {

                    var canvas = document.getElementById("Canvas_" + obj.id);
                    var buffer = document.getElementById("Buffer_" + obj.id);
                    var back = document.getElementById("Back_" + obj.id);
                    var mark = document.getElementById("Markers_" + obj.id);

                    if (FirstMoved == true) {
                        buffer.src = canvas.src;
                        buffer.style.width = canvas.style.width;
                        buffer.style.height = canvas.style.height;

                        canvas.style.display = "none";
                        canvas.src = "";

                        FirstMoved = false;
                        obj.style.cursor = 'move';
                    }

                    var dx = ActualPoint.x - StartPoint.x;
                    var dy = ActualPoint.y - StartPoint.y;

                    buffer.style.left = dx + 'px';
                    buffer.style.top = dy + 'px';

                    if (back != null) {

                        var BckL = parseFloat(document.getElementById("BackLeft_" + obj.id).innerHTML);
                        var BckT = parseFloat(document.getElementById("BackTop_" + obj.id).innerHTML);

                        var Lbck = BckL + dx;
                        var Tbck = BckT + dy;

                        back.style.left = Lbck.toString() + 'px';
                        back.style.top = Tbck.toString() + 'px';
                    }

                    if (mark != null) {
                        mark.style.left = dx.toString() + 'px';
                        mark.style.top = dy.toString() + 'px';
                    }

                }
                else {

                    obj.style.cursor = 'default';
                }
            }
            break;


    case (ToolZoomRect):
    case (ToolSelect):
         {
            if (Exists(StartPoint)) {

                var objFloat = document.getElementById("Floater_" + obj.id)

                objFloat.style.left = Math.min(StartPoint.x, ActualPoint.x) + 'px';
                objFloat.style.top = Math.min(StartPoint.y, ActualPoint.y) + 'px';

                objFloat.style.width = Math.abs(ActualPoint.x - StartPoint.x)  + 'px';
                objFloat.style.height = Math.abs(ActualPoint.y - StartPoint.y)  + 'px';
            }
        }
        break;
    }

    return false;

}

//function DisplayXY(obj) {
//    var ID = obj.id;

//    var elm = document.getElementById("StatusBar_" + ID);

//    if (elm == null) return;
//    
//    var MinX = parseFloat(document.getElementById("MinX_" + ID).innerHTML);
//    var MaxX = parseFloat(document.getElementById("MaxX_" + ID).innerHTML);
//    var MinY = parseFloat(document.getElementById("MinY_" + ID).innerHTML);
//    var MaxY = parseFloat(document.getElementById("MaxY_" + ID).innerHTML);

//    var X = MinX + (ActualPoint.x / obj.offsetWidth) * (MaxX - MinX);
//    var Y = MaxY  - (ActualPoint.y / obj.offsetHeight) * (MaxY - MinY);

//    if (document.getElementById("IsLatLon_" + ID).innerHTML == "1") {
//        elm.innerHTML = X.toFixed(8) + " " + Y.toFixed(8);
//    }
//    else {
//        elm.innerHTML = X.toFixed(3) + " " + Y.toFixed(3);
//    }
//}

function CreateBufferFromList(Point) {
    if (crds_num==0)return;

    if (x_buf.length == 0) {

        x_buf = x_crds;
        y_buf = y_crds;

        x_buf.length = crds_num + 1;
        y_buf.length = crds_num + 1;
    }


    x_buf[crds_num]=Point.x;
    y_buf[crds_num]=Point.y;

}

function AddPointToList(Point) {
    crds_num += 1;

    x_crds.length = crds_num;
    y_crds.length = crds_num;

    x_crds[crds_num-1] = Point.x;
    y_crds[crds_num-1] = Point.y;

}

function OnMouseUp(obj, event) {

    if (IsWait == true) return;

    froze = false;
    FirstMoved = true;
    
    event = event || window.event;

    EndPoint = GetObjectPosition(event.clientX, event.clientY, obj);

    var ActualTool = GetActualTool();
    var tl = CheckMouseOnTool(obj, EndPoint);
    
    switch (tl) {
        case ToolZoomOut:
            {
                Wait(obj.id);
                CallServer("WheelOUT|0|0");
                return;
            }
            break;
        case ToolZoomAll:
            {
                Wait(obj.id);
                CallServer("ZoomAll");
                return;
            }
            break;
        case ToolZoomIn:
            {
                Wait(obj.id);
                CallServer("WheelIN|0|0");
                return;
            }
            break;
        case ToolDataGrid:
            {
                CallPostBack("DataGrid");
                ColorOnTool(obj, ActualTool);
                return;
            }
            break;
        case ToolNone:
            {
            }
            break;
        default:
            {
                SetActualTool(tl)
                ColorOnTool(obj, tl);
                return;
            }
    }

    


    var ret = new Object();
    var sft = new Object();

    if (Exists(StartPoint)) {
        sft.x = StartPoint.x - EndPoint.x;
        sft.y = StartPoint.y - EndPoint.y;
    }


    switch(ActualTool) {

        case ToolEdit:
            {


                if (jg == null) {
                    jg = new jsGraphics("EditCanvas_" + obj.id);
                }

                var ActualLayerType = GetActualLayerType(obj.id);

                switch (ActualLayerType) {

                    case (LayerTypePoint):
                        {
                            AddPointToList(EndPoint);
                        }
                        break;

                    case (LayerTypeArc):
                        {
                            AddPointToList(EndPoint);
                            if (crds_num > 1) {
                                jg.clear();
                                jg.drawPolyline(x_crds, y_crds);
                                jg.paint();
                            }
                        }
                        break;

                    case (LayerTypePoly):
                        {
                            AddPointToList(EndPoint);

                            if (crds_num > 1) {
                                jg.clear();
                                jg.drawPolygon(x_crds, y_crds);
                                jg.paint();
                            }

                        }
                        break;
                }

                if (((event.button != 1 || window.event == null) && event.button != 0) || ActualLayerType == LayerTypePoint) {
                    ret = "AddFeature|" + crds_num.toString();

                    for (var i = 0; i < crds_num; i++) {
                        ret = ret + "|" + x_crds[i].toString() + "|" + y_crds[i].toString();
                    }

                    crds_num = 0;

                    CallPostBack(ret);
                }

            }
            break;
        case ToolPan:
            {
                if (sft.x != 0 | sft.y != 0) {
                    ret = "Pan|" + sft.x.toString() + "|" + sft.y.toString();
                    Wait(obj.id);
                    CallServer(ret);
                }
                obj.style.cursor = 'default';
            }
            break;

        case ToolNone:
            {
                if (sft.x == 0 & sft.y == 0) {
                    if (event.button == 1 && window.event != null || event.button == 0) {
                        ret = "Click|0|" + EndPoint.x.toString() + "|" + EndPoint.y.toString();
                        CallPostBack(ret);
                    }
                    else {
                        ret = "Click|1|" + EndPoint.x.toString() + "|" + EndPoint.y.toString();
                        CallPostBack(ret);
                    }
                }

                obj.style.cursor = 'default';

            }
            break;

        case ToolInfo:
            {
                ret = "Info|1|" + EndPoint.x.toString() + "|" + EndPoint.y.toString();
                CallServer(ret);
            }
            break;

        case ToolSelect:
            {
                ret = "Select|" + StartPoint.x.toString() + "|" + StartPoint.y.toString() + "|" + EndPoint.x.toString() + "|" + EndPoint.y.toString();
                CallServer(ret);
            }
            break;

        case ToolEdit:
            {
            }
            break;

        case ToolZoomRect:
            {
                if (sft.x == 0 & sft.y == 0) {
                    if (event.button == 1 && window.event != null || event.button == 0) {
                        ret = "ZoomIN|" + EndPoint.x.toString() + "|" + EndPoint.y.toString();
                    }
                    else {
                        ret = "ZoomOUT|" + EndPoint.x.toString() + "|" + EndPoint.y.toString();
                    }
                }
                else {
                    ret = "ZoomRect|" + StartPoint.x.toString() + "|" + StartPoint.y.toString() + "|" + EndPoint.x.toString() + "|" + EndPoint.y.toString();
                }

                document.getElementById("Floater_" + obj.id).style.visibility = "hidden";

                Wait(obj.id);
                SetActualTool(ToolPan);
                CallServer(ret);
            }
            break;
    }


    //ActualTool = ToolPan;
    
    StartPoint = null;

    return false;
}


function GetObjectPosition(x, y, obj) {
    obj.left;

    var pos = WebForm_GetElementPosition(obj);

    var p = new Object();
    p.x = x - pos.x;
    p.y = y - pos.y;
    return p;
}


function ReceiveServerData(arg) 
{

    var elementId;
    var Command;

    if (arg != null & arg != "") {

        var elms = arg.split("|");
        
        elementId = elms[0];
        Command = elms[1];

        var obj = document.getElementById(elementId);


        var canvas = document.getElementById("Canvas_" + elementId);
        var buffer = document.getElementById("Buffer_" + elementId);
        var back = document.getElementById("Back_" + elementId);
        var Copyright = document.getElementById("Copyright_" + elementId);
        var mark = document.getElementById("Markers_" + elementId);

        var l = new Object();
        var t = new Object();
        var w = new Object();
        var h = new Object();
        
        switch (Command)
        {
            case "STRUCTURE":
                {
                    var innerHtmlStr = elms[2];

                    obj.innerHTML = innerHtmlStr;

                    UnTip();
                }
                break;

            case "REFRESH":
                {
                    UnTip();

                    canvas.src = elms[2];

                    canvas.style.display = "block";

                    buffer.style.left = "100000px";

                    //                    document.getElementById("MinX_" + elementId).innerHTML = elms[3];
                    //                    document.getElementById("MinY_" + elementId).innerHTML = elms[4];
                    //                    document.getElementById("MaxX_" + elementId).innerHTML = elms[5];
                    //                    document.getElementById("MaxY_" + elementId).innerHTML = elms[6];

                    if (back != null) {

                        back.innerHTML = elms[3];

                        l = parseInt(elms[4]);
                        t = parseInt(elms[5]);
                        w = parseInt(elms[6]);
                        h = parseInt(elms[7]);

                        back.style.left = l.toString() + "px";
                        back.style.top = t.toString() + "px";
                        back.style.width = w.toString() + "px";
                        back.style.height = h.toString() + "px";

                    }

                    if (Copyright != null) {

                        Copyright.innerHTML = elms[8];

                    }

                    if (mark != null) {
                        mark.style.left = "0px";
                        mark.style.top = "0px";
                        mark.innerHTML = elms[9];
                    }



                }
                break;

            case "REFRESHANDHIDEBUFFER":
                {
                    UnTip();

                    canvas.src = "";
                    canvas.style.left = 0;
                    canvas.style.top = 0;
                    canvas.src = elms[2];

                    canvas.style.display = "block";

                    buffer.style.left = "100000px";

                    //                    document.getElementById("MinX_" + elementId).innerHTML = elms[3];
                    //                    document.getElementById("MinY_" + elementId).innerHTML = elms[4];
                    //                    document.getElementById("MaxX_" + elementId).innerHTML = elms[5];
                    //                    document.getElementById("MaxY_" + elementId).innerHTML = elms[6];

                    
                    if (back != null) {

                        back.innerHTML = elms[3];

                        l = parseInt(elms[4]);
                        t = parseInt(elms[5]);
                        w = parseInt(elms[6]);
                        h = parseInt(elms[7]);

                        back.style.left = l.toString() + "px";
                        back.style.top = t.toString() + "px";
                        back.style.width = w.toString() + "px";
                        back.style.height = h.toString() + "px";

                    }

                    if (Copyright != null) {

                        Copyright.innerHTML = elms[8];

                    }

                    if (mark != null) {
                        mark.style.left = "0px";
                        mark.style.top = "0px";
                        mark.innerHTML = elms[9];
                    }

                }
                break;

            case "POPUP":
                {
                    var html = elms[2];

                    html = html + "<table><tr><td align='left'>";
                    html = html + "<input id=\"ButtonTableDiv\" type=\"button\" value=\"Esci\" onclick=\"HideBox('PopupDiv')\" />"
                    html = html + "</td></tr></table>";

                    var elm = document.getElementById("PopupDiv");
                    if (elm == null) {
                        creatediv("PopupDiv", html, 'auto', 'auto', 100000);
                    }
                    else {
                        elm.innerHTML = html;
                    }

                    showPopUpDiv("PopupDiv");
                }
                break;

            case "SCRIPT":
                {
                    eval(elms[2]);
                }
                break;
        }

    }

    var ActualTool = GetActualTool();
    ColorOnTool(obj, ActualTool);

    EndWait(elementId);

}

function SetOpacity(obj, value) {
    obj.style.opacity = value / 100.0;
    obj.style.mozopacity = value / 100.0;
    obj.style.filter = 'ALPHA(opacity=' + value + ')';
}

function handle(obj, delta) {
    var ret = new Object();
    
    if (delta < 0)
        ret = "WheelOUT|0|0";
    else
        ret = "WheelIN|0|0";

    Wait(obj.id);
    CallServer(ret);
}

/** Event handler for mouse wheel event.
*/
function wheel(event) {
    
    var delta = 0;
    if (!event) /* For IE. */
        event = window.event;
    if (event.wheelDelta) { /* IE/Opera. */
        delta = event.wheelDelta / 120;
        /** In Opera 9, delta differs in sign as compared to IE.
        */
        if (window.opera)
            delta = -delta;
    } else if (event.detail) { /** Mozilla case. */
        /** In Mozilla, sign of delta is different than in IE.
        * Also, delta is multiple of 3.
        */
        delta = -event.detail / 3;
    }
    /** If delta is nonzero, handle it.
    * Basically, delta is now positive if wheel was scrolled up,
    * and negative, if wheel was scrolled down.
    */
    if (delta)
        handle(this, delta);
    /** Prevent default actions caused by mouse wheel.
    * That might be ugly, but we handle scrolls somehow
    * anyway, so don't bother here..
    */
    if (event.preventDefault)
        event.preventDefault();
    event.returnValue = false;
}

function creatediv(id, html, width, height, left, top) {

    var newdiv = document.createElement('div');
    newdiv.setAttribute('id', id);

    if (width) {
        newdiv.style.width = width;
    }

    if (height) {
        newdiv.style.height = height;
    }

    if ((left || top) || (left && top)) {
        newdiv.style.position = "absolute";

        if (left) {
            newdiv.style.left = left;
        }

        if (top) {
            newdiv.style.top = top;
        }
    }

    //newdiv.style.overflow = "scroll";
    newdiv.style.background = "white";
    newdiv.style.border = "2px solid black";
    newdiv.style.padding = "3px";

    if (html) {
        newdiv.innerHTML = html;
    } else {
        newdiv.innerHTML = "nothing";
    }

    document.body.appendChild(newdiv);

} 


function showPopUpDiv(divID) {

    var elm = document.getElementById(divID);

    var width = document.documentElement.clientWidth + document.documentElement.scrollLeft;
    var height = document.documentElement.clientHeight;


    var layer = document.createElement('div');
    layer.style.zIndex = 200000;
    layer.id = 'layer';
    layer.style.position = 'absolute';
    layer.style.top = '0px';
    layer.style.left = '0px';
    layer.style.height = document.documentElement.scrollHeight + 'px';
    layer.style.width = width + 'px';
    layer.style.backgroundColor = 'white';
    layer.style.opacity = '.6';
    layer.style.filter += ("progid:DXImageTransform.Microsoft.Alpha(opacity=60)");
    document.body.appendChild(layer);


    var elmwidth = elm.clientWidth;
    var elmheight = elm.clientHeight;


    //elm.style.position = (navigator.userAgent.indexOf('MSIE 6') > -1) ? 'absolute' : 'fixed';
    elm.style.position = 'absolute';
    elm.style.top = (height / 2) - (elmheight / 2) + 'px';
    elm.style.left = (width / 2) - (elmwidth / 2) + 'px';
    elm.style.zIndex = 200001;
    elm.style.visibility = "visible";
}


function HideBox(divID) {
    var elm = document.getElementById(divID);
    elm.style.visibility = "hidden";

    document.body.removeChild(document.getElementById('layer'));
    //document.body.removeChild(document.getElementById(divID));

    delete elm;

    CallPostBack("ForceRefresh");


}

function test() {
    //Wait(obj.id);
    CallServer("ZoomAll");
    //alert('test');
}
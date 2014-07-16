var StartPoint = null;
var froze = false;

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


function GetActualLayerType(ID) {
    var lt = document.getElementById("LayerType_" + ID);
    return parseFloat(lt.innerHTML);
}


var mapsID = new Array();
var mapsIDNum = 0;

function Initialize(ID) {

    //Richiama il resize
    AddMapsID(ID);

    //lo connette all'evvento...
    window.onresize = function () { OnResizeMaps(); };

    var elm = document.getElementById(ID);

    if (elm.addEventListener) {
        elm.addEventListener('DOMMouseScroll', wheel, false);
        }
    elm.onmousewheel = wheel;

    OnResize(ID);

}


function AddMapsID(ID) {

    mapsIDNum += 1;

    mapsID.length = mapsIDNum;

    mapsID[mapsIDNum - 1] = ID;

}


function OnResizeMaps() {
 
    for (var i = 0; i < mapsIDNum; i++) {
        OnResize(mapsID[i]);
    }

}


function OnResize(ID) {

    var obj = document.getElementById(ID);

    if (obj == null) {
        alert(ID + " not found");
        return;
    }

    var ret = "Resize|" + ID + "|" + obj.offsetWidth.toString() + "|" + obj.offsetHeight.toString();

    Wait(obj.id);

    CallServer(ID, ret);

}


function Wait(ID) {

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
}


function OnMouseDown(obj, event) {

    var ID = obj.id;
    ID = ID.toString();

    var ActualTool = GetActualToolType(ID);

    event = event || window.event;

    StartPoint = GetObjectPosition(event.clientX, event.clientY, obj);

//    var tl = CheckMouseOnTool(obj, StartPoint);

//    if (tl > -1) {
//        StartPoint = null;
//        froze = true;
//        return; 
//    }

    obj.ondragstart = function () { return false; };
    obj.oncontextmenu = function () { return false; };


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

        case (ToolPan):
            {
                obj.style.cursor = 'move';
            }
            break;

    }

    return false;
}


function OnMouseMove(obj, event) {

    ActualPoint = GetObjectPosition(event.clientX, event.clientY, obj);

    DisplayXY(obj);

    if (froze == true) return;

    event = event || window.event;

    var ID = obj.id;
    ID = ID.toString();

    var ActualTool = GetActualToolType(ID);

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
        case (ToolPanAndClick):
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
                        back.style.left = dx.toString() + 'px';
                        back.style.top = dy.toString() + 'px';
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


function DisplayXY(obj) {

    if (obj == null) return;

    var ID = obj.id;

    if (document.getElementById("MinX_" + ID) == null) return;
    if (document.getElementById("MaxX_" + ID) == null) return;
    if (document.getElementById("MinY_" + ID) == null) return;
    if (document.getElementById("MaxY_" + ID) == null) return;
    if (document.getElementById("IsLatLon_" + ID) == null) return;

    var MinX = parseFloat(document.getElementById("MinX_" + ID).innerHTML);
    var MaxX = parseFloat(document.getElementById("MaxX_" + ID).innerHTML);
    var MinY = parseFloat(document.getElementById("MinY_" + ID).innerHTML);
    var MaxY = parseFloat(document.getElementById("MaxY_" + ID).innerHTML);

    var X = MinX + (ActualPoint.x / obj.offsetWidth) * (MaxX - MinX);
    var Y = MaxY  - (ActualPoint.y / obj.offsetHeight) * (MaxY - MinY);

    if (document.getElementById("IsLatLon_" + ID).innerHTML == "1") {
        self.status = X.toFixed(8) + " " + Y.toFixed(8);
    }
    else {
        self.status = X.toFixed(3) + " " + Y.toFixed(3);
    }
}


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

    froze = false;
    FirstMoved = true;
    
    event = event || window.event;

    EndPoint = GetObjectPosition(event.clientX, event.clientY, obj);

    var ID = obj.id;
    ID = ID.toString();

    var ActualTool = GetActualToolType(ID);

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

//                    CallPostBack(ID, ret);
                    CallServer(ID, ret);
                }

            }
            break;

        case ToolPan:
            {
                if (sft.x != 0 | sft.y != 0) {
                    ret = "Pan|" + sft.x.toString() + "|" + sft.y.toString();
                    Wait(obj.id);
                    CallServer(ID, ret);
                }
                obj.style.cursor = 'default';
            }
            break;

        case ToolPanAndClick:
            {
                if (sft.x == 0 & sft.y == 0) {
                    if (event.button == 1 && window.event != null || event.button == 0) {
                        ret = "Click|0|" + EndPoint.x.toString() + "|" + EndPoint.y.toString();
                        CallPostBack(ID, ret);
                    }
                    else {
                        ret = "Click|1|" + EndPoint.x.toString() + "|" + EndPoint.y.toString();
                        CallPostBack(ID, ret);
                    }
                }
                else {
                    ret = "Pan|" + sft.x.toString() + "|" + sft.y.toString();
                    Wait(obj.id);
                    CallServer(ID, ret);
                }

                obj.style.cursor = 'default';

            }
            break;

        case ToolNone:
            {
                if (sft.x == 0 & sft.y == 0) {
                    if (event.button == 1 && window.event != null || event.button == 0) {
                        ret = "Click|0|" + EndPoint.x.toString() + "|" + EndPoint.y.toString();
                        CallPostBack(ID, ret);
                    }
                    else {
                        ret = "Click|1|" + EndPoint.x.toString() + "|" + EndPoint.y.toString();
                        CallPostBack(ID, ret);
                    }
                }

                obj.style.cursor = 'default';

            }
            break;

        case ToolInfo:
            {
                ret = "Info|1|" + EndPoint.x.toString() + "|" + EndPoint.y.toString();
                CallServer(ID, ret);
            }
            break;

        case ToolSelect:
            {
                ret = "Select|" + StartPoint.x.toString() + "|" + StartPoint.y.toString() + "|" + EndPoint.x.toString() + "|" + EndPoint.y.toString();
                CallServer(ID, ret);
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
                CallServer(ID, ret);
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
    CallServer(obj.id, ret);
}


// Event handler for mouse wheel event.
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


function ReceiveServerData(arg) {

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

        //var nm = document.getElementById("Name_" + elementId);
        //nm.innerHTML = elementId;

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
                }
                break;

            case "REFRESH":
                {
                    canvas.src = elms[2];

                    canvas.style.display = "block";

                    buffer.style.left = "100000px";

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

                    document.getElementById("MinX_" + elementId).innerHTML = elms[10];
                    document.getElementById("MinY_" + elementId).innerHTML = elms[11];
                    document.getElementById("MaxX_" + elementId).innerHTML = elms[12];
                    document.getElementById("MaxY_" + elementId).innerHTML = elms[13];
                }
                break;

            case "REFRESHANDHIDEBUFFER":
                {
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

                    document.getElementById("MinX_" + elementId).innerHTML = elms[10];
                    document.getElementById("MinY_" + elementId).innerHTML = elms[11];
                    document.getElementById("MaxX_" + elementId).innerHTML = elms[12];
                    document.getElementById("MaxY_" + elementId).innerHTML = elms[13];
                }
                break;

            case "POPUP":
                {
                    initPopupDiv("PopupDiv", elms[2], Command);
                    showPopupDiv("PopupDiv");
                }
                break;

            case "POPUPANDREFRESH":
                {
                    initPopupDiv("PopupDiv", elms[2], Command);
                    showPopupDiv("PopupDiv");
                }
                break;

            case "ALERT":
                {
                    alert(elms[2]);  //problem with alert() is that some browsers 
                                     // prompt to block it after repeated alerts
                }

            case "SCRIPT":
                {
                    eval(elms[2]);
                }
                break;

            case "FORCEREFRESH" :
                {
                    CallPostBack(elementId, "ForceRefresh");
                }
                break;
        }

        ColorOnTool(elementId);

    }

    EndWait(elementId);
}


function initPopupDiv(id, html, command) {
    html = "<div style='max-height:500px; max-width:900px; overflow-y:auto; overflow-x:auto'>" + 
           html + "</div>";

    html = html + "<table width='100%'><tr><td style='text-align:center'>";
    html = html + "<input id=\"" + id + "Button\" type=\"button\" value=\"OK\" " +
                  "style=\"width:60px\" " +
                  "onclick=\"HideBox('" + id + "', '" + command + "')\" />";
    html = html + "</td></tr></table>";

    var elm = document.getElementById(id);
    if (elm == null) {
        createPopupDiv(id, html, 'auto', 'auto', 100000);
    }
    else {
        elm.innerHTML = html;
    }
}


function createPopupDiv(id, html, width, height, left, top) {

    var newdiv = document.createElement('div');
    newdiv.setAttribute('id', id);

    if (width) {
        newdiv.style.width = width;
    }
    newdiv.style.minWidth = "200px";

    if (height) {
        newdiv.style.height = height;
    }
//    newdiv.style.minHeight = "100px";

    if ((left || top) || (left && top)) {
        newdiv.style.position = "absolute";

        if (left) {
            newdiv.style.left = left;
        }

        if (top) {
            newdiv.style.top = top;
        }
    }

    newdiv.style.background = "white";
    newdiv.style.border = "2px solid black";
    newdiv.style.padding = "4px";

//    newdiv.style.fontFamily = "Tahoma, Geneva, sans-serif";
//    newdiv.style.fontFamily = "'Trebuchet MS', Helvetica, sans-serif";
//    newdiv.style.fontFamily = "Arial, Helvetica, sans-serif";
    newdiv.style.fontFamily = "'Lucida Sans Unicode', 'Lucida Grande', sans-serif";
    newdiv.style.fontSize = "10pt";

    if (html) {
        newdiv.innerHTML = html;
    } else {
        newdiv.innerHTML = "nothing";
    }

    document.body.appendChild(newdiv);
} 


function showPopupDiv(divID) {

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


function HideBox(divID, command) {
    var elm = document.getElementById(divID);
    elm.style.visibility = "hidden";

    document.body.removeChild(document.getElementById('layer'));
    //document.body.removeChild(document.getElementById(divID));

    delete elm;

    if (command == "POPUPANDREFRESH") {
      CallPostBack(divID, "ForceRefresh");
    }  
}


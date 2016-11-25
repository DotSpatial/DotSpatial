﻿// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Initial Developer of this Original Code is  Peter Hammond/Jia Liang Liu
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name                        |   Date             |         Comments
//-----------------------------|--------------------|-----------------------------------------------
// Peter Hammond/Jia Liang Liu |  02/20/2010        |  view change event argument
// ********************************************************************************************************

using System;
using System.Drawing;

namespace DotSpatial.Controls
{
    public class ViewChangedEventArgs : EventArgs
    {
        public Rectangle OldView { get; set; }
        public Rectangle NewView { get; set; }
    }
}
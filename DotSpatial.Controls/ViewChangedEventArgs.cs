﻿// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
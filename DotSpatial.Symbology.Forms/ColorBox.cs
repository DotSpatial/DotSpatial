// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/20/2008 10:54:05 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// ColorBox
    /// </summary>
    [DefaultEvent("SelectedItemChanged")]
    [DefaultProperty("Value")]
    public class ColorBox : UserControl
    {
        #region Events

        /// <summary>
        /// Occurs when the selected color has been changed in the drop-down
        /// </summary>
        public event EventHandler SelectedItemChanged;

        #endregion

        private ColorDropDown cddColor;
        private Button cmdShowDialog;
        private Label lblColor;

        #region Private Variables

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ColorBox
        /// </summary>
        public ColorBox()
        {
            InitializeComponent();
            cddColor.SelectedIndexChanged += cddColor_SelectedIndexChanged;
        }

        private void cddColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedItemChanged != null) SelectedItemChanged(this, EventArgs.Empty);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ColorBox));
            this.lblColor = new Label();
            this.cddColor = new ColorDropDown();
            this.cmdShowDialog = new Button();
            this.SuspendLayout();
            //
            // lblColor
            //
            resources.ApplyResources(this.lblColor, "lblColor");
            this.lblColor.Name = "lblColor";
            //
            // cddColor
            //
            resources.ApplyResources(this.cddColor, "cddColor");
            this.cddColor.DrawMode = DrawMode.OwnerDrawFixed;
            this.cddColor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cddColor.FormattingEnabled = true;
            this.cddColor.Items.AddRange(new[] {
                                                   ((resources.GetObject("cddColor.Items"))),
                                                   ((resources.GetObject("cddColor.Items1"))),
                                                   ((resources.GetObject("cddColor.Items2"))),
                                                   ((resources.GetObject("cddColor.Items3"))),
                                                   ((resources.GetObject("cddColor.Items4"))),
                                                   ((resources.GetObject("cddColor.Items5"))),
                                                   ((resources.GetObject("cddColor.Items6"))),
                                                   ((resources.GetObject("cddColor.Items7"))),
                                                   ((resources.GetObject("cddColor.Items8"))),
                                                   ((resources.GetObject("cddColor.Items9"))),
                                                   ((resources.GetObject("cddColor.Items10"))),
                                                   ((resources.GetObject("cddColor.Items11"))),
                                                   ((resources.GetObject("cddColor.Items12"))),
                                                   ((resources.GetObject("cddColor.Items13"))),
                                                   ((resources.GetObject("cddColor.Items14"))),
                                                   ((resources.GetObject("cddColor.Items15"))),
                                                   ((resources.GetObject("cddColor.Items16"))),
                                                   ((resources.GetObject("cddColor.Items17"))),
                                                   ((resources.GetObject("cddColor.Items18"))),
                                                   ((resources.GetObject("cddColor.Items19"))),
                                                   ((resources.GetObject("cddColor.Items20"))),
                                                   ((resources.GetObject("cddColor.Items21"))),
                                                   ((resources.GetObject("cddColor.Items22"))),
                                                   ((resources.GetObject("cddColor.Items23"))),
                                                   ((resources.GetObject("cddColor.Items24"))),
                                                   ((resources.GetObject("cddColor.Items25"))),
                                                   ((resources.GetObject("cddColor.Items26"))),
                                                   ((resources.GetObject("cddColor.Items27"))),
                                                   ((resources.GetObject("cddColor.Items28"))),
                                                   ((resources.GetObject("cddColor.Items29"))),
                                                   ((resources.GetObject("cddColor.Items30"))),
                                                   ((resources.GetObject("cddColor.Items31"))),
                                                   ((resources.GetObject("cddColor.Items32"))),
                                                   ((resources.GetObject("cddColor.Items33"))),
                                                   ((resources.GetObject("cddColor.Items34"))),
                                                   ((resources.GetObject("cddColor.Items35"))),
                                                   ((resources.GetObject("cddColor.Items36"))),
                                                   ((resources.GetObject("cddColor.Items37"))),
                                                   ((resources.GetObject("cddColor.Items38"))),
                                                   ((resources.GetObject("cddColor.Items39"))),
                                                   ((resources.GetObject("cddColor.Items40"))),
                                                   ((resources.GetObject("cddColor.Items41"))),
                                                   ((resources.GetObject("cddColor.Items42"))),
                                                   ((resources.GetObject("cddColor.Items43"))),
                                                   ((resources.GetObject("cddColor.Items44"))),
                                                   ((resources.GetObject("cddColor.Items45"))),
                                                   ((resources.GetObject("cddColor.Items46"))),
                                                   ((resources.GetObject("cddColor.Items47"))),
                                                   ((resources.GetObject("cddColor.Items48"))),
                                                   ((resources.GetObject("cddColor.Items49"))),
                                                   ((resources.GetObject("cddColor.Items50"))),
                                                   ((resources.GetObject("cddColor.Items51"))),
                                                   ((resources.GetObject("cddColor.Items52"))),
                                                   ((resources.GetObject("cddColor.Items53"))),
                                                   ((resources.GetObject("cddColor.Items54"))),
                                                   ((resources.GetObject("cddColor.Items55"))),
                                                   ((resources.GetObject("cddColor.Items56"))),
                                                   ((resources.GetObject("cddColor.Items57"))),
                                                   ((resources.GetObject("cddColor.Items58"))),
                                                   ((resources.GetObject("cddColor.Items59"))),
                                                   ((resources.GetObject("cddColor.Items60"))),
                                                   ((resources.GetObject("cddColor.Items61"))),
                                                   ((resources.GetObject("cddColor.Items62"))),
                                                   ((resources.GetObject("cddColor.Items63"))),
                                                   ((resources.GetObject("cddColor.Items64"))),
                                                   ((resources.GetObject("cddColor.Items65"))),
                                                   ((resources.GetObject("cddColor.Items66"))),
                                                   ((resources.GetObject("cddColor.Items67"))),
                                                   ((resources.GetObject("cddColor.Items68"))),
                                                   ((resources.GetObject("cddColor.Items69"))),
                                                   ((resources.GetObject("cddColor.Items70"))),
                                                   ((resources.GetObject("cddColor.Items71"))),
                                                   ((resources.GetObject("cddColor.Items72"))),
                                                   ((resources.GetObject("cddColor.Items73"))),
                                                   ((resources.GetObject("cddColor.Items74"))),
                                                   ((resources.GetObject("cddColor.Items75"))),
                                                   ((resources.GetObject("cddColor.Items76"))),
                                                   ((resources.GetObject("cddColor.Items77"))),
                                                   ((resources.GetObject("cddColor.Items78"))),
                                                   ((resources.GetObject("cddColor.Items79"))),
                                                   ((resources.GetObject("cddColor.Items80"))),
                                                   ((resources.GetObject("cddColor.Items81"))),
                                                   ((resources.GetObject("cddColor.Items82"))),
                                                   ((resources.GetObject("cddColor.Items83"))),
                                                   ((resources.GetObject("cddColor.Items84"))),
                                                   ((resources.GetObject("cddColor.Items85"))),
                                                   ((resources.GetObject("cddColor.Items86"))),
                                                   ((resources.GetObject("cddColor.Items87"))),
                                                   ((resources.GetObject("cddColor.Items88"))),
                                                   ((resources.GetObject("cddColor.Items89"))),
                                                   ((resources.GetObject("cddColor.Items90"))),
                                                   ((resources.GetObject("cddColor.Items91"))),
                                                   ((resources.GetObject("cddColor.Items92"))),
                                                   ((resources.GetObject("cddColor.Items93"))),
                                                   ((resources.GetObject("cddColor.Items94"))),
                                                   ((resources.GetObject("cddColor.Items95"))),
                                                   ((resources.GetObject("cddColor.Items96"))),
                                                   ((resources.GetObject("cddColor.Items97"))),
                                                   ((resources.GetObject("cddColor.Items98"))),
                                                   ((resources.GetObject("cddColor.Items99"))),
                                                   ((resources.GetObject("cddColor.Items100"))),
                                                   ((resources.GetObject("cddColor.Items101"))),
                                                   ((resources.GetObject("cddColor.Items102"))),
                                                   ((resources.GetObject("cddColor.Items103"))),
                                                   ((resources.GetObject("cddColor.Items104"))),
                                                   ((resources.GetObject("cddColor.Items105"))),
                                                   ((resources.GetObject("cddColor.Items106"))),
                                                   ((resources.GetObject("cddColor.Items107"))),
                                                   ((resources.GetObject("cddColor.Items108"))),
                                                   ((resources.GetObject("cddColor.Items109"))),
                                                   ((resources.GetObject("cddColor.Items110"))),
                                                   ((resources.GetObject("cddColor.Items111"))),
                                                   ((resources.GetObject("cddColor.Items112"))),
                                                   ((resources.GetObject("cddColor.Items113"))),
                                                   ((resources.GetObject("cddColor.Items114"))),
                                                   ((resources.GetObject("cddColor.Items115"))),
                                                   ((resources.GetObject("cddColor.Items116"))),
                                                   ((resources.GetObject("cddColor.Items117"))),
                                                   ((resources.GetObject("cddColor.Items118"))),
                                                   ((resources.GetObject("cddColor.Items119"))),
                                                   ((resources.GetObject("cddColor.Items120"))),
                                                   ((resources.GetObject("cddColor.Items121"))),
                                                   ((resources.GetObject("cddColor.Items122"))),
                                                   ((resources.GetObject("cddColor.Items123"))),
                                                   ((resources.GetObject("cddColor.Items124"))),
                                                   ((resources.GetObject("cddColor.Items125"))),
                                                   ((resources.GetObject("cddColor.Items126"))),
                                                   ((resources.GetObject("cddColor.Items127"))),
                                                   ((resources.GetObject("cddColor.Items128"))),
                                                   ((resources.GetObject("cddColor.Items129"))),
                                                   ((resources.GetObject("cddColor.Items130"))),
                                                   ((resources.GetObject("cddColor.Items131"))),
                                                   ((resources.GetObject("cddColor.Items132"))),
                                                   ((resources.GetObject("cddColor.Items133"))),
                                                   ((resources.GetObject("cddColor.Items134"))),
                                                   ((resources.GetObject("cddColor.Items135"))),
                                                   ((resources.GetObject("cddColor.Items136"))),
                                                   ((resources.GetObject("cddColor.Items137"))),
                                                   ((resources.GetObject("cddColor.Items138"))),
                                                   ((resources.GetObject("cddColor.Items139"))),
                                                   ((resources.GetObject("cddColor.Items140"))),
                                                   ((resources.GetObject("cddColor.Items141"))),
                                                   ((resources.GetObject("cddColor.Items142"))),
                                                   ((resources.GetObject("cddColor.Items143"))),
                                                   ((resources.GetObject("cddColor.Items144"))),
                                                   ((resources.GetObject("cddColor.Items145"))),
                                                   ((resources.GetObject("cddColor.Items146"))),
                                                   ((resources.GetObject("cddColor.Items147"))),
                                                   ((resources.GetObject("cddColor.Items148"))),
                                                   ((resources.GetObject("cddColor.Items149"))),
                                                   ((resources.GetObject("cddColor.Items150"))),
                                                   ((resources.GetObject("cddColor.Items151"))),
                                                   ((resources.GetObject("cddColor.Items152"))),
                                                   ((resources.GetObject("cddColor.Items153"))),
                                                   ((resources.GetObject("cddColor.Items154"))),
                                                   ((resources.GetObject("cddColor.Items155"))),
                                                   ((resources.GetObject("cddColor.Items156"))),
                                                   ((resources.GetObject("cddColor.Items157"))),
                                                   ((resources.GetObject("cddColor.Items158"))),
                                                   ((resources.GetObject("cddColor.Items159"))),
                                                   ((resources.GetObject("cddColor.Items160"))),
                                                   ((resources.GetObject("cddColor.Items161"))),
                                                   ((resources.GetObject("cddColor.Items162"))),
                                                   ((resources.GetObject("cddColor.Items163"))),
                                                   ((resources.GetObject("cddColor.Items164"))),
                                                   ((resources.GetObject("cddColor.Items165"))),
                                                   ((resources.GetObject("cddColor.Items166"))),
                                                   ((resources.GetObject("cddColor.Items167"))),
                                                   ((resources.GetObject("cddColor.Items168"))),
                                                   ((resources.GetObject("cddColor.Items169"))),
                                                   ((resources.GetObject("cddColor.Items170"))),
                                                   ((resources.GetObject("cddColor.Items171"))),
                                                   ((resources.GetObject("cddColor.Items172"))),
                                                   ((resources.GetObject("cddColor.Items173"))),
                                                   ((resources.GetObject("cddColor.Items174"))),
                                                   ((resources.GetObject("cddColor.Items175"))),
                                                   ((resources.GetObject("cddColor.Items176"))),
                                                   ((resources.GetObject("cddColor.Items177"))),
                                                   ((resources.GetObject("cddColor.Items178"))),
                                                   ((resources.GetObject("cddColor.Items179"))),
                                                   ((resources.GetObject("cddColor.Items180"))),
                                                   ((resources.GetObject("cddColor.Items181"))),
                                                   ((resources.GetObject("cddColor.Items182"))),
                                                   ((resources.GetObject("cddColor.Items183"))),
                                                   ((resources.GetObject("cddColor.Items184"))),
                                                   ((resources.GetObject("cddColor.Items185"))),
                                                   ((resources.GetObject("cddColor.Items186"))),
                                                   ((resources.GetObject("cddColor.Items187"))),
                                                   ((resources.GetObject("cddColor.Items188"))),
                                                   ((resources.GetObject("cddColor.Items189"))),
                                                   ((resources.GetObject("cddColor.Items190"))),
                                                   ((resources.GetObject("cddColor.Items191"))),
                                                   ((resources.GetObject("cddColor.Items192"))),
                                                   ((resources.GetObject("cddColor.Items193"))),
                                                   ((resources.GetObject("cddColor.Items194"))),
                                                   ((resources.GetObject("cddColor.Items195"))),
                                                   ((resources.GetObject("cddColor.Items196"))),
                                                   ((resources.GetObject("cddColor.Items197"))),
                                                   ((resources.GetObject("cddColor.Items198"))),
                                                   ((resources.GetObject("cddColor.Items199"))),
                                                   ((resources.GetObject("cddColor.Items200"))),
                                                   ((resources.GetObject("cddColor.Items201"))),
                                                   ((resources.GetObject("cddColor.Items202"))),
                                                   ((resources.GetObject("cddColor.Items203"))),
                                                   ((resources.GetObject("cddColor.Items204"))),
                                                   ((resources.GetObject("cddColor.Items205"))),
                                                   ((resources.GetObject("cddColor.Items206"))),
                                                   ((resources.GetObject("cddColor.Items207"))),
                                                   ((resources.GetObject("cddColor.Items208"))),
                                                   ((resources.GetObject("cddColor.Items209"))),
                                                   ((resources.GetObject("cddColor.Items210"))),
                                                   ((resources.GetObject("cddColor.Items211"))),
                                                   ((resources.GetObject("cddColor.Items212"))),
                                                   ((resources.GetObject("cddColor.Items213"))),
                                                   ((resources.GetObject("cddColor.Items214"))),
                                                   ((resources.GetObject("cddColor.Items215"))),
                                                   ((resources.GetObject("cddColor.Items216"))),
                                                   ((resources.GetObject("cddColor.Items217"))),
                                                   ((resources.GetObject("cddColor.Items218"))),
                                                   ((resources.GetObject("cddColor.Items219"))),
                                                   ((resources.GetObject("cddColor.Items220"))),
                                                   ((resources.GetObject("cddColor.Items221"))),
                                                   ((resources.GetObject("cddColor.Items222"))),
                                                   ((resources.GetObject("cddColor.Items223"))),
                                                   ((resources.GetObject("cddColor.Items224"))),
                                                   ((resources.GetObject("cddColor.Items225"))),
                                                   ((resources.GetObject("cddColor.Items226"))),
                                                   ((resources.GetObject("cddColor.Items227"))),
                                                   ((resources.GetObject("cddColor.Items228"))),
                                                   ((resources.GetObject("cddColor.Items229"))),
                                                   ((resources.GetObject("cddColor.Items230"))),
                                                   ((resources.GetObject("cddColor.Items231"))),
                                                   ((resources.GetObject("cddColor.Items232"))),
                                                   ((resources.GetObject("cddColor.Items233"))),
                                                   ((resources.GetObject("cddColor.Items234"))),
                                                   ((resources.GetObject("cddColor.Items235"))),
                                                   ((resources.GetObject("cddColor.Items236"))),
                                                   ((resources.GetObject("cddColor.Items237"))),
                                                   ((resources.GetObject("cddColor.Items238"))),
                                                   ((resources.GetObject("cddColor.Items239"))),
                                                   ((resources.GetObject("cddColor.Items240"))),
                                                   ((resources.GetObject("cddColor.Items241"))),
                                                   ((resources.GetObject("cddColor.Items242"))),
                                                   ((resources.GetObject("cddColor.Items243"))),
                                                   ((resources.GetObject("cddColor.Items244"))),
                                                   ((resources.GetObject("cddColor.Items245"))),
                                                   ((resources.GetObject("cddColor.Items246"))),
                                                   ((resources.GetObject("cddColor.Items247"))),
                                                   ((resources.GetObject("cddColor.Items248"))),
                                                   ((resources.GetObject("cddColor.Items249"))),
                                                   ((resources.GetObject("cddColor.Items250"))),
                                                   ((resources.GetObject("cddColor.Items251"))),
                                                   ((resources.GetObject("cddColor.Items252"))),
                                                   ((resources.GetObject("cddColor.Items253"))),
                                                   ((resources.GetObject("cddColor.Items254"))),
                                                   ((resources.GetObject("cddColor.Items255"))),
                                                   ((resources.GetObject("cddColor.Items256"))),
                                                   ((resources.GetObject("cddColor.Items257"))),
                                                   ((resources.GetObject("cddColor.Items258"))),
                                                   ((resources.GetObject("cddColor.Items259"))),
                                                   ((resources.GetObject("cddColor.Items260"))),
                                                   ((resources.GetObject("cddColor.Items261"))),
                                                   ((resources.GetObject("cddColor.Items262"))),
                                                   ((resources.GetObject("cddColor.Items263"))),
                                                   ((resources.GetObject("cddColor.Items264"))),
                                                   ((resources.GetObject("cddColor.Items265"))),
                                                   ((resources.GetObject("cddColor.Items266"))),
                                                   ((resources.GetObject("cddColor.Items267"))),
                                                   ((resources.GetObject("cddColor.Items268"))),
                                                   ((resources.GetObject("cddColor.Items269"))),
                                                   ((resources.GetObject("cddColor.Items270"))),
                                                   ((resources.GetObject("cddColor.Items271"))),
                                                   ((resources.GetObject("cddColor.Items272"))),
                                                   ((resources.GetObject("cddColor.Items273"))),
                                                   ((resources.GetObject("cddColor.Items274"))),
                                                   ((resources.GetObject("cddColor.Items275"))),
                                                   ((resources.GetObject("cddColor.Items276"))),
                                                   ((resources.GetObject("cddColor.Items277"))),
                                                   ((resources.GetObject("cddColor.Items278"))),
                                                   ((resources.GetObject("cddColor.Items279"))),
                                                   ((resources.GetObject("cddColor.Items280"))),
                                                   ((resources.GetObject("cddColor.Items281"))),
                                                   ((resources.GetObject("cddColor.Items282"))),
                                                   ((resources.GetObject("cddColor.Items283"))),
                                                   ((resources.GetObject("cddColor.Items284"))),
                                                   ((resources.GetObject("cddColor.Items285"))),
                                                   ((resources.GetObject("cddColor.Items286"))),
                                                   ((resources.GetObject("cddColor.Items287"))),
                                                   ((resources.GetObject("cddColor.Items288"))),
                                                   ((resources.GetObject("cddColor.Items289"))),
                                                   ((resources.GetObject("cddColor.Items290"))),
                                                   ((resources.GetObject("cddColor.Items291"))),
                                                   ((resources.GetObject("cddColor.Items292"))),
                                                   ((resources.GetObject("cddColor.Items293"))),
                                                   ((resources.GetObject("cddColor.Items294"))),
                                                   ((resources.GetObject("cddColor.Items295"))),
                                                   ((resources.GetObject("cddColor.Items296"))),
                                                   ((resources.GetObject("cddColor.Items297"))),
                                                   ((resources.GetObject("cddColor.Items298"))),
                                                   ((resources.GetObject("cddColor.Items299"))),
                                                   ((resources.GetObject("cddColor.Items300"))),
                                                   ((resources.GetObject("cddColor.Items301"))),
                                                   ((resources.GetObject("cddColor.Items302"))),
                                                   ((resources.GetObject("cddColor.Items303"))),
                                                   ((resources.GetObject("cddColor.Items304"))),
                                                   ((resources.GetObject("cddColor.Items305"))),
                                                   ((resources.GetObject("cddColor.Items306"))),
                                                   ((resources.GetObject("cddColor.Items307"))),
                                                   ((resources.GetObject("cddColor.Items308"))),
                                                   ((resources.GetObject("cddColor.Items309"))),
                                                   ((resources.GetObject("cddColor.Items310"))),
                                                   ((resources.GetObject("cddColor.Items311"))),
                                                   ((resources.GetObject("cddColor.Items312"))),
                                                   ((resources.GetObject("cddColor.Items313"))),
                                                   ((resources.GetObject("cddColor.Items314"))),
                                                   ((resources.GetObject("cddColor.Items315"))),
                                                   ((resources.GetObject("cddColor.Items316"))),
                                                   ((resources.GetObject("cddColor.Items317"))),
                                                   ((resources.GetObject("cddColor.Items318"))),
                                                   ((resources.GetObject("cddColor.Items319"))),
                                                   ((resources.GetObject("cddColor.Items320"))),
                                                   ((resources.GetObject("cddColor.Items321"))),
                                                   ((resources.GetObject("cddColor.Items322"))),
                                                   ((resources.GetObject("cddColor.Items323"))),
                                                   ((resources.GetObject("cddColor.Items324"))),
                                                   ((resources.GetObject("cddColor.Items325"))),
                                                   ((resources.GetObject("cddColor.Items326"))),
                                                   ((resources.GetObject("cddColor.Items327"))),
                                                   ((resources.GetObject("cddColor.Items328"))),
                                                   ((resources.GetObject("cddColor.Items329"))),
                                                   ((resources.GetObject("cddColor.Items330"))),
                                                   ((resources.GetObject("cddColor.Items331"))),
                                                   ((resources.GetObject("cddColor.Items332"))),
                                                   ((resources.GetObject("cddColor.Items333"))),
                                                   ((resources.GetObject("cddColor.Items334"))),
                                                   ((resources.GetObject("cddColor.Items335"))),
                                                   ((resources.GetObject("cddColor.Items336"))),
                                                   ((resources.GetObject("cddColor.Items337"))),
                                                   ((resources.GetObject("cddColor.Items338"))),
                                                   ((resources.GetObject("cddColor.Items339"))),
                                                   ((resources.GetObject("cddColor.Items340"))),
                                                   ((resources.GetObject("cddColor.Items341"))),
                                                   ((resources.GetObject("cddColor.Items342"))),
                                                   ((resources.GetObject("cddColor.Items343"))),
                                                   ((resources.GetObject("cddColor.Items344"))),
                                                   ((resources.GetObject("cddColor.Items345"))),
                                                   ((resources.GetObject("cddColor.Items346"))),
                                                   ((resources.GetObject("cddColor.Items347"))),
                                                   ((resources.GetObject("cddColor.Items348"))),
                                                   ((resources.GetObject("cddColor.Items349"))),
                                                   ((resources.GetObject("cddColor.Items350"))),
                                                   ((resources.GetObject("cddColor.Items351"))),
                                                   ((resources.GetObject("cddColor.Items352"))),
                                                   ((resources.GetObject("cddColor.Items353"))),
                                                   ((resources.GetObject("cddColor.Items354"))),
                                                   ((resources.GetObject("cddColor.Items355"))),
                                                   ((resources.GetObject("cddColor.Items356"))),
                                                   ((resources.GetObject("cddColor.Items357"))),
                                                   ((resources.GetObject("cddColor.Items358"))),
                                                   ((resources.GetObject("cddColor.Items359"))),
                                                   ((resources.GetObject("cddColor.Items360"))),
                                                   ((resources.GetObject("cddColor.Items361"))),
                                                   ((resources.GetObject("cddColor.Items362"))),
                                                   ((resources.GetObject("cddColor.Items363"))),
                                                   ((resources.GetObject("cddColor.Items364"))),
                                                   ((resources.GetObject("cddColor.Items365"))),
                                                   ((resources.GetObject("cddColor.Items366"))),
                                                   ((resources.GetObject("cddColor.Items367"))),
                                                   ((resources.GetObject("cddColor.Items368"))),
                                                   ((resources.GetObject("cddColor.Items369"))),
                                                   ((resources.GetObject("cddColor.Items370"))),
                                                   ((resources.GetObject("cddColor.Items371"))),
                                                   ((resources.GetObject("cddColor.Items372"))),
                                                   ((resources.GetObject("cddColor.Items373"))),
                                                   ((resources.GetObject("cddColor.Items374"))),
                                                   ((resources.GetObject("cddColor.Items375"))),
                                                   ((resources.GetObject("cddColor.Items376"))),
                                                   ((resources.GetObject("cddColor.Items377"))),
                                                   ((resources.GetObject("cddColor.Items378"))),
                                                   ((resources.GetObject("cddColor.Items379"))),
                                                   ((resources.GetObject("cddColor.Items380"))),
                                                   ((resources.GetObject("cddColor.Items381"))),
                                                   ((resources.GetObject("cddColor.Items382"))),
                                                   ((resources.GetObject("cddColor.Items383"))),
                                                   ((resources.GetObject("cddColor.Items384"))),
                                                   ((resources.GetObject("cddColor.Items385"))),
                                                   ((resources.GetObject("cddColor.Items386"))),
                                                   ((resources.GetObject("cddColor.Items387"))),
                                                   ((resources.GetObject("cddColor.Items388"))),
                                                   ((resources.GetObject("cddColor.Items389"))),
                                                   ((resources.GetObject("cddColor.Items390"))),
                                                   ((resources.GetObject("cddColor.Items391"))),
                                                   ((resources.GetObject("cddColor.Items392"))),
                                                   ((resources.GetObject("cddColor.Items393"))),
                                                   ((resources.GetObject("cddColor.Items394"))),
                                                   ((resources.GetObject("cddColor.Items395"))),
                                                   ((resources.GetObject("cddColor.Items396"))),
                                                   ((resources.GetObject("cddColor.Items397"))),
                                                   ((resources.GetObject("cddColor.Items398"))),
                                                   ((resources.GetObject("cddColor.Items399"))),
                                                   ((resources.GetObject("cddColor.Items400"))),
                                                   ((resources.GetObject("cddColor.Items401"))),
                                                   ((resources.GetObject("cddColor.Items402"))),
                                                   ((resources.GetObject("cddColor.Items403"))),
                                                   ((resources.GetObject("cddColor.Items404"))),
                                                   ((resources.GetObject("cddColor.Items405"))),
                                                   ((resources.GetObject("cddColor.Items406"))),
                                                   ((resources.GetObject("cddColor.Items407"))),
                                                   ((resources.GetObject("cddColor.Items408"))),
                                                   ((resources.GetObject("cddColor.Items409"))),
                                                   ((resources.GetObject("cddColor.Items410"))),
                                                   ((resources.GetObject("cddColor.Items411"))),
                                                   ((resources.GetObject("cddColor.Items412"))),
                                                   ((resources.GetObject("cddColor.Items413"))),
                                                   ((resources.GetObject("cddColor.Items414"))),
                                                   ((resources.GetObject("cddColor.Items415"))),
                                                   ((resources.GetObject("cddColor.Items416"))),
                                                   ((resources.GetObject("cddColor.Items417"))),
                                                   ((resources.GetObject("cddColor.Items418"))),
                                                   ((resources.GetObject("cddColor.Items419"))),
                                                   ((resources.GetObject("cddColor.Items420"))),
                                                   ((resources.GetObject("cddColor.Items421"))),
                                                   ((resources.GetObject("cddColor.Items422"))),
                                                   ((resources.GetObject("cddColor.Items423"))),
                                                   ((resources.GetObject("cddColor.Items424"))),
                                                   ((resources.GetObject("cddColor.Items425"))),
                                                   ((resources.GetObject("cddColor.Items426"))),
                                                   ((resources.GetObject("cddColor.Items427"))),
                                                   ((resources.GetObject("cddColor.Items428"))),
                                                   ((resources.GetObject("cddColor.Items429"))),
                                                   ((resources.GetObject("cddColor.Items430"))),
                                                   ((resources.GetObject("cddColor.Items431"))),
                                                   ((resources.GetObject("cddColor.Items432"))),
                                                   ((resources.GetObject("cddColor.Items433"))),
                                                   ((resources.GetObject("cddColor.Items434"))),
                                                   ((resources.GetObject("cddColor.Items435"))),
                                                   ((resources.GetObject("cddColor.Items436"))),
                                                   ((resources.GetObject("cddColor.Items437"))),
                                                   ((resources.GetObject("cddColor.Items438"))),
                                                   ((resources.GetObject("cddColor.Items439"))),
                                                   ((resources.GetObject("cddColor.Items440"))),
                                                   ((resources.GetObject("cddColor.Items441"))),
                                                   ((resources.GetObject("cddColor.Items442"))),
                                                   ((resources.GetObject("cddColor.Items443"))),
                                                   ((resources.GetObject("cddColor.Items444"))),
                                                   ((resources.GetObject("cddColor.Items445"))),
                                                   ((resources.GetObject("cddColor.Items446"))),
                                                   ((resources.GetObject("cddColor.Items447"))),
                                                   ((resources.GetObject("cddColor.Items448"))),
                                                   ((resources.GetObject("cddColor.Items449"))),
                                                   ((resources.GetObject("cddColor.Items450"))),
                                                   ((resources.GetObject("cddColor.Items451"))),
                                                   ((resources.GetObject("cddColor.Items452"))),
                                                   ((resources.GetObject("cddColor.Items453"))),
                                                   ((resources.GetObject("cddColor.Items454"))),
                                                   ((resources.GetObject("cddColor.Items455"))),
                                                   ((resources.GetObject("cddColor.Items456"))),
                                                   ((resources.GetObject("cddColor.Items457"))),
                                                   ((resources.GetObject("cddColor.Items458"))),
                                                   ((resources.GetObject("cddColor.Items459"))),
                                                   ((resources.GetObject("cddColor.Items460"))),
                                                   ((resources.GetObject("cddColor.Items461"))),
                                                   ((resources.GetObject("cddColor.Items462"))),
                                                   ((resources.GetObject("cddColor.Items463"))),
                                                   ((resources.GetObject("cddColor.Items464"))),
                                                   ((resources.GetObject("cddColor.Items465"))),
                                                   ((resources.GetObject("cddColor.Items466"))),
                                                   ((resources.GetObject("cddColor.Items467"))),
                                                   ((resources.GetObject("cddColor.Items468"))),
                                                   ((resources.GetObject("cddColor.Items469"))),
                                                   ((resources.GetObject("cddColor.Items470"))),
                                                   ((resources.GetObject("cddColor.Items471"))),
                                                   ((resources.GetObject("cddColor.Items472"))),
                                                   ((resources.GetObject("cddColor.Items473"))),
                                                   ((resources.GetObject("cddColor.Items474"))),
                                                   ((resources.GetObject("cddColor.Items475"))),
                                                   ((resources.GetObject("cddColor.Items476"))),
                                                   ((resources.GetObject("cddColor.Items477"))),
                                                   ((resources.GetObject("cddColor.Items478"))),
                                                   ((resources.GetObject("cddColor.Items479"))),
                                                   ((resources.GetObject("cddColor.Items480"))),
                                                   ((resources.GetObject("cddColor.Items481"))),
                                                   ((resources.GetObject("cddColor.Items482"))),
                                                   ((resources.GetObject("cddColor.Items483"))),
                                                   ((resources.GetObject("cddColor.Items484"))),
                                                   ((resources.GetObject("cddColor.Items485"))),
                                                   ((resources.GetObject("cddColor.Items486"))),
                                                   ((resources.GetObject("cddColor.Items487"))),
                                                   ((resources.GetObject("cddColor.Items488"))),
                                                   ((resources.GetObject("cddColor.Items489"))),
                                                   ((resources.GetObject("cddColor.Items490"))),
                                                   ((resources.GetObject("cddColor.Items491"))),
                                                   ((resources.GetObject("cddColor.Items492"))),
                                                   ((resources.GetObject("cddColor.Items493"))),
                                                   ((resources.GetObject("cddColor.Items494"))),
                                                   ((resources.GetObject("cddColor.Items495"))),
                                                   ((resources.GetObject("cddColor.Items496"))),
                                                   ((resources.GetObject("cddColor.Items497"))),
                                                   ((resources.GetObject("cddColor.Items498"))),
                                                   ((resources.GetObject("cddColor.Items499"))),
                                                   ((resources.GetObject("cddColor.Items500"))),
                                                   ((resources.GetObject("cddColor.Items501"))),
                                                   ((resources.GetObject("cddColor.Items502"))),
                                                   ((resources.GetObject("cddColor.Items503"))),
                                                   ((resources.GetObject("cddColor.Items504"))),
                                                   ((resources.GetObject("cddColor.Items505"))),
                                                   ((resources.GetObject("cddColor.Items506"))),
                                                   ((resources.GetObject("cddColor.Items507"))),
                                                   ((resources.GetObject("cddColor.Items508"))),
                                                   ((resources.GetObject("cddColor.Items509"))),
                                                   ((resources.GetObject("cddColor.Items510"))),
                                                   ((resources.GetObject("cddColor.Items511"))),
                                                   ((resources.GetObject("cddColor.Items512"))),
                                                   ((resources.GetObject("cddColor.Items513"))),
                                                   ((resources.GetObject("cddColor.Items514"))),
                                                   ((resources.GetObject("cddColor.Items515"))),
                                                   ((resources.GetObject("cddColor.Items516"))),
                                                   ((resources.GetObject("cddColor.Items517"))),
                                                   ((resources.GetObject("cddColor.Items518"))),
                                                   ((resources.GetObject("cddColor.Items519"))),
                                                   ((resources.GetObject("cddColor.Items520"))),
                                                   ((resources.GetObject("cddColor.Items521"))),
                                                   ((resources.GetObject("cddColor.Items522"))),
                                                   ((resources.GetObject("cddColor.Items523"))),
                                                   ((resources.GetObject("cddColor.Items524"))),
                                                   ((resources.GetObject("cddColor.Items525"))),
                                                   ((resources.GetObject("cddColor.Items526"))),
                                                   ((resources.GetObject("cddColor.Items527"))),
                                                   ((resources.GetObject("cddColor.Items528"))),
                                                   ((resources.GetObject("cddColor.Items529"))),
                                                   ((resources.GetObject("cddColor.Items530"))),
                                                   ((resources.GetObject("cddColor.Items531"))),
                                                   ((resources.GetObject("cddColor.Items532"))),
                                                   ((resources.GetObject("cddColor.Items533"))),
                                                   ((resources.GetObject("cddColor.Items534"))),
                                                   ((resources.GetObject("cddColor.Items535"))),
                                                   ((resources.GetObject("cddColor.Items536"))),
                                                   ((resources.GetObject("cddColor.Items537"))),
                                                   ((resources.GetObject("cddColor.Items538"))),
                                                   ((resources.GetObject("cddColor.Items539"))),
                                                   ((resources.GetObject("cddColor.Items540"))),
                                                   ((resources.GetObject("cddColor.Items541"))),
                                                   ((resources.GetObject("cddColor.Items542"))),
                                                   ((resources.GetObject("cddColor.Items543"))),
                                                   ((resources.GetObject("cddColor.Items544"))),
                                                   ((resources.GetObject("cddColor.Items545"))),
                                                   ((resources.GetObject("cddColor.Items546"))),
                                                   ((resources.GetObject("cddColor.Items547"))),
                                                   ((resources.GetObject("cddColor.Items548"))),
                                                   ((resources.GetObject("cddColor.Items549"))),
                                                   ((resources.GetObject("cddColor.Items550"))),
                                                   ((resources.GetObject("cddColor.Items551"))),
                                                   ((resources.GetObject("cddColor.Items552"))),
                                                   ((resources.GetObject("cddColor.Items553"))),
                                                   ((resources.GetObject("cddColor.Items554"))),
                                                   ((resources.GetObject("cddColor.Items555"))),
                                                   ((resources.GetObject("cddColor.Items556"))),
                                                   ((resources.GetObject("cddColor.Items557"))),
                                                   ((resources.GetObject("cddColor.Items558"))),
                                                   ((resources.GetObject("cddColor.Items559"))),
                                                   ((resources.GetObject("cddColor.Items560"))),
                                                   ((resources.GetObject("cddColor.Items561"))),
                                                   ((resources.GetObject("cddColor.Items562"))),
                                                   ((resources.GetObject("cddColor.Items563"))),
                                                   ((resources.GetObject("cddColor.Items564"))),
                                                   ((resources.GetObject("cddColor.Items565"))),
                                                   ((resources.GetObject("cddColor.Items566"))),
                                                   ((resources.GetObject("cddColor.Items567"))),
                                                   ((resources.GetObject("cddColor.Items568"))),
                                                   ((resources.GetObject("cddColor.Items569"))),
                                                   ((resources.GetObject("cddColor.Items570"))),
                                                   ((resources.GetObject("cddColor.Items571"))),
                                                   ((resources.GetObject("cddColor.Items572"))),
                                                   ((resources.GetObject("cddColor.Items573"))),
                                                   ((resources.GetObject("cddColor.Items574"))),
                                                   ((resources.GetObject("cddColor.Items575"))),
                                                   ((resources.GetObject("cddColor.Items576"))),
                                                   ((resources.GetObject("cddColor.Items577"))),
                                                   ((resources.GetObject("cddColor.Items578"))),
                                                   ((resources.GetObject("cddColor.Items579"))),
                                                   ((resources.GetObject("cddColor.Items580"))),
                                                   ((resources.GetObject("cddColor.Items581"))),
                                                   ((resources.GetObject("cddColor.Items582"))),
                                                   ((resources.GetObject("cddColor.Items583"))),
                                                   ((resources.GetObject("cddColor.Items584"))),
                                                   ((resources.GetObject("cddColor.Items585"))),
                                                   ((resources.GetObject("cddColor.Items586"))),
                                                   ((resources.GetObject("cddColor.Items587"))),
                                                   ((resources.GetObject("cddColor.Items588"))),
                                                   ((resources.GetObject("cddColor.Items589"))),
                                                   ((resources.GetObject("cddColor.Items590"))),
                                                   ((resources.GetObject("cddColor.Items591"))),
                                                   ((resources.GetObject("cddColor.Items592"))),
                                                   ((resources.GetObject("cddColor.Items593"))),
                                                   ((resources.GetObject("cddColor.Items594"))),
                                                   ((resources.GetObject("cddColor.Items595"))),
                                                   ((resources.GetObject("cddColor.Items596"))),
                                                   ((resources.GetObject("cddColor.Items597"))),
                                                   ((resources.GetObject("cddColor.Items598"))),
                                                   ((resources.GetObject("cddColor.Items599"))),
                                                   ((resources.GetObject("cddColor.Items600"))),
                                                   ((resources.GetObject("cddColor.Items601"))),
                                                   ((resources.GetObject("cddColor.Items602"))),
                                                   ((resources.GetObject("cddColor.Items603"))),
                                                   ((resources.GetObject("cddColor.Items604"))),
                                                   ((resources.GetObject("cddColor.Items605"))),
                                                   ((resources.GetObject("cddColor.Items606"))),
                                                   ((resources.GetObject("cddColor.Items607"))),
                                                   ((resources.GetObject("cddColor.Items608"))),
                                                   ((resources.GetObject("cddColor.Items609"))),
                                                   ((resources.GetObject("cddColor.Items610"))),
                                                   ((resources.GetObject("cddColor.Items611"))),
                                                   ((resources.GetObject("cddColor.Items612"))),
                                                   ((resources.GetObject("cddColor.Items613"))),
                                                   ((resources.GetObject("cddColor.Items614"))),
                                                   ((resources.GetObject("cddColor.Items615"))),
                                                   ((resources.GetObject("cddColor.Items616"))),
                                                   ((resources.GetObject("cddColor.Items617"))),
                                                   ((resources.GetObject("cddColor.Items618"))),
                                                   ((resources.GetObject("cddColor.Items619"))),
                                                   ((resources.GetObject("cddColor.Items620"))),
                                                   ((resources.GetObject("cddColor.Items621"))),
                                                   ((resources.GetObject("cddColor.Items622"))),
                                                   ((resources.GetObject("cddColor.Items623"))),
                                                   ((resources.GetObject("cddColor.Items624"))),
                                                   ((resources.GetObject("cddColor.Items625"))),
                                                   ((resources.GetObject("cddColor.Items626"))),
                                                   ((resources.GetObject("cddColor.Items627"))),
                                                   ((resources.GetObject("cddColor.Items628"))),
                                                   ((resources.GetObject("cddColor.Items629"))),
                                                   ((resources.GetObject("cddColor.Items630"))),
                                                   ((resources.GetObject("cddColor.Items631"))),
                                                   ((resources.GetObject("cddColor.Items632"))),
                                                   ((resources.GetObject("cddColor.Items633"))),
                                                   ((resources.GetObject("cddColor.Items634"))),
                                                   ((resources.GetObject("cddColor.Items635"))),
                                                   ((resources.GetObject("cddColor.Items636"))),
                                                   ((resources.GetObject("cddColor.Items637"))),
                                                   ((resources.GetObject("cddColor.Items638"))),
                                                   ((resources.GetObject("cddColor.Items639"))),
                                                   ((resources.GetObject("cddColor.Items640"))),
                                                   ((resources.GetObject("cddColor.Items641"))),
                                                   ((resources.GetObject("cddColor.Items642"))),
                                                   ((resources.GetObject("cddColor.Items643"))),
                                                   ((resources.GetObject("cddColor.Items644"))),
                                                   ((resources.GetObject("cddColor.Items645"))),
                                                   ((resources.GetObject("cddColor.Items646"))),
                                                   ((resources.GetObject("cddColor.Items647"))),
                                                   ((resources.GetObject("cddColor.Items648"))),
                                                   ((resources.GetObject("cddColor.Items649"))),
                                                   ((resources.GetObject("cddColor.Items650"))),
                                                   ((resources.GetObject("cddColor.Items651"))),
                                                   ((resources.GetObject("cddColor.Items652"))),
                                                   ((resources.GetObject("cddColor.Items653"))),
                                                   ((resources.GetObject("cddColor.Items654"))),
                                                   ((resources.GetObject("cddColor.Items655"))),
                                                   ((resources.GetObject("cddColor.Items656"))),
                                                   ((resources.GetObject("cddColor.Items657"))),
                                                   ((resources.GetObject("cddColor.Items658"))),
                                                   ((resources.GetObject("cddColor.Items659"))),
                                                   ((resources.GetObject("cddColor.Items660"))),
                                                   ((resources.GetObject("cddColor.Items661"))),
                                                   ((resources.GetObject("cddColor.Items662"))),
                                                   ((resources.GetObject("cddColor.Items663"))),
                                                   ((resources.GetObject("cddColor.Items664"))),
                                                   ((resources.GetObject("cddColor.Items665"))),
                                                   ((resources.GetObject("cddColor.Items666"))),
                                                   ((resources.GetObject("cddColor.Items667"))),
                                                   ((resources.GetObject("cddColor.Items668"))),
                                                   ((resources.GetObject("cddColor.Items669"))),
                                                   ((resources.GetObject("cddColor.Items670"))),
                                                   ((resources.GetObject("cddColor.Items671"))),
                                                   ((resources.GetObject("cddColor.Items672"))),
                                                   ((resources.GetObject("cddColor.Items673"))),
                                                   ((resources.GetObject("cddColor.Items674"))),
                                                   ((resources.GetObject("cddColor.Items675"))),
                                                   ((resources.GetObject("cddColor.Items676"))),
                                                   ((resources.GetObject("cddColor.Items677"))),
                                                   ((resources.GetObject("cddColor.Items678"))),
                                                   ((resources.GetObject("cddColor.Items679"))),
                                                   ((resources.GetObject("cddColor.Items680"))),
                                                   ((resources.GetObject("cddColor.Items681"))),
                                                   ((resources.GetObject("cddColor.Items682"))),
                                                   ((resources.GetObject("cddColor.Items683"))),
                                                   ((resources.GetObject("cddColor.Items684"))),
                                                   ((resources.GetObject("cddColor.Items685"))),
                                                   ((resources.GetObject("cddColor.Items686"))),
                                                   ((resources.GetObject("cddColor.Items687"))),
                                                   ((resources.GetObject("cddColor.Items688"))),
                                                   ((resources.GetObject("cddColor.Items689"))),
                                                   ((resources.GetObject("cddColor.Items690"))),
                                                   ((resources.GetObject("cddColor.Items691"))),
                                                   ((resources.GetObject("cddColor.Items692"))),
                                                   ((resources.GetObject("cddColor.Items693"))),
                                                   ((resources.GetObject("cddColor.Items694"))),
                                                   ((resources.GetObject("cddColor.Items695"))),
                                                   ((resources.GetObject("cddColor.Items696")))});
            this.cddColor.Name = "cddColor";
            this.cddColor.Value = Color.Empty;
            //
            // cmdShowDialog
            //
            resources.ApplyResources(this.cmdShowDialog, "cmdShowDialog");
            this.cmdShowDialog.Name = "cmdShowDialog";
            this.cmdShowDialog.UseVisualStyleBackColor = true;
            this.cmdShowDialog.Click += this.cmdShowDialog_Click;
            //
            // ColorBox
            //
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmdShowDialog);
            this.Controls.Add(this.cddColor);
            this.Controls.Add(this.lblColor);
            this.Name = "ColorBox";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the selected color
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the selected color")]
        public Color Value
        {
            get { return cddColor.Value; }
            set { cddColor.Value = value; }
        }

        /// <summary>
        /// Gets or sets the text for the label portion
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the text for the label portion")]
        public string LabelText
        {
            get { return lblColor.Text; }
            set
            {
                lblColor.Text = value;
                Reset();
            }
        }

        /// <summary>
        /// Gets or set the font for the label portion of the component.
        /// </summary>
        [Category("Appearance"), Description("Gets or set the font for the label portion of the component.")]
        public new Font Font
        {
            get { return lblColor.Font; }
            set
            {
                lblColor.Font = value;
                Reset();
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        private void cmdShowDialog_Click(object sender, EventArgs e)
        {
            ColorDialog cdlg = new ColorDialog();
            if (cdlg.ShowDialog(ParentForm) != DialogResult.OK) return;
            foreach (object item in cddColor.Items)
            {
                if (item is KnownColor)
                {
                    KnownColor kn = (KnownColor)item;
                    if (Color.FromKnownColor(kn) == cdlg.Color)
                    {
                        cddColor.SelectedItem = kn;
                        return;
                    }
                }
            }
            if (cddColor.Items.Contains(cdlg.Color))
            {
                cddColor.SelectedItem = cdlg.Color;
                return;
            }
            else
            {
                cddColor.Items.Add(cdlg.Color);
                cddColor.SelectedIndex = cddColor.Items.Count - 1;
            }
        }

        /// <summary>
        /// Changes the starting location of the color drop down based on the current text.
        /// </summary>
        private void Reset()
        {
            cddColor.Left = lblColor.Width + 5;
            cddColor.Width = cmdShowDialog.Left - cddColor.Left - 10;
        }
    }
}
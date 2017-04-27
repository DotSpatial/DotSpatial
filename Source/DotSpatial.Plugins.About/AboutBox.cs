using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;

namespace DotSpatial.Plugins.About
{
    /// <summary>
    /// Generic, self-contained About Box dialog
    /// </summary>
    /// <remarks>
    /// Jeff Atwood
    /// http://www.codinghorror.com
    /// http://www.codinghorror.com/blog/2004/02/about-the-about-box.html
    /// http://www.codeproject.com/KB/vb/aboutbox.aspx
    /// converted to C# by Scott Ferguson
    /// http://www.forestmoon.com
    /// </remarks>
    public partial class AboutBox : Form
    {
        #region Fields

        private string _callingAssemblyName;
        private NameValueCollection _entryAssemblyAttribCollection;
        private string _entryAssemblyName;
        private string _executingAssemblyName;
        private bool _isPainted;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutBox"/> class.
        /// </summary>
        public AboutBox()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a single line of text to show in the copyright section of the about dialog.
        /// </summary>
        /// <remarks>
        /// defaults to "Copyright © %year%, %company%"
        /// %company% = Assembly: AssemblyCompany
        /// %year% = current 4-digit year
        /// </remarks>
        public string AppCopyright
        {
            get
            {
                return AppCopyrightLabel.Text;
            }

            set
            {
                if (value == string.Empty)
                {
                    AppCopyrightLabel.Visible = false;
                }
                else
                {
                    AppCopyrightLabel.Visible = true;
                    AppCopyrightLabel.Text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a single line of text to show in the description section of the about box dialog.
        /// </summary>
        /// <remarks>
        /// defaults to "%description%"
        /// %description% = Assembly: AssemblyDescription
        /// </remarks>
        public string AppDescription
        {
            get
            {
                return AppDescriptionLabel.Text;
            }

            set
            {
                if (value == string.Empty)
                {
                    AppDescriptionLabel.Visible = false;
                }
                else
                {
                    AppDescriptionLabel.Visible = true;
                    AppDescriptionLabel.Text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the "Details" (advanced assembly details) button is shown.
        /// </summary>
        public bool AppDetailsButton
        {
            get
            {
                return DetailsButton.Visible;
            }

            set
            {
                DetailsButton.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets the entry assembly for the current application domain.
        /// </summary>
        /// <remarks>
        /// This is usually read-only, but in some weird cases (Smart Client apps)
        /// you won't have an entry assembly, so you may want to set this manually.
        /// </remarks>
        public Assembly AppEntryAssembly { get; set; }

        /// <summary>
        /// Sets the default 32x32 application icon to appear in the upper left of the about dialog.
        /// </summary>
        /// <remarks>
        /// if you open this form using .ShowDialog(Owner), the icon can be derived from the owning form
        /// </remarks>
        public Icon AppImage
        {
            set
            {
                Icon = value;
                ImagePictureBox.Image = Icon.ToBitmap();

                ImagePictureBox.Visible = true;
                AppTitleLabel.Left = 58;
                AppDescriptionLabel.Left = 58;
            }
        }

        /// <summary>
        /// Gets or sets multiple lines of miscellaneous text to show in rich text box.
        /// </summary>
        /// <remarks>
        /// defaults to "%product% is %copyright%, %trademark%"
        /// %product% = Assembly: AssemblyProduct
        /// %copyright% = Assembly: AssemblyCopyright
        /// %trademark% = Assembly: AssemblyTrademark
        /// </remarks>
        public string AppMoreInfo
        {
            get
            {
                return MoreRichTextBox.Text;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    MoreRichTextBox.Visible = false;
                }
                else
                {
                    MoreRichTextBox.Visible = true;
                    MoreRichTextBox.Text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a single line of text to show in the application title section of the about box dialog.
        /// </summary>
        /// <remarks>
        /// defaults to "%title%"
        /// %title% = Assembly: AssemblyTitle
        /// </remarks>
        public string AppTitle
        {
            get
            {
                return AppTitleLabel.Text;
            }

            set
            {
                AppTitleLabel.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets a single line of text to show in the version section of the about dialog.
        /// </summary>
        /// <remarks>
        /// defaults to "Version %version%"
        /// %version% = Assembly: AssemblyVersion
        /// </remarks>
        public string AppVersion
        {
            get
            {
                return AppVersionLabel.Text;
            }

            set
            {
                if (value == string.Empty)
                {
                    AppVersionLabel.Visible = false;
                }
                else
                {
                    AppVersionLabel.Visible = true;
                    AppVersionLabel.Text = value;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Exception-safe retrieval of LastWriteTime for this assembly.
        /// </summary>
        /// <param name="a">The assembly to get the last write time from.</param>
        /// <returns>File.GetLastWriteTime, or DateTime.MaxValue if exception was encountered.</returns>
        private static DateTime AssemblyLastWriteTime(Assembly a)
        {
            try
            {
                // a.Location will throw NotSupportedException in a Dynamically loaded assembly.
                if (string.IsNullOrEmpty(a.Location))
                    return DateTime.MaxValue;

                return File.GetLastWriteTime(a.Location);
            }
            catch (Exception)
            {
                return DateTime.MaxValue;
            }
        }

        /// <summary>
        /// Matches assembly by Assembly.GetName.Name; returns nothing if no match
        /// </summary>
        /// <param name="assemblyName">Name of the assembly that should be returned.</param>
        /// <returns>Nothing if no match, otherwise the found assembly.</returns>
        private static Assembly MatchAssemblyByName(string assemblyName)
        {
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (a.GetName().Name == assemblyName)
                {
                    return a;
                }
            }

            return null;
        }

        /// <summary>
        /// Reads an HKLM Windows Registry key value.
        /// </summary>
        /// <param name="subkeyName">Name of the subkey to open.</param>
        /// <param name="value">Name of the value.</param>
        /// <returns>Empty string, if nothing was found, otherwise the value that was found.</returns>
        private static string RegistryHklmValue(string subkeyName, string value)
        {
            try
            {
                var rk = Registry.LocalMachine.OpenSubKey(subkeyName);
                if (rk == null) return string.Empty;
                return (string)rk.GetValue(value, string.Empty);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Things to do when form is loaded.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void AboutBoxLoad(object sender, EventArgs e)
        {
            // if the user didn't provide an assembly, try to guess which one is the entry assembly
            if (AppEntryAssembly == null)
            {
                AppEntryAssembly = Assembly.GetEntryAssembly();
            }

            if (AppEntryAssembly == null)
            {
                AppEntryAssembly = Assembly.GetExecutingAssembly();
            }

            _executingAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            _callingAssemblyName = Assembly.GetCallingAssembly().GetName().Name;
            try
            {
                // for web hosted apps, GetEntryAssembly = nothing
                _entryAssemblyName = Assembly.GetEntryAssembly().GetName().Name;
            }
            catch (Exception)
            {
            }

            TabPanelDetails.Visible = false;
            if (!MoreRichTextBox.Visible)
            {
                Height = Height - MoreRichTextBox.Height;
            }
        }

        /// <summary>
        /// Things to do when form is first painted.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void AboutBoxPaint(object sender, PaintEventArgs e)
        {
            if (!_isPainted)
            {
                _isPainted = true;
                Application.DoEvents();
                Cursor.Current = Cursors.WaitCursor;
                PopulateLabels();
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// Returns string name / string value pair of all attributes.
        /// for specified assembly
        /// </summary>
        /// <remarks>
        /// note that Assembly* values are pulled from AssemblyInfo file in project folder
        /// Trademark       = AssemblyTrademark string
        /// Debuggable      = true
        /// GUID            = 7FDF68D5-8C6F-44C9-B391-117B5AFB5467
        /// CLSCompliant    = true
        /// Product         = AssemblyProduct string
        /// Copyright       = AssemblyCopyright string
        /// Company         = AssemblyCompany string
        /// Description     = AssemblyDescription string
        /// Title           = AssemblyTitle string
        /// </remarks>
        /// <param name="a">Assembly to get the attributes from.</param>
        /// <returns>A string name / string value pair of all attributes.</returns>
        private NameValueCollection AssemblyAttribs(Assembly a)
        {
            NameValueCollection nvc = new NameValueCollection();
            Regex r = new Regex(@"(\.Assembly|\.)(?<Name>[^.]*)Attribute$", RegexOptions.IgnoreCase);

            foreach (object attrib in a.GetCustomAttributes(false))
            {
                try
                {
                    var typeName = attrib.GetType().ToString();
                    var name = r.Match(typeName).Groups["Name"].ToString();
                    string value;
                    switch (typeName)
                    {
                        case "System.CLSCompliantAttribute":
                            value = ((CLSCompliantAttribute)attrib).IsCompliant.ToString();
                            break;
                        case "System.Diagnostics.DebuggableAttribute":
                            value = ((DebuggableAttribute)attrib).IsJITTrackingEnabled.ToString();
                            break;
                        case "System.Reflection.AssemblyCompanyAttribute":
                            value = ((AssemblyCompanyAttribute)attrib).Company;
                            break;
                        case "System.Reflection.AssemblyConfigurationAttribute":
                            value = ((AssemblyConfigurationAttribute)attrib).Configuration;
                            break;
                        case "System.Reflection.AssemblyCopyrightAttribute":
                            value = ((AssemblyCopyrightAttribute)attrib).Copyright;
                            break;
                        case "System.Reflection.AssemblyDefaultAliasAttribute":
                            value = ((AssemblyDefaultAliasAttribute)attrib).DefaultAlias;
                            break;
                        case "System.Reflection.AssemblyDelaySignAttribute":
                            value = ((AssemblyDelaySignAttribute)attrib).DelaySign.ToString();
                            break;
                        case "System.Reflection.AssemblyDescriptionAttribute":
                            value = ((AssemblyDescriptionAttribute)attrib).Description;
                            break;
                        case "System.Reflection.AssemblyInformationalVersionAttribute":
                            value = ((AssemblyInformationalVersionAttribute)attrib).InformationalVersion;
                            break;
                        case "System.Reflection.AssemblyKeyFileAttribute":
                            value = ((AssemblyKeyFileAttribute)attrib).KeyFile;
                            break;
                        case "System.Reflection.AssemblyProductAttribute":
                            value = ((AssemblyProductAttribute)attrib).Product;
                            break;
                        case "System.Reflection.AssemblyTrademarkAttribute":
                            value = ((AssemblyTrademarkAttribute)attrib).Trademark;
                            break;
                        case "System.Reflection.AssemblyTitleAttribute":
                            value = ((AssemblyTitleAttribute)attrib).Title;
                            break;
                        case "System.Resources.NeutralResourcesLanguageAttribute":
                            value = ((NeutralResourcesLanguageAttribute)attrib).CultureName;
                            break;
                        case "System.Resources.SatelliteContractVersionAttribute":
                            value = ((SatelliteContractVersionAttribute)attrib).Version;
                            break;
                        case "System.Runtime.InteropServices.ComCompatibleVersionAttribute":
                            ComCompatibleVersionAttribute x = (ComCompatibleVersionAttribute)attrib;
                            value = x.MajorVersion + "." + x.MinorVersion + "." + x.RevisionNumber + "." + x.BuildNumber;
                            break;
                        case "System.Runtime.InteropServices.ComVisibleAttribute":
                            value = ((ComVisibleAttribute)attrib).Value.ToString();
                            break;
                        case "System.Runtime.InteropServices.GuidAttribute":
                            value = ((GuidAttribute)attrib).Value;
                            break;
                        case "System.Runtime.InteropServices.TypeLibVersionAttribute":
                            TypeLibVersionAttribute x1 = (TypeLibVersionAttribute)attrib;
                            value = x1.MajorVersion + "." + x1.MinorVersion;
                            break;
                        case "System.Security.AllowPartiallyTrustedCallersAttribute":
                            value = "(Present)";
                            break;
                        default:
                            value = typeName;
                            break;
                    }

                    if (nvc[name] == null)
                    {
                        nvc.Add(name, value);
                    }
                }
                catch (FormatException)
                {
                    // Not sure we can do anything about FormatException
                }
                catch (NullReferenceException)
                {
                }
            }

            // add some extra values that are not in the AssemblyInfo, but nice to have
            // codebase
            try
            {
                nvc.Add("CodeBase", a.CodeBase.Replace("file:///", string.Empty));
            }
            catch (NotSupportedException)
            {
                nvc.Add("CodeBase", "(not supported)");
            }

            // build date
            DateTime dt = AssemblyBuildDate(a, false);
            nvc.Add("BuildDate", dt == DateTime.MaxValue ? "(unknown)" : dt.ToString("yyyy-MM-dd hh:mm tt"));

            // location
            try
            {
                nvc.Add("Location", a.Location);
            }
            catch (NotSupportedException)
            {
                nvc.Add("Location", "(not supported)");
            }

            // version
            try
            {
                if (a.GetName().Version.Major == 0 && a.GetName().Version.Minor == 0)
                {
                    nvc.Add("Version", "(unknown)");
                }
                else
                {
                    nvc.Add("Version", a.GetName().Version.ToString());
                }
            }
            catch (Exception)
            {
                nvc.Add("Version", "(unknown)");
            }

            nvc.Add("FullName", a.FullName);

            return nvc;
        }

        /// <summary>
        /// returns DateTime this Assembly was last built. Will attempt to calculate from build number, if possible.
        /// If not, the actual LastWriteTime on the assembly file will be returned.
        /// </summary>
        /// <param name="a">Assembly to get build date for</param>
        /// <param name="forceFileDate">Don't attempt to use the build number to calculate the date</param>
        /// <returns>DateTime this assembly was last built</returns>
        private DateTime AssemblyBuildDate(Assembly a, bool forceFileDate)
        {
            Version assemblyVersion = a.GetName().Version;
            DateTime dt;

            if (forceFileDate)
            {
                dt = AssemblyLastWriteTime(a);
            }
            else
            {
                dt = DateTime.Parse("01/01/2000").AddDays(assemblyVersion.Build).AddSeconds(assemblyVersion.Revision * 2);
                if (TimeZone.IsDaylightSavingTime(dt, TimeZone.CurrentTimeZone.GetDaylightChanges(dt.Year)))
                {
                    dt = dt.AddHours(1);
                }

                if (dt > DateTime.Now || assemblyVersion.Build < 730 || assemblyVersion.Revision == 0)
                {
                    dt = AssemblyLastWriteTime(a);
                }
            }

            return dt;
        }

        /// <summary>
        /// Sort the assembly list by column.
        /// </summary>
        /// <param name="sender">The sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void AssemblyInfoListViewColumnClick(object sender, ColumnClickEventArgs e)
        {
            int intTargetCol = e.Column + 1;

            if (AssemblyInfoListView.Tag != null)
            {
                if (Math.Abs(Convert.ToInt32(AssemblyInfoListView.Tag)) == intTargetCol)
                {
                    intTargetCol = -Convert.ToInt32(AssemblyInfoListView.Tag);
                }
            }

            AssemblyInfoListView.Tag = intTargetCol;
            AssemblyInfoListView.ListViewItemSorter = new ListViewItemComparer(intTargetCol, true);
        }

        /// <summary>
        /// If an assembly is double-clicked, go to the detail page for that assembly.
        /// </summary>
        /// <param name="sender">The sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void AssemblyInfoListViewDoubleClick(object sender, EventArgs e)
        {
            if (AssemblyInfoListView.SelectedItems.Count > 0)
            {
                var strAssemblyName = Convert.ToString(AssemblyInfoListView.SelectedItems[0].Tag);
                AssemblyNamesComboBox.SelectedIndex = AssemblyNamesComboBox.FindStringExact(strAssemblyName);
                TabPanelDetails.SelectedTab = TabPageAssemblyDetails;
            }
        }

        /// <summary>
        /// If a new assembly is selected from the combo box, show details for that assembly.
        /// </summary>
        /// <param name="sender">The sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void AssemblyNamesComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            string strAssemblyName = Convert.ToString(AssemblyNamesComboBox.SelectedItem);
            PopulateAssemblyDetails(MatchAssemblyByName(strAssemblyName), AssemblyDetailsListView);
        }

        /// <summary>
        /// expand about dialog to show additional advanced details
        /// </summary>
        /// <param name="sender">The sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void DetailsButtonClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DetailsButton.Visible = false;
            SuspendLayout();
            MaximizeBox = true;
            FormBorderStyle = FormBorderStyle.Sizable;
            SizeGripStyle = SizeGripStyle.Show;
            Size = new Size(580, Size.Height + 200);
            MoreRichTextBox.Visible = false;
            TabPanelDetails.Visible = true;
            SysInfoButton.Visible = true;
            PopulateAssemblies();
            PopulateAppInfo();
            CenterToParent();
            ResumeLayout();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Retrieves a cached value from the entry assembly attribute lookup collection.
        /// </summary>
        /// <param name="strName">Name of value that should be returned</param>
        /// <returns>Value belonging to the given name.</returns>
        private string EntryAssemblyAttrib(string strName)
        {
            if (_entryAssemblyAttribCollection[strName] == null)
            {
                return "<Assembly: Assembly" + strName + "(\"\")>";
            }

            return _entryAssemblyAttribCollection[strName];
        }

        /// <summary>
        /// Launch any http://or mailto: links clicked in the body of the rich text box.
        /// </summary>
        /// <param name="sender">The sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void MoreRichTextBoxLinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                Process.Start(e.LinkText);
            }
            catch (Exception)
            {
            }
        }

        private void OkButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Populate a listview with the specified key and value strings.
        /// </summary>
        /// <param name="lvw">Listview, that gets populated.</param>
        /// <param name="key">Key to add.</param>
        /// <param name="value">Value to add.</param>
        private void Populate(ListView lvw, string key, string value)
        {
            if (value == string.Empty)
                return;
            ListViewItem lvi = new ListViewItem { Text = key };
            lvi.SubItems.Add(value);
            lvw.Items.Add(lvi);
        }

        /// <summary>
        /// Populates the Application Information listview.
        /// </summary>
        private void PopulateAppInfo()
        {
            AppDomain d = AppDomain.CurrentDomain;
            Populate(AppInfoListView, "Application Name", d.SetupInformation.ApplicationName);
            Populate(AppInfoListView, "Application Base", d.SetupInformation.ApplicationBase);
            Populate(AppInfoListView, "Cache Path", d.SetupInformation.CachePath);
            Populate(AppInfoListView, "Configuration File", d.SetupInformation.ConfigurationFile);
            Populate(AppInfoListView, "Dynamic Base", d.SetupInformation.DynamicBase);
            Populate(AppInfoListView, "Friendly Name", d.FriendlyName);
            Populate(AppInfoListView, "License File", d.SetupInformation.LicenseFile);
            Populate(AppInfoListView, "private Bin Path", d.SetupInformation.PrivateBinPath);
            Populate(AppInfoListView, "Shadow Copy Directories", d.SetupInformation.ShadowCopyDirectories);
            Populate(AppInfoListView, " ", " ");
            Populate(AppInfoListView, "Entry Assembly", _entryAssemblyName);
            Populate(AppInfoListView, "Executing Assembly", _executingAssemblyName);
            Populate(AppInfoListView, "Calling Assembly", _callingAssemblyName);
        }

        /// <summary>
        /// Populates the Assembly Information listview with ALL assemblies.
        /// </summary>
        private void PopulateAssemblies()
        {
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                PopulateAssemblySummary(a);
            }

            AssemblyNamesComboBox.SelectedIndex = AssemblyNamesComboBox.FindStringExact(_entryAssemblyName);
        }

        /// <summary>
        /// Populate details for a single assembly.
        /// </summary>
        /// <param name="a">Assembly used for population.</param>
        /// <param name="lvw">Listview, that gets populated.</param>
        private void PopulateAssemblyDetails(Assembly a, ListView lvw)
        {
            lvw.Items.Clear();

            // this assembly property is only available in framework versions 1.1+
            Populate(lvw, "Image Runtime Version", a.ImageRuntimeVersion);
            Populate(lvw, "Loaded from GAC", a.GlobalAssemblyCache.ToString());

            NameValueCollection nvc = AssemblyAttribs(a);
            foreach (string strKey in nvc)
            {
                Populate(lvw, strKey, nvc[strKey]);
            }
        }

        /// <summary>
        /// Populate Assembly Information listview with summary view for a specific assembly.
        /// </summary>
        /// <param name="a">Assembly used for population.</param>
        private void PopulateAssemblySummary(Assembly a)
        {
            NameValueCollection nvc = AssemblyAttribs(a);

            string strAssemblyName = a.GetName().Name;

            ListViewItem lvi = new ListViewItem
            {
                Text = strAssemblyName,
                Tag = strAssemblyName
            };
            if (strAssemblyName == _callingAssemblyName)
            {
                lvi.Text += @" (calling)";
            }

            if (strAssemblyName == _executingAssemblyName)
            {
                lvi.Text += @" (executing)";
            }

            if (strAssemblyName == _entryAssemblyName)
            {
                lvi.Text += @" (entry)";
            }

            lvi.SubItems.Add(nvc["version"]);
            lvi.SubItems.Add(nvc["builddate"]);
            lvi.SubItems.Add(nvc["codebase"]);

            AssemblyInfoListView.Items.Add(lvi);
            AssemblyNamesComboBox.Items.Add(strAssemblyName);
        }

        /// <summary>
        /// Populate all the form labels with tokenized text.
        /// </summary>
        private void PopulateLabels()
        {
            // get entry assembly attribs
            _entryAssemblyAttribCollection = AssemblyAttribs(AppEntryAssembly);

            // set icon from parent, if present
            if (Owner != null)
            {
                Icon = Owner.Icon;
                ImagePictureBox.Image = Icon.ToBitmap();
            }
            else if (ImagePictureBox.Image == null)
            {
                ImagePictureBox.Visible = false;
                AppTitleLabel.Left = AppCopyrightLabel.Left;
                AppDescriptionLabel.Left = AppCopyrightLabel.Left;
            }

            // replace all labels and window title
            Text = ReplaceTokens(Text);
            AppTitleLabel.Text = ReplaceTokens(AppTitleLabel.Text);
            if (AppDescriptionLabel.Visible)
            {
                AppDescriptionLabel.Text = ReplaceTokens(AppDescriptionLabel.Text);
            }

            if (AppCopyrightLabel.Visible)
            {
                AppCopyrightLabel.Text = ReplaceTokens(AppCopyrightLabel.Text);
            }

            if (AppVersionLabel.Visible)
            {
                AppVersionLabel.Text = ReplaceTokens(AppVersionLabel.Text);
            }

            if (AppDateLabel.Visible)
            {
                AppDateLabel.Text = ReplaceTokens(AppDateLabel.Text);
            }

            if (MoreRichTextBox.Visible)
            {
                MoreRichTextBox.Text = ReplaceTokens(MoreRichTextBox.Text);
            }
        }

        /// <summary>
        /// Perform assemblyinfo to string replacements on labels.
        /// </summary>
        /// <param name="s">String, in which the tokens should be replaced.</param>
        /// <returns>The resulting string.</returns>
        private string ReplaceTokens(string s)
        {
            s = s.Replace("%title%", EntryAssemblyAttrib("title"));
            s = s.Replace("%copyright%", EntryAssemblyAttrib("copyright"));
            s = s.Replace("%description%", EntryAssemblyAttrib("description"));
            s = s.Replace("%company%", EntryAssemblyAttrib("company"));
            s = s.Replace("%product%", EntryAssemblyAttrib("product"));
            s = s.Replace("%trademark%", EntryAssemblyAttrib("trademark"));
            s = s.Replace("%year%", DateTime.Now.Year.ToString());
            s = s.Replace("%version%", EntryAssemblyAttrib("version"));
            s = s.Replace("%builddate%", EntryAssemblyAttrib("builddate"));
            return s;
        }

        /// <summary>
        /// Launch the MSInfo "system information" application (works on XP, 2003, and Vista).
        /// </summary>
        private void ShowSysInfo()
        {
            var strSysInfoPath = RegistryHklmValue(@"SOFTWARE\Microsoft\Shared Tools Location", "MSINFO");
            if (strSysInfoPath == string.Empty)
            {
                strSysInfoPath = RegistryHklmValue(@"SOFTWARE\Microsoft\Shared Tools\MSINFO", "PATH");
            }

            if (strSysInfoPath == string.Empty)
            {
                MessageBox.Show("System Information is unavailable at this time." + Environment.NewLine + Environment.NewLine + "(couldn't find path for Microsoft System Information Tool in the registry.)", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Process.Start(strSysInfoPath);
            }
            catch (Exception)
            {
                MessageBox.Show("System Information is unavailable at this time." + Environment.NewLine + Environment.NewLine + "(couldn't launch '" + strSysInfoPath + "')", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        /// <summary>
        /// For detailed system info, launch the external Microsoft system info app.
        /// </summary>
        /// <param name="sender">The sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void SysInfoButtonClick(object sender, EventArgs e)
        {
            ShowSysInfo();
        }

        /// <summary>
        /// Things to do when the selected tab is changed.
        /// </summary>
        /// <param name="sender">The sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void TabPanelDetailsSelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabPanelDetails.SelectedTab == TabPageAssemblyDetails)
                AssemblyNamesComboBox.Focus();
        }

        #endregion

        #region Classes

        private class ListViewItemComparer : IComparer
        {
            #region Fields

            private readonly int _intCol;

            private readonly bool _isAscending = true;

            #endregion

            #region  Constructors

            public ListViewItemComparer()
            {
                _intCol = 0;
                _isAscending = true;
            }

            public ListViewItemComparer(int column, bool ascending)
            {
                _isAscending = column >= 0 && @ascending;

                _intCol = Math.Abs(column) - 1;
            }

            #endregion

            #region Methods

            public int Compare(object x, object y)
            {
                int intResult = string.Compare(((ListViewItem)x).SubItems[_intCol].Text, ((ListViewItem)y).SubItems[_intCol].Text);

                return _isAscending ? intResult : -intResult;
            }

            #endregion
        }

        #endregion
    }
}
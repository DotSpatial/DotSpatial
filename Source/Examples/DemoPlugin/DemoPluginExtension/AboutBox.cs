// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace DemoPlugin.DemoPluginExtension
{
    internal partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            Text = string.Format("About {0}", AssemblyTitle);
            labelProductName.Text = AssemblyProduct;
            labelVersion.Text = string.Format("Version {0}", AssemblyVersion);
            labelCopyright.Text = AssemblyCopyright;
            labelCompanyName.Text = AssemblyCompany;
            textBoxDescription.Text = AssemblyDescription;
        }

        #region Assembly Attribute Accessors

        public static string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }

                return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            }
        }

        public static string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public static string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }

                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public static string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }

                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public static string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }

                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public static string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }

                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        #endregion Assembly Attribute Accessors
    }
}
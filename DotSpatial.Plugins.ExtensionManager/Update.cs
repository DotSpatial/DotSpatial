using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuGet;
using System.Windows.Forms;
using System.Net;
using DotSpatial.Controls;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class Update
    {
        private GetPackage getpack;
        public Update(Packages package, ListViewHelper adder,AppManager Appmanager)
        {
            this.packages = package;
            this.Add = adder;
            this.App = Appmanager;
        }
        private readonly Packages packages;
        private readonly ListViewHelper Add;
        private AppManager App;
       
        public void RefreshUpdate(ListView listview,TabPage tab)
        {
            getpack = new GetPackage(packages);
            IEnumerable<IPackage> localPackages = getpack.GetPackagesFromExtensions(App.Extensions);
            if (localPackages.Count() > 0)
            {
                IEnumerable<IPackage> list = null;
                try
                { 
                    if (listview.InvokeRequired)
                    {
                        listview.Invoke((Action)(() =>
                            {
                        listview.Items.Add("Checking for updates...");
                        list = packages.Repo.GetUpdates(localPackages, false, false);
                            }));
                    }
                    else
                    {
                        
                        listview.Items.Add("Checking for updates...");
                        list = packages.Repo.GetUpdates(localPackages, false, false);
                    }
                 }
               
                catch (WebException)
                {
                    listview.Clear();
                    listview.Items.Add("Updates could not be retrieved for the selected feed.");
                    listview.Items.Add("Try again later or change the feed.");
                }
                if (listview.InvokeRequired)
                {
                    listview.Invoke((Action)(() =>
                    {
                        // copied code.
                        listview.Clear();
                        int Count = list.Count();
                        tab.Text = "Updates (" + Count + ")";
                        Add.AddPackages(list, listview, 0);
                        if (listview.Items.Count == 0)
                        {
                            listview.Items.Add("No updates available for the selected feed.");
                        }
                    }));
                }
                else
                {
                    // copied code.
                    listview.Clear();
                    Add.AddPackages(list, listview, 0);
                    if (listview.Items.Count == 0)
                    {
                        listview.Items.Add("No updates available for the selected feed.");
                    }
                }
            }
            else
            {
                listview.Invoke((Action)(() =>
                {
                    listview.Clear();
                    listview.Items.Add("No packages are installed.");
                }));
            }
        }
    }
}

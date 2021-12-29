// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Plugins.ExtensionManager.Properties;
using NuGet;

namespace DotSpatial.Plugins.ExtensionManager
{
    /// <summary>
    /// Paging.
    /// </summary>
    internal class Paging
    {
        #region Fields

        /// <summary>
        /// The page size.
        /// </summary>
        public const int PageSize = 9;

        private const string DotSpatialPluginTag = "DotSpatial.Plugin";
        private readonly ListViewHelper _add;
        private readonly string _appName;

        private readonly List<Button> _listOfButtons = new List<Button>();
        private readonly Packages _packages;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Paging"/> class.
        /// </summary>
        /// <param name="packageHelper">Contains all the packages.</param>
        /// <param name="adder">Helper used for adding the packages to the listview.</param>
        public Paging(Packages packageHelper, ListViewHelper adder)
        {
            _packages = packageHelper;
            _add = adder;

            // find name of app
            string name = Assembly.GetEntryAssembly().GetName().Name;
            int i;
            for (i = 0; i < name.Length; i++)
            {
                if (!char.IsLetter(name[i]))
                    break;
            }

            _appName = name.Substring(0, i);
        }

        #endregion

        #region Events

        /// <summary>
        /// The page changed event.
        /// </summary>
        public event EventHandler<PageSelectedEventArgs> PageChanged;

        #endregion

        #region Methods

        /// <summary>
        /// Adds the buttons to the given tab page.
        /// </summary>
        /// <param name="tab">Tab page to add the buttons to.</param>
        public void AddButtons(TabPage tab)
        {
            foreach (var button in _listOfButtons)
            {
                tab.Controls.Add(button);
            }
        }

        /// <summary>
        /// Called if the page change button is clicked.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        public void ButtonClick(object sender, EventArgs e)
        {
            if (PageChanged != null)
            {
                Button button = sender as Button;
                if (button == null) return;

                int page = Convert.ToInt32(button.Text);

                var eventArgs = new PageSelectedEventArgs
                                    {
                                        SelectedPage = page
                                    };

                PageChanged.Invoke(this, eventArgs);
            }
        }

        /// <summary>
        /// Creates the buttons.
        /// </summary>
        /// <param name="packageCount">Number of packages.</param>
        public void CreateButtons(int packageCount)
        {
            int buttonsToShow = HowManyPagesAreNeeded(packageCount);

            // hack: we only show the first 5 pages.
            buttonsToShow = Math.Min(5, buttonsToShow);

            for (int i = 1; i <= buttonsToShow; i++)
            {
                Button button = new Button
                                    {
                                        Text = i.ToString(),
                                        Location = new Point(50 * i, 510),
                                        Size = new Size(41, 23)
                                    };
                _listOfButtons.Add(button);
                button.Click += ButtonClick;
            }
        }

        /// <summary>
        /// Displays the packagese.
        /// </summary>
        /// <param name="listview">Listview  for status messages.</param>
        /// <param name="pagenumber">Number of the page for which the packages are loaded.</param>
        /// <param name="tab">The tab page.</param>
        /// <param name="app">The AppManager.</param>
        public void DisplayPackages(ListView listview, int pagenumber, TabPage tab, AppManager app)
        {
            ResetButtons(tab);
            listview.Items.Clear();
            listview.Items.Add(Resources.Loading);

            Task<PackageList> task = GetPackages(pagenumber);
            task.ContinueWith(
                t =>
                {
                    listview.Items.Clear();
                    if (t.Result == null)
                    {
                        listview.Items.Add(Resources.NoPackagesRetrieved);
                        listview.Items.Add(Resources.TryAgainLaterSelectAnotherFeed);
                    }
                    else
                    {
                        var packs = from pack in t.Result.Packages where app.GetExtension(pack.Id) == null select pack;
                        var localPacks = _packages.Manager.LocalRepository.GetPackages();
                        packs = from pack in packs where !localPacks.Any(p => p.Id == pack.Id) select pack;

                        _add.AddPackages(packs.ToArray(), listview, pagenumber);
                        CreateButtons(t.Result.TotalPackageCount);
                        AddButtons(tab);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Clears the buttons of the given tab page.
        /// </summary>
        /// <param name="tab">Tab page whose buttons get cleared.</param>
        public void ResetButtons(TabPage tab)
        {
            foreach (var button in _listOfButtons)
            {
                tab.Controls.Remove(button);
            }

            _listOfButtons.Clear();
        }

        private bool AppDependencyCheck(IPackage pack)
        {
            bool result = true;
            var programVersion = SemanticVersion.Parse(Assembly.GetEntryAssembly().GetName().Version.ToString());

            foreach (var dependency in pack.Dependencies)
            {
                if (dependency.Id.ToLowerInvariant().Contains("sampleprojects"))
                {
                    result = true;
                }
                else if (!dependency.Id.Contains("Plugins"))
                {
                    if (dependency.Id == _appName)
                    {
                        if (dependency.VersionSpec.IsMaxInclusive)
                        {
                            if (programVersion > dependency.VersionSpec.MaxVersion)
                                result = false;
                        }

                        if (result && dependency.VersionSpec.IsMinInclusive)
                        {
                            if (programVersion < dependency.VersionSpec.MinVersion)
                                result = false;
                        }
                    }
                    else
                    {
                        result = false;
                    }

                    break;
                }
            }

            return result;
        }

        private Task<PackageList> GetPackages(int pagenumber)
        {
            var task = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var result = from item in _packages.Repo.GetPackages() where item.Tags != null && item.Tags.Contains(DotSpatialPluginTag) select item;
                        result = result.OrderBy(item => item.Id).ThenByDescending(item => item.Version);

                        string id = string.Empty;
                        List<IPackage> onlinePacks = new List<IPackage>();
                        foreach (var item in result)
                        {
                            if (id != item.Id && AppDependencyCheck(item))
                            {
                                onlinePacks.Add(item);
                                id = item.Id;
                            }
                        }

                        if (onlinePacks.Count == 0)
                            throw new InvalidOperationException();

                        var info = new PackageList
                                       {
                                           // Toggle comments here to reenable paging.
                                           // info.TotalPackageCount = result.Count();
                                           // info.packages = result.Skip(pagenumber * Paging.PageSize).Take(Paging.PageSize).ToArray();
                                           Packages = onlinePacks.ToArray()
                                       };

                        return info;
                    }
                    catch (InvalidOperationException)
                    {
                        // This usually means the url was bad.
                        return null;
                    }
                });
            return task;
        }

        private int HowManyPagesAreNeeded(int itemsToDisplay)
        {
            return (int)Math.Ceiling(itemsToDisplay / (double)PageSize);
        }

        #endregion

        #region Classes

        private class PackageList
        {
            #region Properties

            public IPackage[] Packages { get; set; }

            public int TotalPackageCount { get; set; }

            #endregion
        }

        #endregion
    }
}
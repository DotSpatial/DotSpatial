using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotSpatial.Controls;
using NuGet;

namespace DotSpatial.Plugins.ExtensionManager
{
    internal class Paging
    {
        public event EventHandler<PageSelectedEventArgs> PageChanged;
        private readonly ListViewHelper add;
        private readonly Packages packages;
        private const string DotSpatialPluginTag = "DotSpatial.Plugin";
        private string AppName;

        public const int PageSize = 9;

        private List<Button> listOfButtons = new List<Button>();
        public Paging(Packages packageHelper, ListViewHelper adder)
        {
            this.packages = packageHelper;
            this.add = adder;

            //find name of app
            string name = Assembly.GetEntryAssembly().GetName().Name;
            int i;
            for (i = 0; i < name.Length; i++)
            {
                if (!Char.IsLetter(name[i]))
                    break;
            }
            AppName = name.Substring(0, i);
        }

        public void CreateButtons(int packageCount)
        {
            int buttonsToShow = HowManyPagesAreNeeded(packageCount);
            // hack: we only show the first 5 pages.
            buttonsToShow = Math.Min(5, buttonsToShow);

            for (int i = 1; i <= buttonsToShow; i++)
            {
                Button button = new Button();
                button.Text = i.ToString();
                button.Location = new Point(50 * i, 510);
                button.Size = new Size(41, 23);
                listOfButtons.Add(button);
                button.Click += new EventHandler(this.button_Click);
            }
        }

        private int HowManyPagesAreNeeded(int itemsToDisplay)
        {
            return (int)Math.Ceiling(itemsToDisplay / (double)PageSize);
        }

        public void AddButtons(TabPage tab)
        {
                foreach (var button in listOfButtons)
                {
                    tab.Controls.Add(button);
                }   
        }

        public void button_Click(object sender, EventArgs e)
        {
            if (PageChanged != null)
            {
                Button button = sender as Button;
                int page = Convert.ToInt32(button.Text);

                var eventArgs = new PageSelectedEventArgs();
                eventArgs.SelectedPage = page;

                if (PageChanged != null)
                { PageChanged(this, eventArgs); }
            }
        }

        public void ResetButtons(TabPage tab)
        {
            foreach (var button in listOfButtons)
            {
                tab.Controls.Remove(button);
            }
            listOfButtons.Clear();
        }

        private class PackageList
        {
            public IPackage[] packages { get; set; }

            public int TotalPackageCount { get; set; }
        }

        public void DisplayPackages(ListView listview, int pagenumber, TabPage tab, AppManager App)
        {
            ResetButtons(tab);
            listview.Items.Clear();
            listview.Items.Add("Loading...");

            Task<PackageList> task = GetPackages(pagenumber);
            task.ContinueWith(t =>
            {
                listview.Items.Clear();
                if (t.Result == null)
                {
                    listview.Items.Add("No packages could be retrieved for the selected feed.");
                    listview.Items.Add("Try again later or Select another feed.");
                }
                else
                {
                    var packs = from pack in t.Result.packages
                                        where App.GetExtension(pack.Id) == null
                                        select pack;
                    var localPacks = packages.Manager.LocalRepository.GetPackages();
                    packs = from pack in packs where !localPacks.Where(p => p.Id == pack.Id).Any() select pack;

                    add.AddPackages(packs.ToArray(), listview, pagenumber);
                    CreateButtons(t.Result.TotalPackageCount);
                    AddButtons(tab);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private Task<PackageList> GetPackages(int pagenumber)
        {
            var task = Task.Factory.StartNew(() =>
            {
                try
                {
                    var result = from item in packages.Repo.GetPackages()
                                 where item.Tags != null && item.Tags.Contains(DotSpatialPluginTag)
                                 select item;
                    result = result.OrderBy(item => item.Id)
                                .ThenByDescending(item => item.Version);

                    String id = "";
                    List<IPackage> onlinePacks = new List<IPackage>();
                    foreach (var item in result)
                    {
                        if (id != item.Id && AppDependencyCheck(item))
                        {
                            onlinePacks.Add(item);
                            id = item.Id;
                        }
                    }
                    if(onlinePacks.Count() == 0)
                        throw new InvalidOperationException();

                    var info = new PackageList();
                    //info.TotalPackageCount = result.Count();
                    //info.packages = result.Skip(pagenumber * Paging.PageSize).Take(Paging.PageSize).ToArray();
                    info.packages = onlinePacks.ToArray(); //Toggle comments here to reenable paging.

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

        private bool AppDependencyCheck(IPackage pack)
        {
            bool result = true;
            var programVersion = SemanticVersion.Parse(Assembly.GetEntryAssembly().GetName().Version.ToString());

            foreach(var dependency in pack.Dependencies)
            {
                if (dependency.Id.ToLowerInvariant().Contains("sampleprojects"))
                    result = true;
                else if (!dependency.Id.Contains("Plugins"))
                {
                    if (dependency.Id == AppName)
                    {
                        if (dependency.VersionSpec.IsMaxInclusive)
                        {
                            if (programVersion > dependency.VersionSpec.MaxVersion)
                                result = false;
                        }
                        if (result && dependency.VersionSpec.IsMinInclusive)
                        {
                            if(programVersion < dependency.VersionSpec.MinVersion)
                                result = false;
                        }
                    }
                    else
                        result = false;

                    break;
                }
            }
            return result;
        }
    }
}
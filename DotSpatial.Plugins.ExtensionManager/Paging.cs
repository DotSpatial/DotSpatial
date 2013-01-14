using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NuGet;
using System.Drawing;
using System.Threading.Tasks;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class Paging
    {
        public event EventHandler<PageSelectedEventArgs> PageChanged;
        private readonly ListViewHelper add;
        private readonly Packages packages;
        private const string HideReleaseFromEndUser = "HideReleaseFromEndUser";

        public const int PageSize = 9;

        private List<Button> listOfButtons = new List<Button>();
        public Paging(Packages packageHelper, ListViewHelper adder)
        {
            this.packages = packageHelper;
            this.add = adder;
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
        public void DisplayPackages(ListView listview, int pagenumber, TabPage tab)
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
                    add.AddPackages(t.Result.packages, listview, pagenumber);
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
                                 where item.IsLatestVersion && (item.Tags == null || !item.Tags.Contains(HideReleaseFromEndUser))
                                 select item;

                    var info = new PackageList();
                    //info.TotalPackageCount = result.Count();
                    //info.packages = result.Skip(pagenumber * Paging.PageSize).Take(Paging.PageSize).ToArray();
                    info.packages = result.ToArray(); //Toggle comments here to reenable paging.

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
    }
}
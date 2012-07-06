using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NuGet;
using System.Drawing;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class Paging
    {
        public event EventHandler<PageSelectedEventArgs> PageChanged;

        public const int PageSize = 9;
        private List<Button> listOfButtons = new List<Button>();

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
    }
}
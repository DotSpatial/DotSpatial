using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NuGet;

namespace DotSpatial.Plugins.Updater
{
    public partial class Updater : Form
    {
        private String ExtensionFolder;
        private List<Tuple<string, string>> ExtensionsToUpdate;
        private readonly Packages packages;

        public Updater(string[] args)
        {
            ExtensionsToUpdate = new List<Tuple<string, string>>();
            InitializeComponent();

            if (args.Length > 0)
            {
                ExtensionFolder = args[0];

                for (int i = 1; i < args.Length; i++)
                {
                    ExtensionsToUpdate.Add(new Tuple<string, string>(args[i], args[i + 1]));
                    i++;
                }
            }

            packages = new Packages(ExtensionFolder);
            performUpdates();
        }

        private void performUpdates()
        {
            foreach (Tuple<string, string> tuple in ExtensionsToUpdate)
                UpdatePackage(packages.Repo.FindPackage(tuple.Item1), tuple.Item2);
        }

        internal void UpdatePackage(IPackage pack, String CurrentLocation)
        {
            if (pack == null) return;

            //try
            //{
            //    BackupExtension(extension);
            //}
            //catch (Exception)
            //{
            //    DialogResult dialogResult = MessageBox.Show("Unable to make a backup of the extension." +
            //    "\n\nDo you want to Update without backing up?", "Backup Error", MessageBoxButtons.YesNo);
            //    if (dialogResult == DialogResult.No)
            //    {
            //        throw new Exception();
            //    }
            //}

            //App.ProgressHandler.Progress(null, 0, "Updating " + pack.Title);

            // get new version
            packages.Update(pack);

            //App.ProgressHandler.Progress(null, 0, "");
        }
    }
}

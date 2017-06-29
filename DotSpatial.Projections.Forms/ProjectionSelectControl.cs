using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Projections.Forms
{
    public partial class ProjectionSelectControl : UserControl
    {
        #region Fields

        private ProjectionInfo _selectedCoordinateSystem;
        private bool _disableEvents;
        private bool _userChanges;

        #endregion

        public ProjectionSelectControl()
        {
            InitializeComponent();

            chbEsri.Checked = true;
            Load += delegate
            {
                if (DesignMode) return;
                if (SelectedCoordinateSystem == null)
                {
                    radProjected.Checked = true;
                }
            };
        }

        /// <summary>
        /// Gets or sets the currently chosen coordinate system
        /// </summary>
        [Browsable(false)]
        public ProjectionInfo SelectedCoordinateSystem
        {
            get { return _selectedCoordinateSystem; }
            set
            {
                if (_selectedCoordinateSystem == value) return;
                _selectedCoordinateSystem = value;
                if (_selectedCoordinateSystem == null)
                {
                    nudEpsgCode.Value = 0;
                    tbEsriProj4.Text = null;
                    return;
                }
                nudEpsgCode.Value = _selectedCoordinateSystem.AuthorityCode;
                tbEsriProj4.Text = chbEsri.Checked
                    ? _selectedCoordinateSystem.ToEsriString()
                    : _selectedCoordinateSystem.ToProj4String();
                if (!_userChanges)
                {
                    // try to found Category for SelectedCoordinateSystem
                    var prc = GetProjectionCategory(_selectedCoordinateSystem);
                    if (prc == null)
                    {
                        cmbMinorCategory.Text = cmbMajorCategory.Text = "Not found in Predefined";
                        return;
                    }

                    _disableEvents = true;
                    radProjected.Checked = !_selectedCoordinateSystem.IsLatLon;
                    radGeographic.Checked = _selectedCoordinateSystem.IsLatLon;
                    LoadMajorCategories(prc.CategoryName);
                    LoadMinorCategories(prc.ProjectionFieldName);
                    _disableEvents = false;
                }
            }
        }

        private static ProjectionCategory GetProjectionCategory(ProjectionInfo projectionInfo)
        {
            var holder = projectionInfo.IsLatLon
                  ? (ICoordinateSystemCategoryHolder)KnownCoordinateSystems.Geographic
                  : KnownCoordinateSystems.Projected;
            var selectedAsStr = projectionInfo.ToString();
            var selectedEsri = projectionInfo.ToEsriString();
            foreach (var name in holder.Names)
            {
                var cat = holder.GetCategory(name);
                foreach (var projName in cat.Names)
                {
                    var proj = cat.GetProjection(projName);
                    if (proj.ToString() == selectedAsStr &&
                        proj.ToEsriString() == selectedEsri)
                    {
                        return new ProjectionCategory { CategoryName = name, ProjectionFieldName = projName };
                    }
                }
            }
            return null;
        }

        private class ProjectionCategory
        {
            public string CategoryName { get; set; }
            public string ProjectionFieldName { get; set; }
        }

        private ICoordinateSystemCategoryHolder CurrentCategoryHolder
        {
            get
            {
                return radProjected.Checked
                ? (ICoordinateSystemCategoryHolder)KnownCoordinateSystems.Projected
                : KnownCoordinateSystems.Geographic;
            }
        }

        private void radProjected_CheckedChanged(object sender, EventArgs e)
        {
            if (_disableEvents) return;
            LoadMajorCategories();
        }

        private void LoadMajorCategories(string selectedName = null)
        {
            cmbMajorCategory.SuspendLayout();
            cmbMajorCategory.Items.Clear();
            foreach (var name in CurrentCategoryHolder.Names)
            {
                cmbMajorCategory.Items.Add(name);
            }
            if (selectedName != null)
            {
                cmbMajorCategory.SelectedItem = selectedName;
            }
            else
            {
                cmbMajorCategory.SelectedIndex = 0;
            }
            cmbMajorCategory.ResumeLayout();
        }

        private void LoadMinorCategories(string selectedName = null)
        {
            var c = CurrentCategoryHolder.GetCategory((string)cmbMajorCategory.SelectedItem);
            if (c == null) return;

            cmbMinorCategory.SuspendLayout();
            cmbMinorCategory.Items.Clear();
            foreach (var name in c.Names)
            {
                cmbMinorCategory.Items.Add(name);
            }

            if (selectedName != null)
            {
                cmbMinorCategory.SelectedItem = selectedName;
            }
            else
            {
                cmbMinorCategory.SelectedIndex = 0;
            }
            cmbMinorCategory.ResumeLayout();
        }

        private void cmbMajorCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_disableEvents) return;
            LoadMinorCategories();
        }

        private void cmbMinorCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_disableEvents) return;
            var c = CurrentCategoryHolder.GetCategory((string)cmbMajorCategory.SelectedItem);
            if (c == null) return;
            _userChanges = true;
            SelectedCoordinateSystem = c.GetProjection((string)cmbMinorCategory.SelectedItem);
            _userChanges = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ProjectionInfo proj;
            try
            {
                proj = ProjectionInfo.FromEpsgCode((int) nudEpsgCode.Value);
            }
            catch (Exception)
            {
                MessageBox.Show(this, "Unknown EPSG code!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                proj = null;
            }

            if (proj != null)
            {
                SelectedCoordinateSystem = proj;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProjectionInfo proj;
            try
            {
                proj = chbEsri.Checked
                ? ProjectionInfo.FromEsriString(tbEsriProj4.Text)
                : ProjectionInfo.FromProj4String(tbEsriProj4.Text);
            }
            catch (Exception)
            {
                MessageBox.Show(this, "Unknown ESRI or Proj4 string!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                proj = null;
            }
            if (proj != null)
            {
                SelectedCoordinateSystem = proj;
            }   
        }

        private void chbEsri_CheckedChanged(object sender, EventArgs e)
        {
            if (SelectedCoordinateSystem == null) return;
            tbEsriProj4.Text = chbEsri.Checked
                    ? SelectedCoordinateSystem.ToEsriString()
                    : SelectedCoordinateSystem.ToProj4String();
        }
    }
}

using System.Windows.Forms;

namespace DotSpatial.Controls.DefaultRequiredImports
{
    internal class PanelGuiElements
    {
        public ToolStripStatusLabel Caption { get; set; }
        public ToolStripProgressBar Progress { get; set; }

        public static string GetKeyName<T>(string key)
        {
            return typeof(T).Name + key;
        }
    }
}
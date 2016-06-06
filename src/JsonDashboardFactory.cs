using System.ComponentModel.Composition;
using System.Drawing;
using Prosa.Log4View.Extensibility;
using RedBear.Log4View.JsonDashboard.Properties;

namespace RedBear.Log4View.JsonDashboard
{
    /// <summary>
    /// Factory for the JSON extension.
    ///  </summary>
    [Export(typeof(ICustomControlFactory))]
    public class JsonDashboardFactory : ICustomControlFactory
    {
        /// <summary>
        /// Creates the custom control.
        /// </summary>
        /// <returns>CustomControl.</returns>
        public CustomControl CreateCustomControl() {
            return new JsonDashboard();
        }

        /// <summary>
        /// Gets the menu caption.
        /// </summary>
        /// <value>The menu caption.</value>
        public string MenuCaption => "JSON View";

        /// <summary>
        /// Gets the control id.
        /// The control id has to be a unique string.
        /// </summary>
        /// <value>The control id.</value>
        public string ControlId => "RedBear.Log4View.JsonDashboard.JsonDashboard";

        /// <summary>
        /// Gets the custom control icon.
        /// </summary>
        /// <value>The glyph.</value>
        public Image Glyph => Resources.Json16;
    }
}
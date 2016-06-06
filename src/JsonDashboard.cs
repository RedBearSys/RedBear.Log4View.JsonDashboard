using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prosa.Log4View.Extensibility;
using Prosa.Log4View.Message;

namespace RedBear.Log4View.JsonDashboard
{
    /// <summary>
    ///     A sample extension.
    /// </summary>
    public partial class JsonDashboard : CustomControl
    {
        private bool _loaded;
        private LogMessage _message;

        /// <summary>
        ///     Initializes a new instance of the <see cref="JsonDashboard" /> class.
        /// </summary> 
        public JsonDashboard()
        {
            InitializeComponent();
        }

        private void LoadJson(LogMessage message)
        {
            if (!string.IsNullOrEmpty(message?.Message) && !message.Equals(_message) && message.Message.Contains("{") && message.Message.Contains("}"))
            {
                try
                {
                    _loaded = false;
                    // ReSharper disable once PossibleNullReferenceException
                    _message = message;
                    
                    var obj = JObject.Parse(message.Message);

                    treeView.BeginUpdate();
                    treeView.Nodes.Clear();
                    AddObjectNodes(obj, "{}", treeView.Nodes);
                    treeView.Invalidate();
                    treeView.SelectedNode = treeView.Nodes[0];
                    treeView.Nodes[0].Expand();
                    treeView.Nodes[0].EnsureVisible();
                    treeView.EndUpdate();

                    _loaded = true;
                }
                catch (JsonException)
                {
                    _message = null;
                    treeView.Nodes.Clear();
                    _loaded = false;
                }
            }
            else if (message != null && !message.Equals(_message))
            {
                _message = null;
                treeView.Nodes.Clear();
                _loaded = false;
            }
        }

        private void JsonDashboard_Load(object sender, EventArgs e)
        {
            // Hook on API events
            API.CurrentMessageChanged += OnCurrentMessageChanged;
            API.ConfigurationChanged += OnConfigurationChanged;

            LoadJson(API.CurrentMessage);
            UpdateStatus();
        }

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            _message = null;
        }


        private void OnCurrentMessageChanged(object sender, CurrentMessageChangedArgs args)
        {
            LoadJson(args.CurrentMessage);
        }

        private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            var text = e.Node.Text;

            if (e.Node.IsSelected)
            {
                var silverBrush = new SolidBrush(Color.LightGray);
                e.Graphics.FillRectangle(silverBrush, e.Bounds);
            }

            if (text.Contains("*|:|*"))
            {
                var name = text.Substring(0, text.IndexOf("*|:|*", StringComparison.Ordinal));
                var value = text.Substring(text.IndexOf("*|:|*", StringComparison.Ordinal) + 5);

                using (var font = new Font(treeView.Font, FontStyle.Regular))
                {
                    using (Brush brush = new SolidBrush(Color.Green))
                    {
                        e.Graphics.DrawString(name + ':', font, brush, e.Bounds.Left, e.Bounds.Top);
                    }

                    using (Brush brush = new SolidBrush(Color.Black))
                    {
                        var s = e.Graphics.MeasureString(name, font);
                        e.Graphics.DrawString(value, font, brush, e.Bounds.Left + (int) s.Width, e.Bounds.Top);
                    }
                }
            }
            else if (!e.Node.Text.StartsWith("["))
            {
                using (Brush brush = new SolidBrush(Color.Blue))
                using (var font = new Font(treeView.Font, FontStyle.Bold))
                {
                    e.Graphics.DrawString(e.Node.Text, font, brush, e.Bounds.Left, e.Bounds.Top);
                }
            }
            else
            {
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    e.Graphics.DrawString(e.Node.Text, treeView.Font, brush, e.Bounds.Left, e.Bounds.Top);
                }
            }
        }

        private void AddObjectNodes(JObject @object, string name, TreeNodeCollection parent)
        {
            var node = new TreeNode(name);
            parent.Add(node);

            foreach (var property in @object.Properties())
            {
                AddTokenNodes(property.Value, property.Name, node.Nodes);
            }
        }

        private void AddArrayNodes(JArray array, string name, TreeNodeCollection parent)
        {
            var node = new TreeNode($"{name} []");
            parent.Add(node);

            for (var i = 0; i < array.Count; i++)
            {
                AddTokenNodes(array[i], $"[{i}]", node.Nodes);
            }
        }

        private void AddTokenNodes(JToken token, string name, TreeNodeCollection parent)
        {
            if (token is JValue)
            {
                var content = $"{name}*|:|*{CreatePadding(name)}{((JValue) token).Value}";
                var node = new TreeNode(content) { Tag = ((JValue)token).Value };
                parent.Add(node);
            }
            else if (token is JArray)
            {
                AddArrayNodes((JArray) token, name, parent);
            }
            else if (token is JObject && !name.StartsWith("["))
            {
                AddObjectNodes((JObject) token, name + " {}", parent);
            }
            else if (token is JObject)
            {
                AddObjectNodes((JObject) token, name, parent);
            }
        }

        private static string CreatePadding(string name)
        {
            var spaces = 30 - name.Length;
            if (spaces < 1) spaces = 1;
            var padding = string.Empty;

            for (var temp = 0; temp < spaces; temp++)
            {
                padding += " ";
            }

            return padding;
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (_loaded) treeView.Invalidate();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(treeView.SelectedNode.Tag.ToString());
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Node.Tag?.ToString()))
            {
                treeView.ContextMenuStrip = contextMenuStrip;
            }
            else
            {
                treeView.ContextMenuStrip = null;
            }
        }

        private void TreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (treeView.SelectedNode.Tag != null && e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            {
                Clipboard.SetText(treeView.SelectedNode.Tag.ToString());
            }
        }
    }
}
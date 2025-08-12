using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DiversityWorkbench.UserControls
{
    public class UserControlJsonTreeView : UserControl
    {
        private RichTextBox richTextBoxJson;
        public UserControlJsonTreeView()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            this.richTextBoxJson = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true
            };
            this.Controls.Add(this.richTextBoxJson);
        }
        public void LoadJson(string jsonString)
        {
            var formattedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(jsonString), Formatting.Indented);
            richTextBoxJson.Text = formattedJson;
        }

        public void ClearJson()
        {
            richTextBoxJson.Clear();
        }

    }
}

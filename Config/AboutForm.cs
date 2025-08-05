using System.Windows.Forms;

namespace ZMatrixConfig
{
    public class AboutForm : Form
    {
        public AboutForm()
        {
            Text = "About ZMatrix";
            Width = 300;
            Height = 150;
            var label = new Label
            {
                Text = "ZMatrix\nC# Configuration\nConverted from Borland",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            var button = new Button { Text = "OK", Dock = DockStyle.Bottom };
            button.Click += (s, e) => Close();
            Controls.Add(label);
            Controls.Add(button);
        }
    }
}

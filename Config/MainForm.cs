using System;
using System.IO;
using System.Windows.Forms;

namespace ZMatrixConfig
{
    public class MainForm : Form
    {
        private NumericUpDown numMaxStream;
        private NumericUpDown numSpeedVariance;
        private CheckBox chkMonotonous;
        private NumericUpDown numBackTrace;
        private CheckBox chkRandomized;
        private NumericUpDown numLeading;
        private NumericUpDown numSpacePad;
        private NumericUpDown numRefreshTime;
        private ComboBox cmbPriority;
        private NumericUpDown numSpecialProb;

        private IniFile ini;
        private ConfigModel model;
        private string configPath;

        public MainForm()
        {
            Text = "ZMatrix Configuration";
            Width = 400;
            Height = 400;

            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 10,
                AutoSize = true
            };

            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));

            int row = 0;
            void AddRow(string labelText, Control control)
            {
                panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                panel.Controls.Add(new Label { Text = labelText, Anchor = AnchorStyles.Left, AutoSize = true }, 0, row);
                panel.Controls.Add(control, 1, row);
                control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                row++;
            }

            numMaxStream = new NumericUpDown { Minimum = 0, Maximum = 10000 };
            AddRow("Max Stream", numMaxStream);

            numSpeedVariance = new NumericUpDown { Minimum = 0, Maximum = 100 };
            AddRow("Speed Variance", numSpeedVariance);

            chkMonotonous = new CheckBox();
            AddRow("Monotonous Cleanup", chkMonotonous);

            numBackTrace = new NumericUpDown { Minimum = 0, Maximum = 1000 };
            AddRow("Back Trace", numBackTrace);

            chkRandomized = new CheckBox();
            AddRow("Randomized Cleanup", chkRandomized);

            numLeading = new NumericUpDown { Minimum = 0, Maximum = 100 };
            AddRow("Leading", numLeading);

            numSpacePad = new NumericUpDown { Minimum = 0, Maximum = 100 };
            AddRow("Space Pad", numSpacePad);

            numRefreshTime = new NumericUpDown { Minimum = 1, Maximum = 1000 };
            AddRow("Refresh Time", numRefreshTime);

            cmbPriority = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPriority.Items.AddRange(new object[] {
                "ABOVE_NORMAL_PRIORITY_CLASS",
                "BELOW_NORMAL_PRIORITY_CLASS",
                "HIGH_PRIORITY_CLASS",
                "IDLE_PRIORITY_CLASS",
                "NORMAL_PRIORITY_CLASS"
            });
            AddRow("Priority", cmbPriority);

            numSpecialProb = new NumericUpDown { Minimum = 0, Maximum = 1, DecimalPlaces = 2, Increment = 0.01M };
            AddRow("Special Stream Probability", numSpecialProb);

            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true
            };
            var btnSave = new Button { Text = "Save" };
            btnSave.Click += SaveConfig;
            var btnClose = new Button { Text = "Close" };
            btnClose.Click += (s, e) => Close();
            buttonPanel.Controls.Add(btnSave);
            buttonPanel.Controls.Add(btnClose);
            var btnAbout = new Button { Text = "About" };
            btnAbout.Click += (s, e) => new AboutForm().ShowDialog(this);
            buttonPanel.Controls.Add(btnAbout);

            Controls.Add(panel);
            Controls.Add(buttonPanel);

            Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "default.cfg");
            ini = IniFile.Load(configPath);
            model = ConfigModel.FromIni(ini);

            numMaxStream.Value = model.MaxStream;
            numSpeedVariance.Value = model.SpeedVariance;
            chkMonotonous.Checked = model.MonotonousCleanupEnabled;
            numBackTrace.Value = model.BackTrace;
            chkRandomized.Checked = model.RandomizedCleanupEnabled;
            numLeading.Value = model.Leading;
            numSpacePad.Value = model.SpacePad;
            numRefreshTime.Value = model.RefreshTime;
            cmbPriority.SelectedItem = model.PriorityClass;
            numSpecialProb.Value = (decimal)model.SpecialStringStreamProbability;
        }

        private void SaveConfig(object sender, EventArgs e)
        {
            model.MaxStream = (int)numMaxStream.Value;
            model.SpeedVariance = (int)numSpeedVariance.Value;
            model.MonotonousCleanupEnabled = chkMonotonous.Checked;
            model.BackTrace = (int)numBackTrace.Value;
            model.RandomizedCleanupEnabled = chkRandomized.Checked;
            model.Leading = (int)numLeading.Value;
            model.SpacePad = (int)numSpacePad.Value;
            model.RefreshTime = (int)numRefreshTime.Value;
            model.PriorityClass = cmbPriority.SelectedItem?.ToString() ?? "IDLE_PRIORITY_CLASS";
            model.SpecialStringStreamProbability = (double)numSpecialProb.Value;

            model.Apply(ini);
            ini.Save(configPath);
            MessageBox.Show("Configuration saved", "ZMatrix", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

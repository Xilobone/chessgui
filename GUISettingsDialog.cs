namespace gui
{
    /// <summary>
    /// Represents a dialog window where gui settings can be viewed and changed
    /// </summary>
    public class GUISettingsDialog : Form
    {
        private GUISettings settings;

        /// <summary>
        /// Creates a new gui settings dialog window
        /// </summary>
        public GUISettingsDialog()
        {
            Size = new Size(300, 400);
            settings = GUISettings.SETTINGS;

            showSetting("lightColor", settings.lightColor, 0);
            showSetting("darkColor", settings.darkColor, 1);
            showSetting("showFilesAndRanks", 2);

            Button save = new Button();
            save.Text = "Save changes";
            save.Click += OnSave;
            save.Location = new Point(150, 250);
            Controls.Add(save);
        }

        private void OnSave(object? sender, EventArgs e)
        {
            GUISettings.writeAsDefault(settings);
            Close();
        }

        private void showSetting(string name, string value, int position)
        {
            Label label = new Label();
            label.Text = $"{name}:";
            label.Location = new Point(0, 50 * position);
            Controls.Add(label);

            Button button = new Button();
            button.Text = value;
            button.BackColor = (Color)new ColorConverter().ConvertFromString(value)!;
            button.DataBindings.Add("Text", settings, name);
            button.Location = new Point(150, 50 * position);

            button.Click += (sender, e) =>
            {
                ColorDialog colorDialog = new ColorDialog();
                colorDialog.Color = button.BackColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    button.BackColor = colorDialog.Color;
                    button.Text = $"#{colorDialog.Color.R:X2}{colorDialog.Color.G:X2}{colorDialog.Color.B:X2}";

                }
            };

            Controls.Add(button);
        }

        private void showSetting(string name, int position)
        {
            Label label = new Label();
            label.Text = $"{name}:";
            label.Location = new Point(0, 50 * position);
            Controls.Add(label);

            CheckBox checkBox = new CheckBox();
            checkBox.Location = new Point(100, 50 * position);
            checkBox.Size = new Size(16, 16);
            checkBox.DataBindings.Add("Checked", settings, name);
            Controls.Add(checkBox);
        }
    }
}
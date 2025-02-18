using chessPlayer;

namespace gui
{
    public class SettingsDialog : Form
    {
        private ChessPlayerSettings settings;
        public SettingsDialog()
        {
            Size = new Size(300, 400);
            settings = ChessPlayerSettings.DEFAULT_SETTINGS;

            showSetting("limitedTurns", "maxTurns", 0);
            showSetting("limitedTime", "maxTime", 1);
            showSetting("limitedTurnTime", "maxTurnTime", 2);

            showSetting("displayBoards", 3);
            showSetting("requireInputAfterEachTurn", 4);

            Label label = new Label();
            label.Text = settings.configPath;
            label.Location = new Point(200, 200);
            Controls.Add(label);

            Button changeConfig = new Button();
            changeConfig.Text = "Change config";
            changeConfig.Click += OpenFileButton_Click;
            changeConfig.Location = new Point(50, 250);
            Controls.Add(changeConfig);

            Button save = new Button();
            save.Text = "Save changes";
            save.Click += OnSave;
            save.Location = new Point(150, 250);
            Controls.Add(save);
        }

        private void OnSave(object? sender, EventArgs e)
        {
            // settings.maxTurnTime = 123456;
            ChessPlayerSettings.writeAsDefault(settings);
            Close();
        }

        private void showSetting(string name, string valueName, int position)
        {
            showSetting(name, position);

            NumericUpDown numericUpDown = new NumericUpDown();
            numericUpDown.Minimum = 0;
            numericUpDown.DecimalPlaces = 0;
            numericUpDown.Maximum = 99999999;
            numericUpDown.Location = new Point(150, 50 * position);
            numericUpDown.DataBindings.Add("Value", settings, valueName);
            Controls.Add(numericUpDown);
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

        private void OpenFileButton_Click(object? sender, EventArgs e)
        {
            // Create an OpenFileDialog instance
            Console.WriteLine("opening file dialog");


            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
            openFileDialog.InitialDirectory = settings.configPath;
            // Show the dialog and check if the user selected a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file path
                string selectedFile = openFileDialog.FileName;
                MessageBox.Show("File selected: " + selectedFile);
            }
        }
    }
}
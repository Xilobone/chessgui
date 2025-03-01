
namespace gui
{
    /// <summary>
    /// Class containing all elements and logic of the gui menu strip
    /// </summary>
    public class GUIMenuStrip : MenuStrip
    {
        private GUI gui;

        /// <summary>
        /// Creates a new gui menu strip object
        /// </summary>
        /// <param name="gui">The gui to attatch the menu strip to</param>
        public GUIMenuStrip(GUI gui) : base()
        {
            this.gui = gui;

            //add game controls
            ToolStripMenuItem gameControls = new ToolStripMenuItem("Game Controls");
            ToolStripMenuItem newGame = new ToolStripMenuItem("New game");
            ToolStripMenuItem stopGame = new ToolStripMenuItem("End game");
            newGame.Click += OnNewGame;
            stopGame.Click += OnStopGame;
            gameControls.DropDownItems.AddRange(newGame, stopGame);
            Items.Add(gameControls);

            //add change settings button
            ToolStripMenuItem changeSettings = new ToolStripMenuItem("Change settings");
            changeSettings.Click += OnChangeSettings;
            Items.Add(changeSettings);

            //add customize button
            ToolStripMenuItem customize = new ToolStripMenuItem("Customize gui");
            customize.Click += OnCustomize;
            Items.Add(customize);
        }

        private void OnCustomize(object? sender, EventArgs e)
        {
            new GUISettingsDialog().ShowDialog();
            gui.Invalidate();
        }

        private void OnStopGame(object? sender, EventArgs e)
        {
            gui.StopGame();
        }

        private void OnChangeSettings(object? sender, EventArgs e)
        {
            new SettingsDialog().ShowDialog();
        }

        private void OnNewGame(object? sender, EventArgs e)
        {
            gui.StartGame();
        }

    }
}
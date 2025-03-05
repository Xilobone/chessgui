using chess;

namespace gui
{
    /// <summary>
    /// Represents the panel in the gui in which a new game can be started
    /// </summary>
    public class NewGamePanel : Panel
    {
        private static int MARGIN = 16;
        /// <summary>
        /// The white player combobox
        /// </summary>
        public ComboBox whitePlayerSelect;

        /// <summary>
        /// The black player combobox
        /// </summary>
        public ComboBox blackPlayerSelect;

        /// <summary>
        /// Creates a new game panel
        /// </summary>
        public NewGamePanel(GUI gui) : base()
        {
            Size = new Size(300, 200);
            BackColor = Color.FromArgb(235, 208, 148);

            //add title
            Label title = new Label();
            title.Location = new Point(MARGIN, MARGIN);
            title.Size = new Size(Size.Width - 2 * MARGIN, 32);
            title.Text = "Start a new game";
            title.Font = new Font("Arial", 12, FontStyle.Bold);
            Controls.Add(title);

            //add white player select
            Label whiteLabel = new Label();
            whiteLabel.Location = new Point(MARGIN, title.Location.Y + title.Size.Height + MARGIN);
            whiteLabel.Text = "White player";
            whiteLabel.Font = new Font("Arial", 12, FontStyle.Regular);
            Controls.Add(whiteLabel);
            whitePlayerSelect = new ComboBox();
            whitePlayerSelect.DropDownStyle = ComboBoxStyle.DropDownList;

            IPlayer whitePlayer = new Player<Player>();
            Player white = (Player)whitePlayer.engine;
            gui.moveSelecter.onMove += white.OnMove;
            PlayerList.whitePlayers[0] = whitePlayer;

            whitePlayerSelect.DataSource = PlayerList.whitePlayers;
            whitePlayerSelect.DisplayMember = "name";
            whitePlayerSelect.Location = new Point(16, whiteLabel.Location.Y + whiteLabel.Size.Height + MARGIN);
            Controls.Add(whitePlayerSelect);

            IPlayer blackPlayer = new Player<Player>();
            Player black = (Player)blackPlayer.engine;
            gui.moveSelecter.onMove += black.OnMove;
            PlayerList.blackPlayers[0] = blackPlayer;

            //add black player select
            Label blackLabel = new Label();
            blackLabel.Location = new Point(166, title.Location.Y + title.Size.Height + MARGIN);
            blackLabel.Text = "Black player";
            blackLabel.Font = new Font("Arial", 12, FontStyle.Regular);
            Controls.Add(blackLabel);
            blackPlayerSelect = new ComboBox();
            blackPlayerSelect.DropDownStyle = ComboBoxStyle.DropDownList;
            blackPlayerSelect.DataSource = PlayerList.blackPlayers;
            blackPlayerSelect.DisplayMember = "name";
            blackPlayerSelect.Location = new Point(166, blackLabel.Location.Y + blackLabel.Size.Height + MARGIN);
            Controls.Add(blackPlayerSelect);
        }
    }
}
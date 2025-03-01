using chess;
using chessPlayer;

namespace gui
{
    /// <summary>
    /// The main gui window of the application
    /// </summary>
    public class GUI : Form
    {
        
        /// <summary>
        /// The current board that is being displayed on the gui
        /// </summary>
        public Board board;
        private MoveSelecter moveSelecter;

        private ComboBox whitePlayerSelect;
        private ComboBox blackPlayerSelect;

        private ChessPlayer? player;

        private Thread? chessThread;

        /// <summary>
        /// Creates a new GUI window
        /// </summary>
        public GUI()
        {
            board = new Board();
            moveSelecter = new MoveSelecter(this);
            moveSelecter.onMove += OnMoveMade;
            moveSelecter.onMoveSelectionChange += board.onMoveSelectionChange;
            moveSelecter.onMoveSelectionChange += OnChange;

            DoubleBuffered = true;

            Text = "Chess GUI";
            Size = new Size(1920, 1080);

            //add the menu strip
            GUIMenuStrip menuStrip = new GUIMenuStrip(this);
            MainMenuStrip = menuStrip;
            Controls.Add(menuStrip);

            whitePlayerSelect = new ComboBox();
            whitePlayerSelect.DropDownStyle = ComboBoxStyle.DropDownList;


            Player white = new Player(true);
            moveSelecter.onMove += white.OnMove;
            PlayerList.whitePlayers[0] = new chess.Player("gui player", white, new player.Evaluator());

            whitePlayerSelect.DataSource = PlayerList.whitePlayers;
            whitePlayerSelect.DisplayMember = "name";
            whitePlayerSelect.Location = new Point(600, 100);
            Controls.Add(whitePlayerSelect);

            Player black = new Player(false);
            moveSelecter.onMove += black.OnMove;
            PlayerList.blackPlayers[0] = new chess.Player("gui player", black, new player.Evaluator());

            blackPlayerSelect = new ComboBox();
            blackPlayerSelect.DropDownStyle = ComboBoxStyle.DropDownList;
            blackPlayerSelect.DataSource = PlayerList.blackPlayers;
            blackPlayerSelect.DisplayMember = "name";
            blackPlayerSelect.Location = new Point(750, 100);
            Controls.Add(blackPlayerSelect);
        }

        private void OnChange(object? sender, MoveSelecter.MoveSelectionEvent e)
        {
            Invalidate();
        }

        /// <summary>
        /// Starts a new game of chess with the selected players, stops any previously running games, if any
        /// </summary>
        public void StartGame()
        {
            StopGame();

            //create new chess player
            player = new ChessPlayer((chess.Player)whitePlayerSelect.SelectedItem!, (chess.Player)blackPlayerSelect.SelectedItem!);
            player.onChange += OnChange;
            player.onChange += board.OnChange;
            chessThread = new Thread(() =>
            {
                player.Play();

            });

            chessThread.IsBackground = true;
            chessThread.Start();
        }

        /// <summary>
        /// Stops the currently running game, if any
        /// </summary>
        public void StopGame()
        {
            if (chessThread == null || !chessThread.IsAlive || player == null)
            {
                return;
            }

            //no longer listen to old chess player updates and stop running old player
            player.onChange -= OnChange;
            player.onChange -= board.OnChange;
            player.Stop();
        }

        private void OnChange(object? sender, ChessEventArgs e)
        {
            Invalidate();
        }

        private void OnMoveMade(object? sender, MoveSelecter.MoveEvent e)
        {
            if (player == null) return;
        }

        /// <summary>
        /// Draws the graphics on the screen, also updates the displayed board
        /// </summary>
        /// <param name="e">The paint event arguments to use</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            board.Draw(g);
        }
    }
}
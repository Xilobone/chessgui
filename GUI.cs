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

        /// <summary>
        /// Responsible for listening to move selection on the board
        /// </summary>
        public MoveSelecter moveSelecter;

        // private ComboBox whitePlayerSelect;
        // private ComboBox blackPlayerSelect;
        private Label infoLabel;
        private TextBox fenLabel;

        private NewGamePanel newGamePanel;
        private ChessPlayer? player;

        private Thread? chessThread;

        /// <summary>
        /// Creates a new GUI window
        /// </summary>
        public GUI()
        {   
            BackColor = Color.FromArgb(235, 223, 196);
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

            //add new game panel
            newGamePanel = new NewGamePanel(this);
            newGamePanel.Location = new Point(600, 100);
            Controls.Add(newGamePanel);

            //add info label
            infoLabel = new Label();
            infoLabel.Location = new Point(newGamePanel.Location.X, newGamePanel.Location.Y + newGamePanel.Size.Height + 8);
            infoLabel.Size = new Size(newGamePanel.Size.Width, 128);
            infoLabel.BackColor = Color.Blue;
            Controls.Add(infoLabel);

            fenLabel = new TextBox();
            fenLabel.ReadOnly = true;
            fenLabel.Size = new Size(600, 50);
            fenLabel.Location = new Point(Board.BOARD_OFFSET[0], Board.BOARD_OFFSET[1] + 8 * Board.SQUARE_SIZE + 16);
            Controls.Add(fenLabel);
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
            player = new ChessPlayer((IPlayer)newGamePanel.whitePlayerSelect.SelectedItem!, (IPlayer)newGamePanel.blackPlayerSelect.SelectedItem!);
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
            fenLabel.Text = e.board.toFen();
            string playerToMove = e.board.whiteToMove ? "white" : "black";
            infoLabel.Text = $"{playerToMove} to move";

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
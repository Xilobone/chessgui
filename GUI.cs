using chess;
using chessPlayer;

namespace gui
{
    public class GUI : Form
    {

        public Board board;
        private MoveSelecter moveSelecter;

        private ComboBox whitePlayerSelect;
        private ComboBox blackPlayerSelect;

        private ChessPlayer? player;

        private Thread? chessThread;

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

            chess.Player[] whitePlayers = new chess.Player[PlayerList.whitePlayers.Length + 1];

            Player white = new Player(true, new player.Evaluator());
            moveSelecter.onMove += white.OnMove;
            whitePlayers[0] = new chess.Player("gui player", white, new player.Evaluator());

            for (int i = 0; i < PlayerList.whitePlayers.Length; i++)
            {
                whitePlayers[i + 1] = PlayerList.whitePlayers[i];
            }

            whitePlayerSelect.DataSource = whitePlayers;
            whitePlayerSelect.DisplayMember = "name";
            whitePlayerSelect.Location = new Point(600, 100);
            Controls.Add(whitePlayerSelect);

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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            board.Draw(g);
        }
    }
}
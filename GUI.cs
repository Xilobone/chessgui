using chess;
using chessPlayer;

namespace gui
{
    public class GUI : Form
    {

        public Board board;
        private MoveSelecter moveSelecter;

        private Button button;

        // static Thread? chessThread;

        private ChessPlayer player;
        public static GUI Create()
        {
            GUI gui = new GUI();

            new Thread(() =>
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(gui);
                }).Start();

            return gui;
        }

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

            button = new Button();
            button.Text = "New Game";
            button.Click += OnClick;
            Controls.Add(button);
        }

        private void OnChange(object? sender, MoveSelecter.MoveSelectionEvent e)
        {
            Invalidate();
        }

        private void OnClick(object? sender, EventArgs e)
        {
            player = new ChessPlayer();

            // chessThread = new Thread(() =>
            // {

            // });

            // chessThread.IsBackground = true;
            // chessThread.Start();
            player.onChange += OnChange;
            player.onChange += board.OnChange;

            player.Play();
            

        }

        private void OnChange(object? sender, ChessPlayer.ChessPlayerEvent e)
        {
            Invalidate();
        }

        private void OnMoveMade(object? sender, MoveSelecter.MoveEvent e)
        {
            if (player == null) return;

            player.makeMove(e.move);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            board.Draw(g);
        }
    }
}
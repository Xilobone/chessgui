using chessPlayer;

namespace gui
{
    public class GUI : Form
    {

        private Board board;

        public static GUI Create(ChessPlayer? player)
        {   
            GUI gui = new GUI(player);
            Thread guiThread = new Thread(() =>
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new GUI(player));
            });
            guiThread.Start();

            return gui;
        }

        public static GUI Create()
        {
            return Create(null);
        }

        public GUI(ChessPlayer? player)
        {
            board = new Board();

            if (player != null) 
            {
                player.onChange += OnChange;
                player.onChange += board.OnChange;
            }

            DoubleBuffered = true;

            Text = "Chess GUI";
            Size = new Size(1920, 1080);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            board.Draw(g);
        }

        public void OnChange(object? sender, ChessEventArgs e)
        {
            Refresh();
        }
    }
}
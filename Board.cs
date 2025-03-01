using chess;
using chessPlayer;

namespace gui
{
    /// <summary>
    /// Class that represents the visible board on the gui
    /// </summary>
    public class Board
    {
        /// <summary>
        /// The chessboard that is currently shown
        /// </summary>
        public chess.Board? board;

        /// <summary>
        /// The x and y offset (in pixels) of the board relative to the top left corner of the screen
        /// </summary>
        public static readonly int[] BOARD_OFFSET = [64, 64];

        /// <summary>
        /// The size (in pixels) of each of the squares of the board
        /// </summary>
        public static readonly int SQUARE_SIZE = 64;

        private static Dictionary<int, int> IMAGE_INDEX = new Dictionary<int, int>() {
            {Piece.WHITE_KING, 0},
            {Piece.WHITE_QUEEN, 1},
            {Piece.WHITE_BISHOP, 2},
            {Piece.WHITE_KNIGHT, 3},
            {Piece.WHITE_ROOK, 4},
            {Piece.WHITE_PAWN, 5},
            {Piece.BLACK_KING, 6},
            {Piece.BLACK_QUEEN, 7},
            {Piece.BLACK_BISHOP, 8},
            {Piece.BLACK_KNIGHT, 9},
            {Piece.BLACK_ROOK, 10},
            {Piece.BLACK_PAWN, 11},
        };

        private static readonly int PIECE_SIZE = 133;

        private Image image;

        private int selectedIndex = -1;

        // private ulong bitboard;

        /// <summary>
        /// Creates a new board object
        /// </summary>
        public Board() : this(null) { }

        /// <summary>
        /// Creates a new board object, that stores a chessboard
        /// </summary>
        /// <param name="board">The chess board to show</param>
        public Board(chess.Board? board)
        {
            this.board = board;

            image = Image.FromFile($"{AppDomain.CurrentDomain.BaseDirectory}/../../../lib/chess_pieces.png");
        }

        /// <summary>
        /// Draws the board, pieces, files, ranks and highlighted squares on the gui
        /// </summary>
        /// <param name="g">The graphics object to use for drawing</param>
        public void Draw(Graphics g)
        {
            //draw board
            Color light = (Color)new ColorConverter().ConvertFromString(GUISettings.SETTINGS.lightColor)!;
            Color dark = (Color)new ColorConverter().ConvertFromString(GUISettings.SETTINGS.darkColor)!;
            Brush lightBrush = new SolidBrush(light);
            Brush darkBrush = new SolidBrush(dark);

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Brush brush = (x + y) % 2 == 0 ? lightBrush : darkBrush;

                    g.FillRectangle(brush, BOARD_OFFSET[0] + SQUARE_SIZE * x, BOARD_OFFSET[1] + SQUARE_SIZE * y, SQUARE_SIZE, SQUARE_SIZE);
                }
            }

            lightBrush.Dispose();
            darkBrush.Dispose();

            if (GUISettings.SETTINGS.showFilesAndRanks)
            {
                Font font = new Font("Arial", 12, FontStyle.Regular);
                Brush brush = Brushes.Black;
                //show files
                for (int file = 0; file < 8; file++)
                {
                    char f = (char)('a' + file);
                    g.DrawString(f.ToString(), font, brush, new Point((int)(BOARD_OFFSET[0] + SQUARE_SIZE * (file + 0.5)), BOARD_OFFSET[1] + SQUARE_SIZE * 8));
                }

                //show ranks
                for (int rank = 0; rank < 8; rank++)
                {
                    int r = 8 - rank;
                    g.DrawString(r.ToString(), font, brush, new Point(BOARD_OFFSET[0] - 16, (int)(BOARD_OFFSET[0] + SQUARE_SIZE * (rank + 0.5))));

                }
            }

            // DrawBitboard(g);

            if (selectedIndex != -1) DrawSquare(g, selectedIndex, Color.Yellow);
            DrawPieces(g);
        }

        private void DrawSquare(Graphics g, int index, Color color)
        {
            int y = index / 8;
            int x = index % 8;
            g.FillRectangle(new SolidBrush(color), BOARD_OFFSET[0] + SQUARE_SIZE * x, BOARD_OFFSET[1] + SQUARE_SIZE * (7 - y), SQUARE_SIZE, SQUARE_SIZE);
        }

        // private void DrawBitboard(Graphics g)
        // {
        //     Brush brush = new SolidBrush(bitboardColor);
        //     ulong btb = bitboard;

        //     for (int i = 0; i < 64; i++)
        //     {
        //         bool isAttacking = (btb & 1) == 1;
        //         btb >>= 1;

        //         if (!isAttacking)
        //         {
        //             continue;
        //         }

        //         int y = i / 8;
        //         int x = i % 8;

        //         g.FillRectangle(brush, BOARD_OFFSET[0] + SQUARE_SIZE * x, BOARD_OFFSET[1] + SQUARE_SIZE * (7 - y), SQUARE_SIZE, SQUARE_SIZE);

        //     }
        // }

        private void DrawPieces(Graphics g)
        {
            if (board == null)
            {
                return;
            }

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    int piece = board.getPiece(x + 8 * y);
                    if (piece == Piece.EMPTY)
                    {
                        continue;
                    }

                    int imageX = IMAGE_INDEX[piece] % 6;
                    int imageY = IMAGE_INDEX[piece] < 6 ? 0 : 1;

                    Rectangle source = new Rectangle(PIECE_SIZE * imageX, PIECE_SIZE * imageY, PIECE_SIZE, PIECE_SIZE);

                    //board y positions inverted
                    Rectangle position = new Rectangle(BOARD_OFFSET[0] + SQUARE_SIZE * x, BOARD_OFFSET[1] + SQUARE_SIZE * (7 - y), SQUARE_SIZE, SQUARE_SIZE);
                    g.DrawImage(image, position, source, GraphicsUnit.Pixel);

                }
            }
        }

        internal void onMoveSelectionChange(object? sender, MoveSelecter.MoveSelectionEvent e)
        {
            selectedIndex = e.fr;
        }

        internal void OnChange(object? sender, ChessEventArgs e)
        {
            board = e.board;
        }
    }
}
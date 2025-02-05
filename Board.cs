using chess;
using chessPlayer;

namespace gui
{
    public class Board
    {
        private static readonly int[] BOARD_OFFSET = [64, 64];
        private static readonly int SQUARE_SIZE = 64;

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

        private Color light = Color.FromArgb(219, 198, 140);
        private Color dark = Color.FromArgb(128, 100, 25);
        private Color bitboardColor = Color.FromArgb(255, 10, 10);

        private ulong bitboard;

        private chess.Board? board;
        public Board() : this(null) {}

        public Board(chess.Board? board)
        {
            this.board = board;

            image = Image.FromFile($"{AppDomain.CurrentDomain.BaseDirectory}/../../../lib/chess_pieces.png");
        }

        public void Draw(Graphics g)
        {
            //draw board
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

            DrawBitboard(g);
            DrawPieces(g);
        }

        private void DrawBitboard(Graphics g)
        {
            Brush brush = new SolidBrush(bitboardColor);
            ulong btb = bitboard;

            for (int i = 0; i < 64; i++)
            {
                bool isAttacking = (btb & 1) == 1;
                btb >>= 1;

                if (!isAttacking)
                {
                    continue;
                }

                int y = i / 8;
                int x = i % 8;

                g.FillRectangle(brush, BOARD_OFFSET[0] + SQUARE_SIZE * x, BOARD_OFFSET[1] + SQUARE_SIZE * (7 - y), SQUARE_SIZE, SQUARE_SIZE);

            }


        }
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
                    int piece = board.getPiece(new Position(x, y));
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

        public void OnChange(object? sender, ChessEventArgs e)
        {
            board = e.board;
            // UpdateBitboard();
            // Refresh();
        }
    }
}
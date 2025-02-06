using System.Diagnostics.Tracing;
using chess;
using gui;

namespace gui
{
    public class ChessPlayer
    {
        chess.Board? board;

        private chess.Player white;
        private chess.Player black;

        public EventHandler<ChessPlayerEvent>? onChange;
        public ChessPlayer(chess.Player white, chess.Player black)
        {
            this.white = white;
            this.black = black;
        }

        public void Play()
        {
            board = chess.Board.startPosition();
            onChange?.Invoke(this, new ChessPlayerEvent(board));

            while (!board.isInMate())
            {

                Move move;
                if (board.whiteToMove)
                {
                    move = white.engine.makeMove(board, 10000);
                }
                else
                {
                    move = black.engine.makeMove(board, 10000);
                }

                Console.WriteLine($"move {move} has just been made");
                board = board.makeMove(move);
                board.display();
                onChange?.Invoke(this, new ChessPlayerEvent(board));

            }
        }

        public class ChessPlayerEvent
        {
            public chess.Board board;

            public ChessPlayerEvent(chess.Board board)
            {
                this.board = board;
            }
        }
    }
}
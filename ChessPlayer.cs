using System.Diagnostics.Tracing;
using chess;
using gui;

namespace gui
{
    public class ChessPlayer
    {
        chess.Board? board;

        public EventHandler<ChessPlayerEvent>? onChange;
        public ChessPlayer()
        {
        }

        public void Play()
        {
            board = chess.Board.startPosition();
            onChange?.Invoke(this, new ChessPlayerEvent(board));
        }

        public void makeMove(Move move)
        {
            if (board == null) return;
            
            board = board.makeMove(move);
            onChange?.Invoke(this, new ChessPlayerEvent(board));
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
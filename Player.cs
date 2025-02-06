using chess;

namespace gui
{
    public class Player : chess.Engine
    {
        Move? selectedMove;
        public Player(bool isWhite, IEvaluator evaluator) : base(isWhite, evaluator)
        {

        }

        public override Move makeMove(chess.Board board)
        {
            while (selectedMove == null)
            {
                Thread.Sleep(100);
            }

            Move move = selectedMove;
            selectedMove = null;
            return move;
        }

        public override Move makeMove(chess.Board board, float maxTime)
        {
            return makeMove(board);
        }

        internal void OnMove(object? sender, MoveSelecter.MoveEvent e)
        {
            selectedMove = e.move;
        }

    }
}
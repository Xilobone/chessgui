using chess;

namespace gui
{   
    /// <summary>
    /// Represents a player that makes moves trough the gui
    /// </summary>
    public class Player : chess.engine.Engine
    {
        Move? selectedMove;

        /// <summary>
        /// Creates a new player
        /// </summary>
        /// <param name="isWhite">True if the player plays as white, false if it plays as black</param>
        public Player(bool isWhite) : base(isWhite, new player.Evaluator()) { }

        /// <summary>
        /// Gets the move that the player will make on the given board
        /// </summary>
        /// <param name="board">The board to make the move on</param>
        /// <returns>The move to be made on the board</returns>
        public override Move makeMove(chess.Board board)
        {
            while (selectedMove == null)
            {
                Thread.Sleep(100);
            }

            Console.WriteLine("making move");

            Move move = selectedMove;
            selectedMove = null;
            return move;
        }

        /// <summary>
        /// Gets the move that the player will make on the given board, within the allowed time,
        /// this method is equal to the makeMove() method without a time limit for players
        /// </summary>
        /// <param name="board">The board to make the move on</param>
        /// <param name="maxTime">The maximum allowed time to make a move</param>
        /// <returns>The move to make on the board</returns>
        public override Move makeMove(chess.Board board, float maxTime)
        {
            return makeMove(board);
        }

        internal void OnMove(object? sender, MoveSelecter.MoveEvent e)
        {
            if (e.isWhite == isWhite) selectedMove = e.move;
        }

    }
}
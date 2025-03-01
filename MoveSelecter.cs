
using chess;

namespace gui
{   
    /// <summary>
    /// Class that handles the selection of moves on the gui board, invokes the onMove event when a
    /// valid move is selected
    /// </summary>
    public class MoveSelecter
    {
        private GUI gui;

        private int selectedFrom = -1;
        private int selectedTo = -1;

        /// <summary>
        /// Gets invoked when a valid move is selected, includes the move that has been selected as well
        /// as the color of the pieces involved
        /// </summary>
        public EventHandler<MoveEvent>? onMove;

        /// <summary>
        /// Gets invoked when the selected indexes for moves changes
        /// </summary>
        public EventHandler<MoveSelectionEvent>? onMoveSelectionChange;

        /// <summary>
        /// Creates a new move selecter object
        /// </summary>
        /// <param name="gui">The gui window to detect move selection from</param>
        public MoveSelecter(GUI gui)
        {
            this.gui = gui;
            gui.MouseClick += OnMouseClick;
        }

        private void OnMouseClick(object? sender, MouseEventArgs e)
        {
            if (gui.board.board == null) return;

            int x = (e.Location.X - Board.BOARD_OFFSET[0]) / Board.SQUARE_SIZE;
            int y = 7 - (e.Location.Y - Board.BOARD_OFFSET[1]) / Board.SQUARE_SIZE;

            //do not do anything if pressed somewhere outside of the board
            if (x < 0 || x > 7 || y < 0 || y > 7) return;

            int index = x + 8 * y;

            //select this index if nothing selected previously
            if (selectedFrom == -1)
            {
                selectedFrom = index;
                onMoveSelectionChange?.Invoke(this, new MoveSelectionEvent(selectedFrom, selectedTo));
                return;
            }

            //deselect this index if it has already been selected
            if (index == selectedFrom)
            {
                selectedFrom = -1;
                selectedTo = -1;
                onMoveSelectionChange?.Invoke(this, new MoveSelectionEvent(selectedFrom, selectedTo));
                return;
            }

            //select this index as the to position
            selectedTo = index;
            onMoveSelectionChange?.Invoke(this, new MoveSelectionEvent(selectedFrom, selectedTo));


            List<Move> moves = MoveGenerator.generateMoves(gui.board.board, selectedFrom);

            foreach (Move move in moves)
            {
                if (move.to == selectedTo)
                {
                    bool isWhite = Piece.isWhite(gui.board.board.getPiece(move.fr));
                    onMove?.Invoke(this, new MoveEvent(move, isWhite));
                    selectedFrom = -1;
                    selectedTo = -1;
                    onMoveSelectionChange?.Invoke(this, new MoveSelectionEvent(selectedFrom, selectedTo));

                    return;
                }
            }
        }

        /// <summary>
        /// The event that gets passed when the onMove event is invoked
        /// </summary>
        public class MoveEvent
        {   
            /// <summary>
            /// The move that was selected to be made
            /// </summary>
            public Move move;

            /// <summary>
            /// True if the move was made with the white pieces, false if the move
            /// was made with the black pieces
            /// </summary>
            public bool isWhite;

            /// <summary>
            /// Creates a new move event object
            /// </summary>
            /// <param name="move">The move that was selected</param>
            /// <param name="isWhite">The color of the piece that was moved</param>
            public MoveEvent(Move move, bool isWhite)
            {
                this.move = move;
                this.isWhite = isWhite;
            }
        }

        /// <summary>
        /// The event that gets passed when the selected indexes are changed
        /// </summary>
        public class MoveSelectionEvent
        {
            /// <summary>
            /// The selected index to move from, -1 if no index has been selected
            /// </summary>
            public int fr;

            /// <summary>
            /// The selected index to move to, -1 if no index has been selected
            /// </summary>
            public int to;

            /// <summary>
            /// Creates a new move selection event object
            /// </summary>
            /// <param name="fr">The selected index to move from</param>
            /// <param name="to">The selected index to move to</param>
            public MoveSelectionEvent(int fr, int to)
            {
                this.fr = fr;
                this.to = to;
            }
        }
    }
}
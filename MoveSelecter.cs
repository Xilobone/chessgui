
using chess;

namespace gui
{
    public class MoveSelecter
    {
        private GUI gui;

        private int selectedFrom = -1;
        private int selectedTo = -1;

        public EventHandler<MoveEvent> onMove;
        public EventHandler<MoveSelectionEvent> onMoveSelectionChange;
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
                if (move.toIndex == selectedTo)
                {
                    onMove.Invoke(this, new MoveEvent(move));
                    selectedFrom = -1;
                    selectedTo = -1;
                    onMoveSelectionChange?.Invoke(this, new MoveSelectionEvent(selectedFrom, selectedTo));

                    return;
                }
            }
        }

        public class MoveEvent
        {
            public Move move;

            public MoveEvent(Move move)
            {
                this.move = move;
            }
        }

        public class MoveSelectionEvent
        {
            public int fr;
            public int to;

            public MoveSelectionEvent(int fr, int to)
            {
                this.fr = fr;
                this.to = to;
            }
        }
    }
}
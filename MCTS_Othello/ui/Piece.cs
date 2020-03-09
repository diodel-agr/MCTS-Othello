using MCTS_Othello.player;

namespace MCTS_Othello.ui
{
    /**
     * This class will reprezent a piece on the table.
     * It will hold its position and the owner.
     */
    class Piece
    {
        /* private members. */
        public int X { get; }
        public int Y { get; }
        public IMCTSPlayer owner { get;  set; }
        /* constructors. */
        public Piece()
        {
            X = 0;
            Y = 0;
            owner = null;
        }
        public Piece(int _x, int _y, IMCTSPlayer _owner)
        {
            X = _x;
            Y = _y;
            owner = _owner;
        }
        public Piece(Piece p)
        {
            X = p.X;
            Y = p.Y;
            owner = p.owner;
        }
        ~Piece()
        {
            owner = null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCTS_Othello.ui;

namespace MCTS_Othello.player
{
    class RandomComputer : IMCTSPlayer
    {
        /* fields. */
        Color color;
        PlayerType type;
        Board board;
        Random rand;

        /* constructors. */
        public RandomComputer()
        {
            type = PlayerType.computer;
            board = null;
            rand = new Random();
        }
        public RandomComputer(Color c) : this()
        {
            color = c;
        }
        /* methods */
        public Color GetColor()
        {
            return color;
        }

        public Piece MakeMove()
        {
            Piece result = null;
            List<Piece> myPieces = board.GetPlayerPieces(color);
            if (myPieces.Count != 0)
            {
                /* pick random a piece. */
                List<Piece> next = new List<Piece>();
                foreach (Piece pct in myPieces)
                {
                    List<Piece> ngh = board.GetValidMoves(pct);
                    next.AddRange(ngh);
                }
                if (next.Count != 0)
                {
                    Piece dest = next[rand.Next(next.Count)];
                    dest.owner = this;
                    result = new Piece(dest);
                }
            }
            return result;
        }
        public void SetBoard(Board b)
        {
            board = new ui.Board(b);
        }
    }
}

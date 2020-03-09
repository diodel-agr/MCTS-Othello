using MCTS_Othello.ui;
using System.Collections.Generic;

namespace MCTS_Othello.player.MCTS.expansion
{
    class SimpleExpansion : IExpansion
    {
        /* members. */
        Board board;

        /* constructors. */
        public SimpleExpansion() { }

        /* interface IExpansion methods. */
        public List<Node> Expand(Node node, Color nodeColor)
        {
            if (node.X != -1 && nodeColor == board.pieces[node.X, node.Y].owner.GetColor())
            {
                throw new MCTSException("Nu este bine!");
            }
            if (node.expanded == false)
            {
                List<Node> children = new List<Node>();
                node.expanded = true;
                /* obtain all posible moves. */
                int[,] freq = new int[8, 8];
                List<Piece> pcs = board.GetPlayerPieces(nodeColor);
                foreach (Piece p in pcs)
                {
                    foreach (Piece m in board.GetValidMoves(p))
                    {
                        if (freq[m.X, m.Y] == 0)
                        {
                            Node n = new Node(node, m.X, m.Y);
                            children.Add(n);
                            freq[m.X, m.Y]++;
                        }
                    }
                }
                if (children.Count == 0)
                {
                    node.expandable = false;
                    node.expanded = false;
                }
                freq = null;
                node.SetChildren(children);
            }
            board = null;
            return node.GetChildren();
        }

        public void SetBoard(Board b)
        {
            board = b;
        }
    }
}

using MCTS_Othello.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTS_Othello.player.MCTS.expansion
{
    interface IExpansion
    {
        void SetBoard(Board b);
        List<Node> Expand(Node node, Color nodeColor);
    }
}

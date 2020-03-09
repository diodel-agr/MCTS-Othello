using MCTS_Othello.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTS_Othello.player.MCTS.selection
{
    interface ISelection
    {
        Node Select(Node node);
    }
}

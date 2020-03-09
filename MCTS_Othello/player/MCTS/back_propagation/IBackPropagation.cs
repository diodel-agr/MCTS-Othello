using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTS_Othello.player.MCTS.back_propagation
{
    interface IBackPropagation
    {
        void BackPropagate(Node node);
        bool Ready();
    }
}

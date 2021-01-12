using MCTS_Othello.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTS_Othello.player
{
    /**
     * This interface specify the operations executed by players.
     */
    interface IMCTSPlayer
    {
        Piece MakeMove(); /* trb modifica sa intoarca numai Piece[1]. */
        Color GetColor();
        void SetBoard(Board b);
    }
}

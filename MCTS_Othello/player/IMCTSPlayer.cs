using MCTS_Othello.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTS_Othello.player
{
    /// <summary>
    /// This interface specifies the operations executed by players.
    /// </summary>
    interface IMCTSPlayer
    {
        Piece MakeMove(); /* trb modifica sa intoarca numai Piece[1]. */
        Color GetColor();
        void SetBoard(Board b);
    }
}

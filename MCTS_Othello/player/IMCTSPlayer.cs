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
        void Play(); /*facem ca game sa nu mai foloseasca asta. */
        void Play(Piece lastMove); /* facem game sa foloseasca asta iar bot-ul ca apela pe celalalt. */
        Color GetColor();
        void SetBoard(Board b);
    }
}

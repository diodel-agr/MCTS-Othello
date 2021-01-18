using System;
using MCTS_Othello.ui;

namespace MCTS_Othello.player
{
    enum PlayerType
    {
        human = 0,
        computer = 1
    }

    enum Color
    {
        black = 0,
        white = 1
    }

    class HumanPlayer : IMCTSPlayer
    {
        /* members. */
        private PlayerType playerType;
        private Color color;
        /* methods. */
        public HumanPlayer(Color color)
        {
            playerType = PlayerType.human;
            this.color = color;
        }
        public Piece MakeMove()
        {
            return null;
        }
        public Color GetColor()
        {
            return color;
        }

        public void SetBoard(Board b)
        {
            return;
        }

        public void StopFromPlaying(Board board)
        {
            return;
        }
    }
}

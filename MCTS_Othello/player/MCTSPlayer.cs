using MCTS_Othello.player.MCTS;
using MCTS_Othello.player.MCTS.selection;
using MCTS_Othello.player.MCTS.simulation;
using MCTS_Othello.ui;
using System;

namespace MCTS_Othello.player
{
    class MCTSPlayer : IMCTSPlayer
    {
        /* fields. */
        Color color;
        ISelection selection;
        ISimulation simulation;
        Random rand;
        string simulationType;
        string expansionType;
        string backPropagationType;

        /* constructors. */
        public MCTSPlayer(Color c, string sel, string exp, string sim, string bp)
        {
            color = c;
            selection = SelectionFactory.Create(sel);
            simulation = null;
            simulationType = sim;
            expansionType = exp;
            backPropagationType = bp;
            rand = new Random();
        }

        /* interface IMCTSPlayer methods. */
        public Piece MakeMove()
        {
            /* stop simulation. */
            Node simRes = simulation.GetSimulationResult(); /* BACK-PROPAGATE happens in this function. */
            /* select the best next move -> SELECTION. */
            Node bestNode = selection.Select(simRes);
            Piece bestPiece = new Piece(bestNode.X, bestNode.Y, this);
            /* advance game state (add a node to the tree) -> EXPANSION and resume SIMULATION. */
            //simulation.StartSimulation(bestPiece);
            /* return result. */
            return bestPiece;
        }
        public Color GetColor()
        {
            return color;
        }

        public void SetBoard(Board b)
        {
            if (simulation == null)
            {
                Board board = new Board(b);
                IMCTSPlayer opponent = null;
                /* find the opponent. */
                foreach (Piece p in b.pieces)
                {
                    if (p != null && p.owner != this)
                    {
                        opponent = p.owner;
                        break;
                    }
                }
                if (opponent == null)
                {
                    throw new MCTSException("[MCTSPlayer::SetBoard] - Could not found the opponent!");
                }
                simulation = SimulationFactory.Create(simulationType, this, opponent, board, expansionType, backPropagationType);
                simulation.StartSimulation();
            }
            else
            {
                simulation.SetBoard(b);
            }
        }
    }
}

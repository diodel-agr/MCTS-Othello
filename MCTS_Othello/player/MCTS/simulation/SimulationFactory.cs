using MCTS_Othello.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTS_Othello.player.MCTS.simulation
{
    class SimulationFactory
    {
        public static ISimulation Create(string simulationType, IMCTSPlayer player, 
            IMCTSPlayer opponent, Board board, string expansionType, string backPropagationType)
        {
            switch (simulationType)
            {
                case "random_simulation":
                    return new RandomSimulation(player, opponent, board, expansionType, backPropagationType);
                default:
                    throw new MCTSException("[SimulationFactory::Create] Unknown simulation type '" + simulationType + "'");
            }
        }
    }
}

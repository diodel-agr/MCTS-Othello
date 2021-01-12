using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTS_Othello.player.MCTS.back_propagation
{
    class BackPropagationFactory
    {
        public static IBackPropagation Create(string backPropagationType)
        {
            switch (backPropagationType)
            {
                case "simple_bp":
                    return new SimpleBackPropagation();
                default:
                    throw new MCTSException("[RandomSimulation constructor] - invalid back propagation type.");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTS_Othello.player
{
    class PlayerFactory
    {
        public static IMCTSPlayer Create(string playerType, Color color, string sel, string exp, string sim, string bp)
        {
            switch (playerType)
            {
                case "MCTS":
                    return new MCTSPlayer(color, "simple_selection", "simple_expansion", "random_simulation", "simple_bp");
                case "Random":
                    return new RandomComputer(color);
                case "Human":
                    return new HumanPlayer(color);
                default:
                    throw new MCTSException("[PlayerFactory::IMCTSPlayer] Unknown bot type '" + playerType + "'");
            }
        }
    }
}

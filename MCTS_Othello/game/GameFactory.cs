using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTS_Othello.game
{
    class GameFactory<T>
    {
        public static IMCTSGame<T> Create(int gameType, string typeBot1, string typeBot2)
        {
            switch (gameType)
            {
                case 0: // "Human vs. Human":
                    return new HHGame<T>();
                case 1: // "Human vs. Computer":
                    return new HCGame<T>(typeBot1);
                case 2: // "Computer vs. Computer":
                    return new CCGame<T>(typeBot1, typeBot2);
                default:
                    throw new MCTSException("[GameFactory::Create] Unknown type '" + gameType + "'");
            }

        }
    }
}

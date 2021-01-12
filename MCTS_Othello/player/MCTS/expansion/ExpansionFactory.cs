using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTS_Othello.player.MCTS.expansion
{
    class ExpansionFactory
    {
        public static IExpansion Create(string expansionType)
        {
            switch (expansionType)
            {
                case "simple_expansion":
                    return new SimpleExpansion();
                default:
                    throw new MCTSException("[RandomSimulation constructor] - invalid expantion type.");
            }
        }
    }
}

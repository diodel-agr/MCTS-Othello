using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTS_Othello.player.MCTS.selection
{
    class SelectionFactory
    {
        public static ISelection Create(string selectionType)
        {
            switch (selectionType)
            {
                case "simple_selection":
                    return new SimpleSelection();
                default:
                    throw new MCTSException("[SelectionFactory::Create] Unknown selection type '" + selectionType + "'");
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace MCTS_Othello.player.MCTS.selection
{
    class SimpleSelection : ISelection
    {
        /* members. */
        Random rand;
        double C = 2.0;
        /* constructors. */
        public SimpleSelection()
        {
            rand = new Random();
        }

        /* interface ISelection methods. */
        public Node Select(Node root)
        {
            /* select nodes that have the same score. */
            List<Node> bestChild = new List<Node>();
            List<Node> children = root.GetChildren();
            if (children == null || children.Count == 0)
            {
                throw new MCTSException("[SimpleSelection/Select()] - node has 0 children.");
            }
            double bestScore = ComputeScore(root, children[0].GetWins(), children[0].GetVisits());
            foreach (Node ch in children)
            {
                double chScore = ComputeScore(root, ch.GetWins(), ch.GetVisits());
                if (chScore > bestScore)
                {
                    bestScore = chScore;
                    bestChild.Clear();
                    bestChild.Add(ch);
                }
                else if (chScore == bestScore)
                {
                    bestChild.Add(ch);
                }
            }
            /* select one best child. */
            if (bestChild.Count == 0)
            {
                throw new MCTSException("[SimpleSelection/Select()] - best child not found!");
            }
            return bestChild[rand.Next(bestChild.Count)];
        }
        /* methods. */
        private double ComputeScore(Node root, int wins, int visits)
        {
            if (visits == 0)
            {
                return visits;
            }
            else
            {
                return ((double)wins) / visits + Math.Sqrt((C * Math.Log((double)root.GetVisits()) / visits));
            }
        }
    }
}
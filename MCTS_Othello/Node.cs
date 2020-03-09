using System.Collections.Generic;
using System.Threading;

namespace MCTS_Othello
{
    /**
     * This class represents a node of the tree constructed by the bot.
     * A node is a posible state in the game (a table configuration).
     */
    class Node
    {
        /*** members. ***/
        public bool expandable { get; set; }
        public bool expanded { get; set; }
        private int wins;
        private int visits;
        public Node parent { get; }
        List<Node> children; /* the list of all possible ways to expand the tree. */
        /* the position of the cooresponding piece for this node. */
        public int X { get; }
        public int Y { get; }
        Mutex mutex;
        /* methods. */
        public Node()
        {
            expandable = true;
            expanded = false;
            wins = 0;
            visits = 0;
            parent = null;
            children = null;
            X = -1; Y = -1;
            mutex = new Mutex();
        }
        public Node(int x, int y) : this()
        {
            X = x;
            Y = y;
        }
        public Node(Node parent, int x, int y) : this()
        {
            this.parent = parent;
            X = x;
            Y = y;
        }
        public List<Node> GetChildren()
        {
            return children;
        }
        public void SetChildren(List<Node> chl)
        {
            children = chl;
        }
        public void IncWins(int amount)
        {
            mutex.WaitOne();
            wins += amount;
            mutex.ReleaseMutex();
        }
        public void IncVisits(int amount)
        {
            mutex.WaitOne();
            visits += amount;
            mutex.ReleaseMutex();
        }
        public int GetWins()
        {
            return wins;
        }
        public int GetVisits()
        {
            return visits;
        }
    }
}

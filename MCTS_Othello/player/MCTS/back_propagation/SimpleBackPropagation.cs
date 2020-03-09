using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MCTS_Othello.player.MCTS.back_propagation
{
    class SimpleBackPropagation : IBackPropagation
    {
        /* fields. */
        Thread thread;
        List<Node> nodeList;
        List<int> winsList;
        List<int> visitsList;
        Mutex mutex;
        /* constructors. */
        public SimpleBackPropagation()
        {
            thread = new Thread(new ThreadStart(ThreadPropagate));
            nodeList = new List<Node>();
            winsList = new List<int>();
            visitsList = new List<int>();
            mutex = new Mutex();
        }

        /* interface methods. */
        public void BackPropagate(Node node)
        {
            mutex.WaitOne();
            nodeList.Add(node);
            winsList.Add(node.GetWins());
            visitsList.Add(node.GetVisits());
            mutex.ReleaseMutex();
            if (thread.ThreadState == ThreadState.Stopped)
            {
                thread.Start();
            }
        }

        public bool Ready()
        {
            return (thread.ThreadState == ThreadState.Stopped);
        }

        /* methods. */
        private void ThreadPropagate()
        {
            while (nodeList.Count != 0)
            {
                mutex.WaitOne();
                Node n = nodeList.ElementAt(0);
                nodeList.RemoveAt(0);
                int win = winsList.ElementAt(0);
                winsList.RemoveAt(0);
                int vis = visitsList.ElementAt(0);
                visitsList.RemoveAt(0);
                mutex.ReleaseMutex();
                n = n.parent;
                while (n != null)
                {
                    n.IncWins(win);
                    n.IncVisits(vis);
                    n = n.parent;
                }
            }
        }
    }
}

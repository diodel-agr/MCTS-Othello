using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTS_Othello
{
    [Serializable]
    class MCTSException : System.Exception
    {
        public MCTSException(string msg) : base()
        {
            Console.WriteLine("MCTS Exception: " + msg);
        }
    }
}

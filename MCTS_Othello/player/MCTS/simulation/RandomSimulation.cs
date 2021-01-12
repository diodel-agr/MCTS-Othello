using System;
using System.Collections.Generic;
using MCTS_Othello.ui;
using System.Threading;
using MCTS_Othello.player.MCTS.expansion;
using MCTS_Othello.player.MCTS.back_propagation;
using System.IO;

namespace MCTS_Othello.player.MCTS.simulation
{
    class RandomSimulation : ISimulation
    {
        /* members. */
        Board board;
        Node root;
        Node treeRoot;
        IMCTSPlayer thisPlayer;
        IMCTSPlayer opponent;
        int wins, visits;
        IExpansion exp;
        IBackPropagation bp;
        Random rand;
        List<Thread> workers;
        Mutex mutex;
        CancellationTokenSource cancelToken;
        int threadNo = 1;
        bool isSimulating;

        StreamWriter logger;
        /* constructors. */
        public RandomSimulation(IMCTSPlayer thisPlayer, IMCTSPlayer opponent, Board b, string expansion, string backPropagation)
        {
            board = new Board(b);
            this.thisPlayer = thisPlayer;
            this.opponent = opponent;
            wins = visits = 0;
            exp = ExpansionFactory.Create(expansion);
            bp = BackPropagationFactory.Create(backPropagation);
            rand = new Random();
            workers = new List<Thread>(threadNo);
            mutex = new Mutex();
            isSimulating = false;
            root = null;
            treeRoot = null;
        }

        /* interface methods. */
        /**
         * GetSimulationResult - stops the threads that simulate and 
         * returns the node on which the simulation was executing.
         */
        public Node GetSimulationResult()
        {
            /* stop threads. */
            cancelToken.Cancel();
            foreach (Thread th in workers)
            {
                th.Join();
            }
            workers.Clear();
            isSimulating = false;
            /* back propagate node statistics. */
            bp.BackPropagate(root); /* BACK - PROPAGATION. */
            wins = 0;
            visits = 0;
            /* return root. */
            return root;
        }
        /**
         * StartSimulation() - depanding on who will make the next move, the simulation strategy differs
         * 
         * If the current player has to move, will simulate on the current state.
         * If the opponent has to move, will simulate on some / all of the current node children.  
         */
        public void StartSimulation()
        {
            /* print current board. */
            //board.PrintBoard();
            if (root == null)
            {   /* executed only at the beginning of the game. */
                root = new Node(-1, -1);
                treeRoot = root;
                exp.SetBoard(board);
                exp.Expand(root, Color.black);
            }
            else if (root.expandable == true && root.expanded == false)
            {
                exp.SetBoard(board);
                exp.Expand(root, GetColor(board, root));
            }
            /* start simulation. */
            //if (root.X == -1)
            //{
            //    Console.WriteLine("Simuleaza pentru black.");
            //}
            //else
            //{
            //    Console.WriteLine("Simuleaza pentru " + SwitchColor(board.pieces[root.X, root.Y].owner.GetColor()));
            //}
            cancelToken = new CancellationTokenSource();
            for (int i = 0; i < threadNo; ++i)
            {
                Board auxBoard = new Board(board);
                Thread th = new Thread(new ParameterizedThreadStart(ThreadSim));
                th.Start(new object[] { cancelToken, auxBoard, root });
                workers.Add(th);
            }
            isSimulating = true;
            //Console.WriteLine("Started " + threadNo + " threads.");
        }
        /**
         * StartSimulation(p)
         * 
         * updates the current game state according to the last piece placed on the map.
         * Usually called after opponents move.
         */
        public void StartSimulation(Piece p)
        {
            if (isSimulating == true)
            {
                GetSimulationResult();
            }
            isSimulating = false;
            /* find chosen node and update state and MCTSPlayer board. */
            bool found = false;
            foreach (Node n in root.GetChildren())
            {
                if (n.X == p.X && n.Y == p.Y)
                {
                    root = n;
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                throw new MCTSException("[RandomSimulation/StartSimulation(Piece)] - child node not found!");
            }
            //PlacePieceOnBoard(board, p);
            /* start simulation. */
            StartSimulation();
        }
        /**
         * SetBoard(Board) - Set board and advance game state.
         * 
         * @b - board to set.
         * 
         **/
        public void SetBoard(Board b)
        {
            if (isSimulating == true)
            {
                GetSimulationResult();
                isSimulating = false;
            }
            Piece newPiece = null;
            int single = 0;
            foreach (Piece p in b.pieces)
            {
                if (p != null && board.pieces[p.X, p.Y] == null)
                {
                    newPiece = p;
                    single++;
                }
            }
            if (single == 0)
            {
                //throw new MCTSException("No difference found!");
            }
            else if (single > 1)
            {
                throw new MCTSException("Too many differences found!");
            }
            else
            {   /* update game state. */
                bool found = false;
                foreach (Node ch in root.GetChildren()) // when here a null pointer dereference is thrown, the game is finished, probably the bot won.
                {
                    if (ch.X == newPiece.X && ch.Y == newPiece.Y)
                    {
                        root = ch;
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                    throw new MCTSException("Next state not found!");
                }
            }
            board.FreePieces();
            board = new Board(b);
            GC.Collect();
            //board.PrintBoard();
            StartSimulation();
            isSimulating = true;
        }
        /* methods. */
        private void ThreadSim(object obj)
        {
            //Console.WriteLine("Thread started simulation.");
            object[] objArr = (object[])obj;
            CancellationTokenSource token = (CancellationTokenSource)(objArr[0]);
            Board b = (Board)(objArr[1]);
            Node cn = (Node)(objArr[2]);
            /* choose simulation color. */
            Color rootColor = Color.black;
            if (cn.X != -1 && b.pieces[cn.X, cn.Y].owner.GetColor() == Color.black)
            {
                rootColor = Color.white;
            }
            IExpansion expansion = new SimpleExpansion();
            int win = 0;
            int sim = 0;
            while (token.IsCancellationRequested == false)
            {
                //Console.WriteLine("score: " + b.GetScore(1) + " : " + b.GetScore(2));
                int res = Simulate(token, new Board(b), cn, expansion, rootColor);
                if (res == -2)
                {   /* received cancel request while simulating, discard this result. */
                    break;
                }
                else
                {
                    win += res;
                    sim++;
                }
            }
            //File.WriteAllText("output.txt", "Score " + b.GetScore(1) + ":" + b.GetScore(2) + "||" + win + " wins and " + sim + " simulations.");
            Console.WriteLine("Score " + b.GetScore(1) + ":" + b.GetScore(2) + "||" + win + " wins and " + sim + " simulations.");
            //using (StreamWriter sw = File.CreateText("output.txt"))
            //{
            //    sw.WriteLine(win + " wins and " + sim + " simulations.");
            //}
            //logger.WriteLine(win + " wins and " + sim + " simulations.");
            /* update node statistics. */
            mutex.WaitOne();
            wins += win;
            visits += sim;
            mutex.ReleaseMutex();
            /* free memory. */
            b.FreePieces();
            b = null;
        }
        /// <summary>
        /// This method executes the simulation of a game. It decides the statistics (wins and simulations)
        /// for a certain starting node. Based on the current board and the current player, chooses the next move
        /// until the end of the game. If the current player (the bot) wins, it returns 1 (1 win) and updates 
        /// (back-propagates the results) back in the recursion stack.
        /// </summary>
        /// <param name="token">Cancellation token used to stop the execution of the thread executing this function.</param>
        /// <param name="b">The current board.</param>
        /// <param name="n">The current node, representing the board configuration.</param>
        /// <param name="expansion">Object representing the expansion strategy.</param>
        /// <param name="expandColor">The color of the next player.</param>
        /// <returns></returns>
        private int Simulate(CancellationTokenSource token, Board b, Node n, IExpansion expansion, Color expandColor)
        {
            //Console.WriteLine("Expand " + expandColor.ToString());
            int res = 0;
            if (token.IsCancellationRequested == false)
            {
                /* check if game is finished. */
                if (b.IsFinished(thisPlayer) == true || b.IsFinished(opponent) == true || (b.GetScore(1) + b.GetScore(2) == 64)
                    || b.PlayerHasPossibleMoves(thisPlayer) == false || b.PlayerHasPossibleMoves(opponent) == false)
                {   /* set this node as terminal. */
                    n.expandable = false;
                    /* find the game result. */
                    if (b.GetScore(1) == b.GetScore(2))
                    {
                        res = 0;
                    }
                    else if (b.GetScore(1) > b.GetScore(2))
                    {   /* player 1(black) won. */
                        res = (thisPlayer.GetColor() == Color.black) ? 1 : -1;
                    }
                    else
                    {   /* player 2(white) won. */
                        res = (thisPlayer.GetColor() == Color.black) ? -1 : 1;
                    }
                }
                else
                {   /* continue simulating. */
                    /* expand, if necessary. */
                    expansion.SetBoard(b);
                    List<Node> chl = expansion.Expand(n, expandColor);
                    expandColor = SwitchColor(expandColor);
                    /* choose random child. */
                    Node nextNode = chl[rand.Next(chl.Count)];
                    /* update board. */
                    Piece pct = new Piece(nextNode.X, nextNode.Y, GetOwner(b, n));
                    if (b.pieces[nextNode.X, nextNode.Y] != null)
                    {
                        throw new MCTSException("next node is not null!");
                    }
                    PlacePieceOnBoard(b, pct);
                    /* simulate. */
                    res = Simulate(token, b, nextNode, expansion, expandColor);
                    /* update node statistics if cancellation is not set. */
                    if (res != -2)
                    {
                        n.IncWins(res); /* res could be -1, 0 or 1. */
                        n.IncVisits(1);
                    }
                }
            }
            else
            {   /* cancel request while simulating. */
                res = -2;
            }
            return res; /* return result. */
        }
        /// <summary>
        /// This method returns the color of the current player based on the current node.
        /// </summary>
        /// <param name="b">Current board.</param>
        /// <param name="n">current node representing the state of the board.</param>
        /// <returns></returns>
        private Color GetColor(Board b, Node n)
        {
            Color color;
            if (n.X == -1 || n.Y == -1)
            {
                color = Color.black;
            }
            else if (b.pieces[n.X, n.Y].owner == thisPlayer)
            {   /* this player moved last. it's opponent's turn. */
                color =  opponent.GetColor();
            }
            else
            {   /* opponent moved last. this player's turn. */
                color = thisPlayer.GetColor();
            }
            return color;
        }
        private IMCTSPlayer GetOwner(Board b, Node n)
        {
            IMCTSPlayer result = null;
            if (n.X == -1)
            {
                result = opponent;
                if (thisPlayer.GetColor() == Color.black)
                {
                    result = thisPlayer;
                }
            }
            else if (b.pieces[n.X, n.Y].owner == thisPlayer)
            {
                result = opponent;
            }
            else
            {
                result =  thisPlayer;
            }
            return result;
        }
        private void PlacePieceOnBoard(Board b, Piece p)
        {
            b.AddPiece(p);
            foreach (Piece n in b.GetPieceNeighbors(p))
            {
                b.AddPiece(p, n);
            }
        }
        private Color SwitchColor(Color c)
        {
            return (c == Color.black) ? Color.white : Color.black;
        }
    }
}
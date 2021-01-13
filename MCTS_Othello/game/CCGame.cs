using MCTS_Othello.player;
using MCTS_Othello.ui;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MCTS_Othello.game
{
    class CCGame<T> : IMCTSGame<T>
    {
        /* members. */
        Board board;
        IMCTSPlayer player1;
        IMCTSPlayer player2;
        IMCTSPlayer currentPlayer;
        Piece lastMove;
        GameState state;
        Mutex pieceMutex;
        CancellationTokenSource cancelToken; // token used to stop the worker thread.
        Thread botThread;
        /* constructors. */
        public CCGame(string bot1, string bot2)
        {
            cancelToken = new CancellationTokenSource();
            player1 = PlayerFactory.Create(bot1, Color.black, "simple_selection", "simple_expansion", "random_simulation", "simple_bp");
            player2 = PlayerFactory.Create(bot2, Color.white, "simple_selection", "simple_expansion", "random_simulation", "simple_bp");
            pieceMutex = new Mutex();
        }
        /* methods. */
        public void Start()
        {
            InitBoard();
            /* init score. */
            board.SetScore(1, 0);
            board.SetScore(2, 0);
            /* init player tiles. */
            Piece p1 = new Piece(4, 3, player1);
            Piece p2 = new Piece(3, 4, player1);
            Piece p3 = new Piece(3, 3, player2);
            Piece p4 = new Piece(4, 4, player2);
            board.AddPiece(p1);
            board.AddPiece(p2);
            board.AddPiece(p3);
            board.AddPiece(p4);
            state = GameState.waitPlayer;
            currentPlayer = player1;
            /* set bots boards. */
            player1.SetBoard(board);
            player2.SetBoard(board);
            /* launch the thread that will let the 2 bots to play. */
            botThread = new Thread(new ParameterizedThreadStart(BotPlay));
            botThread.Start(cancelToken);
        }

        public void InitBoard()
        {
            board = new Board(8);
        }

        public void RestartGame()
        {
            // stop the thread.
            cancelToken.Cancel();
            // wait for the worker to stop.
            botThread.Join();
            // start another game.
            Start();
        }

        public Piece[,] GetPieces()
        {
            /* obtain lock. */
            pieceMutex.WaitOne();
            Piece[,] res = new Piece[board.size, board.size];
            for (int i = 0; i < board.size; ++i)
            {
                for (int j = 0; j < board.size; ++j)
                {
                    res[i, j] = null;
                    if (board.pieces[i, j] != null)
                    {
                        res[i, j] = new Piece(board.pieces[i, j]);
                    }
                }
            }
            /* release mutex. */
            pieceMutex.ReleaseMutex();
            return res;
        }

        public IMCTSPlayer GetCurrentPlayer()
        {
            return currentPlayer;
        }

        public IMCTSPlayer GetWinner()
        {
            IMCTSPlayer winner = null;
            if (board.GetScore(1) > board.GetScore(2))
            {
                winner =  player1;
            }
            else
            {
                winner =  player2;
            }
            return winner;
        }

        public bool PlayerClicked(int x, int y)
        {
            throw new NotImplementedException();
        }

        public List<Piece> GetOptions()
        {
            return new List<Piece>();
        }

        public int GetScore(int player)
        {
            return board.GetScore(player);
        }

        public bool PlayerHasPossibleMoves()
        {
            return board.PlayerHasPossibleMoves(currentPlayer);
        }

        public void SetGameState(GameState state)
        {
            this.state = state;
        }

        public IMCTSPlayer GetPlayer(int player)
        {
            if (player == 1)
            {
                return player1;
            }
            else
            {
                return player2;
            }
        }

        public bool IsFinished()
        {
            return board.IsFinished(currentPlayer);
        }

        public string GetGameResult()
        {
            string result;
            if (GetScore(1) + GetScore(2) == 64)
            {
                if (GetScore(1) == GetScore(2))
                {
                    result = "Tie!";
                }
                else if (GetScore(1) > GetScore(2))
                {
                    result = "Black is the winner!";
                }
                else
                {
                    result = "White is the winner!";
                }
            }
            else
            {
                player.Color loser = GetWinner().GetColor();
                player.Color winner = player.Color.black;
                if (loser == player.Color.black)
                {
                    winner = player.Color.white;
                }
                result = loser.ToString() + " cannot do any moves! " + winner.ToString() + " won the game!";
            }
            return result;
        }
        
        private void BotPlay(object token)
        {
            Console.WriteLine("Game thread started!");
            CancellationTokenSource cancelToken = (CancellationTokenSource)token;
            lastMove = null;
            while (board.IsFinished(currentPlayer) == false && state != GameState.stopped 
                && cancelToken.IsCancellationRequested != true)
            {
                /* let the current player play. */
                currentPlayer.SetBoard(board);
                Thread.Sleep(1000);
                /* obtain move. */
                Piece move = currentPlayer.MakeMove();
                /* print new move. */
                //Console.WriteLine("Player:" + currentPlayer.GetColor().ToString() + " moved " + move.X + ":" + move.Y);
                /* modify board. */
                pieceMutex.WaitOne();
                board.AddPiece(move);
                List<Piece> neigh = board.GetPieceNeighbors(move);
                foreach (Piece n in neigh)
                {
                    board.AddPiece(move, n);
                }
                lastMove = move;
                /* release board mutex. */
                pieceMutex.ReleaseMutex(); 
                /**/
                if (currentPlayer.GetType() == typeof(MCTSPlayer))
                {
                    currentPlayer.SetBoard(board);
                }
                /* set next player. */
                if (currentPlayer == player1)
                {
                    currentPlayer = player2;
                }
                else
                {
                    currentPlayer = player1;
                }
                GC.Collect();
            }
            state = GameState.stopped;
            Console.WriteLine("Game thread exit!");
        }
        /// <summary>
        /// Method used by the observers to register to this observable.
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<T> observer)
        {
            return null;
        }
    }
}

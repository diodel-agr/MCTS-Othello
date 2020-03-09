using MCTS_Othello.player;
using MCTS_Othello.ui;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MCTS_Othello.game
{
    class CCGame : IMCTSGame
    {
        /* members. */
        Board board;
        IMCTSPlayer player1;
        IMCTSPlayer player2;
        IMCTSPlayer currentPlayer;
        Piece lastMove;
        GameState state;
        Mutex pieceMutex;
        /* constructors. */
        public CCGame(string bot1, string bot2)
        {
            switch (bot1)
            {
                case "MCTS":
                    player1 = new MCTSPlayer(Color.black, "simple_selection", "simple_expansion", "random_simulation", "simple_bp");
                    break;
                case "Random":
                    player1 = new RandomComputer(Color.black);
                    break;
                default:
                    player1 = null;
                    break;
            }
            switch (bot2)
            {
                case "MCTS":
                    player2 = new MCTSPlayer(Color.white, "simple_selection", "simple_expansion", "random_simulation", "simple_bp");
                    break;
                case "Random":
                    player2 = new RandomComputer(Color.white);
                    break;
                default:
                    player2 = null;
                    break;
            }
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
            player1.Play(null);
            player2.SetBoard(board);
            player2.Play(null);
            /* launch the thread that will let the 2 bots to play. */
            Thread botThread = new Thread(new ThreadStart(BotPlay));
            botThread.Start();
        }

        public void InitBoard()
        {
            board = new Board(8);
        }

        public void RestartGame()
        {
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

        public void PlayerClicked(int x, int y)
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
        
        private void BotPlay()
        {
            Console.WriteLine("Game thread started!");
            lastMove = null;
            while (board.IsFinished(currentPlayer) == false && state != GameState.stopped)
            {
                /* let the current player play. */
                currentPlayer.SetBoard(board);
                currentPlayer.Play(lastMove);
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
    }
}

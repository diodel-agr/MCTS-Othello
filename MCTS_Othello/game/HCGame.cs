﻿using System;
using System.Collections.Generic;
using MCTS_Othello.player;
using MCTS_Othello.ui;
using System.Threading;

namespace MCTS_Othello.game
{
    class HCGame<T> : IMCTSGame<T>
    {
        /* static members. */
        private static int BOT_WAIT_TIMEOUT = 500;

        /* members. */
        Board board;
        IMCTSPlayer player1;
        IMCTSPlayer bot;
        IMCTSPlayer currentPlayer;
        Piece clickedTile;
        /// <summary>
        /// List with the current valid moves for a clicked tile.
        /// </summary>
        List<Piece> optionList;
        GameState state;
        /* The list of observers. */
        private List<IObserver<T>> observers;

        /* constructors. */
        public HCGame()
        {
            board = null;
            player1 = bot = currentPlayer = null;
            clickedTile = null;
            optionList = null;
            state = GameState.stopped;
            observers = new List<IObserver<T>>();
        }

        public HCGame(string comp) : this()
        {
            player1 = new HumanPlayer(Color.black);
            switch (comp)
            {
                case "MCTS":
                    bot = new MCTSPlayer(Color.white, "simple_selection", "simple_expansion", "random_simulation", "simple_bp");
                    break;
                case "Random":
                    bot = new RandomComputer(Color.white);
                    break;
                default:
                    throw new MCTSException("Unknown bot type!");
            }
        }
        /* methods. */
        public IMCTSPlayer GetCurrentPlayer()
        {
            return currentPlayer;
        }

        public List<Piece> GetOptions()
        {
            return optionList;
        }

        public Piece[,] GetPieces()
        {
            return board.pieces;
        }

        public IMCTSPlayer GetPlayer(int player)
        {
            if (player == 1)
            {
                return player1;
            }
            else
            {
                return bot;
            }
        }

        public int GetScore(int player)
        {
            return board.GetScore(player);
        }

        public IMCTSPlayer GetWinner()
        {
            if (currentPlayer == player1)
            {
                return bot;
            }
            else
            {
                return player1;
            }
        }

        public void InitBoard()
        {
            board = new Board(8);
        }

        public void PlayerClicked(int x, int y)
        {
            /* see if the tile corresponds to the current player. */
            Piece p = board.pieces[x, y];
            if (p != null)
            {
                if (p.owner == player1 && (state == GameState.waitPlayer || state == GameState.tileClicked))
                {
                    /* construct and show possible moves. */
                    clickedTile = p;
                    optionList = board.GetValidMoves(p);
                    state = GameState.tileClicked;
                }
            }
            else if (state == GameState.tileClicked)
            {
                /* check if tile is in the option list. */
                bool isInList = false;
                foreach (Piece opt in optionList)
                {
                    if (opt.X == x && opt.Y == y)
                    {
                        isInList = true;
                        break;
                    }
                }
                if (isInList == true)
                {
                    /* add tile to the board. */
                    Piece newPiece = new ui.Piece(x, y, player1);
                    board.AddPiece(newPiece);
                    foreach (Piece n in board.GetPieceNeighbors(newPiece))
                    {
                        board.AddPiece(newPiece, n);
                    }
                    optionList = null;
                    //board.PrintBoard();
                    /* let bot play. */
                    currentPlayer = bot;
                    bot.SetBoard(board);
                    bot.Play(newPiece); /* newPiece is the piece placed by the human. */
                    /* launch thread to get move from bot. */
                    //Thread botThread = new Thread(new ThreadStart(BotThread));
                    //botThread.Start();
                    //botThread.Join();
                }
            }
        }

        public bool PlayerHasPossibleMoves()
        {
            return board.PlayerHasPossibleMoves(currentPlayer);
        }

        public void RestartGame()
        {
            Start();
        }

        public void SetGameState(GameState state)
        {
            this.state = state;
        }

        public void Start()
        {
            InitBoard();
            /* init score. */
            board.SetScore(1, 0);
            board.SetScore(2, 0);
            /* init player tiles. */
            Piece p1 = new Piece(4, 3, player1);
            Piece p2 = new Piece(3, 4, player1);
            Piece p3 = new Piece(3, 3, bot);
            Piece p4 = new Piece(4, 4, bot);
            board.AddPiece(p1);
            board.AddPiece(p2);
            board.AddPiece(p3);
            board.AddPiece(p4);
            state = GameState.waitPlayer;
            currentPlayer = player1;
            /* set bot board. */
            bot.SetBoard(board);
            bot.Play(null);
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

        /// <summary>
        /// Function executed by the bot thread.
        /// </summary>
        private void BotThread()
        {
            //Console.WriteLine("Bot thread started.");
            /* do bot things. */
            Thread.Sleep(BOT_WAIT_TIMEOUT);
            /* get move from bot. */
            Piece piece = bot.MakeMove();
            if (piece == null)
            {
                throw new MCTSException("[HCGame/PlayerClicked()] - Bot returned a null move.");
            }
            Console.WriteLine("Bot chose piece:" + piece.X + ": " + piece.Y);
            board.AddPiece(piece);
            List<Piece> neigh = board.GetPieceNeighbors(piece);
            foreach (Piece n in neigh)
            {
                board.AddPiece(piece, n);
            }
            //board.PrintBoard();
            bot.SetBoard(board);
            bot.Play();
            // find a way to get rid of this ugly call to garbage collector.
            GC.Collect();
            /* set next player. */
            currentPlayer = player1;
            /* set game state. */
            state = GameState.waitPlayer;
            /* update ui. */
            T x = default(T);
            Refresh(x);
        }
        /** Method for the IObservable interface. **/
        /// <summary>
        /// Method used by the observers to register to this observable.
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<T> observer)
        {
            // add the observer to the observers list.
            observers.Add(observer);
            // start the thread to get the move from the bot and update the UI.
            Thread botThread = new Thread(new ThreadStart(BotThread));
            botThread.Start();
            botThread.Join();
            return new Unsubscriber<T>(observers, observer);
        }
        /// <summary>
        /// Method used to call the 'UpdateUI' method.
        /// </summary>
        /// <param name="value"></param>
        public void Refresh(T value)
        {
            foreach (var obs in observers)
            {
                obs.OnNext(value);
            }
        }
        /// <summary>
        /// Method used by the observers to unsubscribe from this observable.
        /// </summary>
        public void EndSubscription()
        {
            foreach (var observer in observers.ToArray())
            {
                if (observers.Contains(observer))
                {
                    observer.OnCompleted();
                }
            }
            observers.Clear();
        }
        /// <summary>
        /// IDisposable object returned to the Observer (Form).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class Unsubscriber<T> : IDisposable
        {
            private List<IObserver<T>> _observers;
            private IObserver<T> _observer;

            public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}

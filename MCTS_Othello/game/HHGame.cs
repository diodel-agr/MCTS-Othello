using MCTS_Othello.player;
using MCTS_Othello.ui;
using System.Collections.Generic;
using System;

namespace MCTS_Othello.game
{
    /// <summary>
    /// Enum which encodes the different states of a game.
    /// </summary>
    enum GameState
    {
        stopped = 0,
        waitPlayer = 1,
        tileClicked = 2,
        waitBot = 3
    }

    /// <summary>
    /// Class which modelates a Human vs Human game.
    /// </summary>
    class HHGame : IMCTSGame
    {
        /* members. */
        Board board;
        IMCTSPlayer player1;
        IMCTSPlayer player2;
        IMCTSPlayer currentPlayer;
        Piece clickedTile;
        List<Piece> optionList;
        GameState state;
        public HHGame()
        {
            board = null;
            player1 = player2 = currentPlayer = null;
            optionList = null;
            state = GameState.stopped;
        }
        /* methods implemetation. */
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
        }
        public void InitBoard()
        {
            player1 = new HumanPlayer(Color.black);
            player2 = new HumanPlayer(Color.white);
            board = new Board(8);
        }
        public void RestartGame()
        {
            Start();
        }

        public Piece[,] GetPieces()
        {
            return board.pieces;
        }
        public IMCTSPlayer GetCurrentPlayer()
        {
            return currentPlayer;
        }
        public IMCTSPlayer GetWinner()
        {
            if (currentPlayer == player1)
            {
                return player2;
            }
            else
            {
                return player1;
            }
        }
        public void PlayerClicked(int x, int y)
        {
            /* see if the tile corresponds to the current player. */
            Piece p = board.pieces[x, y];
            if (p != null)
            {
                if (p.owner == currentPlayer && (state == GameState.waitPlayer || state == GameState.tileClicked))
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
                    }
                }
                if (isInList == true)
                {
                    /* add tile to the board. */
                    Piece newPiece = new ui.Piece(x, y, currentPlayer);
                    board.AddPiece(newPiece);
                    List<Piece> neigh = board.GetPieceNeighbors(newPiece);
                    foreach (Piece n in neigh)
                    {
                        board.AddPiece(newPiece, n);
                    }
                    optionList = null;
                    /* set next player. */
                    if (currentPlayer == player1)
                    {
                        currentPlayer = player2;
                    }
                    else
                    {
                        currentPlayer = player1;
                    }
                    /* set game state. */
                    state = GameState.waitPlayer;
                }
            }
        }
        public List<Piece> GetOptions()
        {
            return optionList;
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
    }
}

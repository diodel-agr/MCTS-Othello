using MCTS_Othello.player;
using System;
using System.Collections.Generic;

namespace MCTS_Othello.ui
{
    class Board
    {
        /* members. */
        public int size { get; }
        private int playerOneScore;
        private int playerTwoScore;
        public Piece[,] pieces { get; }

        /* constructors. */
        public Board(int sz)
        {
            size = sz;
            pieces = new Piece[sz, sz];
        }

        public Board(Board b)
        {
            size = b.size;
            playerOneScore = b.playerOneScore;
            playerTwoScore = b.playerTwoScore;
            pieces = new Piece[size, size];
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    if (b.pieces[i, j] != null)
                    {
                        pieces[i, j] = new Piece(b.pieces[i, j]);
                    }
                    else
                    {
                        pieces[i, j] = null;
                    }
                }
            }
        }

        /* methods. */
        /**
         * AddPiece - adds a piece on the board.
         * 
         * @p: the piece to add.
         * 
         * This function is mainly used to initialize the board.
         */
        public void AddPiece(Piece p)
        {
            pieces[p.X, p.Y] = p;
            IncPlayerScore(p.owner);
        }
        /**
         * AddPiece - adds a piece on the board.
         * 
         * @src: the 'selected' piece.
         * @dest: the new piece which we want to insert. 
         */
        public void AddPiece(Piece src, Piece dest)
        {
            pieces[dest.X, dest.Y] = dest;
            /* update board. */
            if (src.X == dest.X || src.Y == dest.Y)
            {
                UpdateBoardLine(src, dest);
            }
            else
            {
                UpdateBoardDiagonal(src, dest);
            }
        }

        /**
         * UpdateBoardLine - after a horizontal / vertical move, switches the color
         * of the enemy's pieces that are affected by the move.
         * 
         * @src: the 'selected' piece.
         * @dest: the piece that has been inserted.
         */
        private void UpdateBoardLine(Piece src, Piece dest)
        {
            /* check src and dest owner. */
            if (src.owner != dest.owner)
            {
                throw new MCTSException("Source and dest pieces don't have the same owner.");
            }
            int i;
            int start, end;
            if (src.X == dest.X)
            {   /* same column. */
                if (src.Y > dest.Y)
                {
                    UpdateBoardLine(dest, src);
                }
                start = src.Y + 1;
                end = dest.Y - 1;
                for (i = start; i <= end; ++i)
                {   /* switch owner and update score. */
                    DecPlayerScore(pieces[src.X, i].owner);
                    pieces[src.X, i].owner = src.owner;
                    IncPlayerScore(pieces[src.X, i].owner);
                }
            }
            else
            {   /* same line. */
                if (src.X > dest.X)
                {
                    UpdateBoardLine(dest, src);
                }
                start = src.X + 1;
                end = dest.X - 1;
                for (i = start; i <= end; ++i)
                {   /* switch owner and update score. */
                    DecPlayerScore(pieces[i, src.Y].owner);
                    pieces[i, src.Y].owner = src.owner;
                    IncPlayerScore(pieces[i, src.Y].owner);
                }
            }
        }

        /**
         * UpdateBoardDiagonal - after a diagonal move, switches the color
         * of the enemy's pieces that are affected by the move.
         * 
         * @src: the 'selected' piece.
         * @dest: the piece that has been inserted.
         */
        private void UpdateBoardDiagonal(Piece src, Piece dest)
        {
            /* check src and dest owner. */
            if (src.owner != dest.owner)
            {
                throw new MCTSException("Source and dest pieces doesn't have the same owner.");
            }
            int i, j, k;
            if (src.Y > dest.Y)
            {
                k = src.Y - dest.Y;
                if (src.X > dest.X)
                {
                    i = dest.X + 1;
                    j = dest.Y + 1;
                    for (int c = 1; c < k; ++c)
                    {   /* switch owner and update score. */
                        DecPlayerScore(pieces[i, j].owner);
                        pieces[i, j].owner = src.owner;
                        IncPlayerScore(pieces[i, j].owner);
                        i++;
                        j++;
                    }
                }
                else
                {
                    i = src.X + 1;
                    j = src.Y - 1;
                    for (int c = 1; c < k; ++c)
                    {   /* switch owner and update score. */
                        DecPlayerScore(pieces[i, j].owner);
                        pieces[i, j].owner = src.owner;
                        IncPlayerScore(pieces[i, j].owner);
                        i++;
                        j--;
                    }
                }
            }
            else /* src.Y < dest.Y */
            {
                UpdateBoardDiagonal(dest, src);
            }
        }

        /**
         * GetValidMoves - returns the posible positions for the next move.
         * 
         * @p the piece from which we want to obtain the posible positions for a new piece.
         * return: a list with the posible positions to move.
         * 
         * This function uses 8 different functions to construct the list of possible positions.
         * When the list is empty, there are no possible moves and the game is finished.
         */
        public List<Piece> GetValidMoves(Piece p)
        {
            List<Piece> options = new List<Piece>();
            Piece res;
            res = GetValidMovesUp(p);
            if (res != null) options.Add(res);
            res = GetValidMovesDown(p);
            if (res != null) options.Add(res);
            res = GetValidMovesLeft(p);
            if (res != null) options.Add(res);
            res = GetValidMovesRight(p);
            if (res != null) options.Add(res);
            res = GetValidMovesCornerOne(p);
            if (res != null) options.Add(res);
            res = GetValidMovesCornerTwo(p);
            if (res != null) options.Add(res);
            res = GetValidMovesCornerThree(p);
            if (res != null) options.Add(res);
            res = GetValidMovesCornerFour(p);
            if (res != null) options.Add(res);
            return options;
        }

        /**
         * GetValidMovesUp - returns a piece if it is posible to move on the up direction
         * or null if no up move is possible.
         * 
         * @p the piece to start the search in the specified direction. 
         */
        private Piece GetValidMovesUp(Piece p)
        {
            Piece result = null;
            int resY = p.Y - 1;
            while (resY > 0 && pieces[p.X, resY] != null)
            {
                if (pieces[p.X, resY].owner != p.owner)
                {
                    resY--;
                }
                else
                {
                    resY = -1;
                    break;
                }
            }
            if (resY != -1 && resY != (p.Y - 1) && pieces[p.X, resY] == null)
            {
                result = new Piece(p.X, resY, null);
            }
            return result;
        }

        /**
         * GetValidMovesDown - returns a piece if it is posible to move on the down direction
         * or null if no down move is possible.
         * 
         * @p the piece to start the search in the specified direction. 
         */
        private Piece GetValidMovesDown(Piece p)
        {
            Piece result = null;
            int resY = p.Y + 1;
            while (resY < (size - 1) && pieces[p.X, resY] != null)
            {
                if (pieces[p.X, resY].owner != p.owner)
                {
                    resY++;
                }
                else
                {
                    resY = size;
                    break;
                }
            }
            if (resY < size && resY != (p.Y + 1) && pieces[p.X, resY] == null)
            {
                result = new Piece(p.X, resY, null);
            }
            return result;
        }

        /**
         * GetValidMovesLeft - returns a piece if it is posible to move on the left direction
         * or null if no left move is possible.
         * 
         * @p the piece to start the search in the specified direction. 
         */
        private Piece GetValidMovesLeft(Piece p)
        {
            Piece result = null;
            int resX = p.X - 1;
            while (resX > 0 && pieces[resX, p.Y] != null)
            {
                if (pieces[resX, p.Y].owner != p.owner)
                {
                    resX--;
                }
                else
                {
                    resX = -1;
                    break;
                }
            }
            if (resX != -1 && resX != (p.X - 1) && pieces[resX, p.Y] == null)
            {
                result = new Piece(resX, p.Y, null);
            }
            return result;
        }

        /**
         * GetValidMovesRight - returns a piece if it is posible to move on the right direction
         * or null if no right move is possible.
         * 
         * @p the piece to start the search in the specified direction. 
         */
        private Piece GetValidMovesRight(Piece p)
        {
            Piece result = null;
            int resX = p.X + 1;
            while (resX < (size - 1) && pieces[resX, p.Y] != null)
            {
                if (pieces[resX, p.Y].owner != p.owner)
                {
                    resX++;
                }
                else
                {
                    resX = size;
                    break;
                }
            }
            if (resX < size && resX != (p.X + 1) && pieces[resX, p.Y] == null)
            {
                result = new Piece(resX, p.Y, null);
            }
            return result;
        }

        /**
         * GetValidMovesCornerOne - returns a piece if it is posible to move on the up-right corner direction
         * or null if no move is possible.
         * 
         * @p the piece to start the search in the specified direction. 
         */
        private Piece GetValidMovesCornerOne(Piece p)
        {
            Piece result = null;
            int resX = p.X + 1;
            int resY = p.Y - 1;
            while (resY > 0 && resX < (size - 1) && pieces[resX, resY] != null)
            {
                if (pieces[resX, resY].owner != p.owner)
                {
                    resY--;
                    resX++;
                }
                else
                {
                    resY = -1;
                    resX = -1;
                    break;
                }
            }
            if (resY != -1 && resY != (p.Y - 1) && resX != -1 && resX != (p.X + 1) && pieces[resX, resY] == null)
            {
                result = new Piece(resX, resY, null);
            }
            return result;
        }

        /**
         * GetValidMovesCornerTwo - returns a piece if it is posible to move on the up-left corner direction
         * or null if no move is possible.
         * 
         * @p the piece to start the search in the specified direction. 
         */
        private Piece GetValidMovesCornerTwo(Piece p)
        {
            Piece result = null;
            int resY = p.Y - 1;
            int resX = p.X - 1;
            while (resY > 0 && resX > 0 && pieces[resX, resY] != null)
            {
                if (pieces[resX, resY].owner != p.owner)
                {
                    resY--;
                    resX--;
                }
                else
                {
                    resY = -1;
                    resX = -1;
                    break;
                }
            }
            if (resY != -1 && resY != (p.Y - 1) && resX != -1 && resX != (p.X - 1) && pieces[resX, resY] == null)
            {
                result = new Piece(resX, resY, null);
            }
            return result;
        }

        /**
         * GetValidMovesCornerThree - returns a piece if it is posible to move on the down-left corner direction
         * or null if no move is possible.
         * 
         * @p the piece to start the search in the specified direction. 
         */
        private Piece GetValidMovesCornerThree(Piece p)
        {
            Piece result = null;
            int resY = p.Y + 1;
            int resX = p.X - 1;
            while (resY < (size - 1) && resX > 0 && pieces[resX, resY] != null)
            {
                if (pieces[resX, resY].owner != p.owner)
                {
                    resY++;
                    resX--;
                }
                else
                {
                    resY = -1;
                    resX = -1;
                    break;
                }
            }
            if (resY != -1 && resY != (p.Y + 1) && resX != -1 && resX != (p.X - 1) && pieces[resX, resY] == null)
            {
                result = new Piece(resX, resY, null);
            }
            return result;
        }

        /**
         * GetValidMovesCornerFour - returns a piece if it is posible to move on the down-right corner direction
         * or null if no move is possible.
         * 
         * @p the piece to start the search in the specified direction. 
         */
        private Piece GetValidMovesCornerFour(Piece p)
        {
            Piece result = null;
            int resY = p.Y + 1;
            int resX = p.X + 1;
            while (resY < (size - 1) && resX < (size - 1) && pieces[resX, resY] != null)
            {
                if (pieces[resX, resY].owner != p.owner)
                {
                    resY++;
                    resX++;
                }
                else
                {
                    resY = -1;
                    resX = -1;
                    break;
                }
            }
            if (resY != -1 && resY != (p.Y + 1) && resX != -1 && resX != (p.X + 1) && pieces[resX, resY] == null)
            {
                result = new Piece(resX, resY, null);
            }
            return result;
        }

        /**
         * GetPlayerPieces - returns the list of pieces of a certain player.
         * 
         * @c: player's color.
         * @return: players current pieces.
         */
        public List<Piece> GetPlayerPieces(player.Color c)
        {
            List<Piece> playerPieces = new List<Piece>();
            foreach (Piece p in pieces)
            {
                if (p != null && p.owner.GetColor() == c)
                {
                    playerPieces.Add(p);
                }
            }
            return playerPieces;
        }

        /**
         * GetPieceNeighbors - this function finds the neighbors of a piece. 
         * 
         * @p: the piece who's neighbors we want to find.
         * @return: a list of pieces which holds the neighbors on every direction.
         * 
         * The neighbors are pieces that are separated by at least one enemy piece. This function is useful
         * in some cases when a move might affect enemy's pieces that are not on the path between the src and dest
         * pieces. This function uses 8 functions to compose the result. 
         */
        public List<Piece> GetPieceNeighbors(Piece p)
        {
            List<Piece> neigh = new List<Piece>();
            Piece res;
            res = GetLeftNeighbor(p);
            if (res != null) neigh.Add(res);
            res = GetRightNeighbor(p);
            if (res != null) neigh.Add(res);
            res = GetUpNeighbor(p);
            if (res != null) neigh.Add(res);
            res = GetDownNeighbor(p);
            if (res != null) neigh.Add(res);
            res = GetCornerOneNeighbor(p);
            if (res != null) neigh.Add(res);
            res = GetCornerTwoNeighbor(p);
            if (res != null) neigh.Add(res);
            res = GetCornerThreeNeighbor(p);
            if (res != null) neigh.Add(res);
            res = GetCornerFourNeighbor(p);
            if (res != null) neigh.Add(res);
            return neigh;
        }

        /**
         * GetLeftNeighbor - finds the neighbor on the left.
         * 
         * @p: the piece who's neighbor we want to find.
         * @return: the neighbor piece or null if there is no neighbor.
         */
        private Piece GetLeftNeighbor(Piece p)
        {
            Piece result = null;
            int i = p.X - 1;
            while (i > 0 && pieces[i, p.Y] != null && pieces[i, p.Y].owner != p.owner)
            {
                i--;
            }
            if ( i != -1 && i != (p.X - 1) && pieces[i, p.Y] != null && pieces[i, p.Y].owner == p.owner)
            {
                result = pieces[i, p.Y];
            }
            return result;
        }

        /**
         * GetRightNeighbor - finds the neighbor on the right.
         * 
         * @p: the piece who's neighbor we want to find.
         * @return: the neighbor piece or null if there is no neighbor.
         */
        private Piece GetRightNeighbor(Piece p)
        {
            Piece result = null;
            int i = p.X + 1;
            while (i < (size - 1) && pieces[i, p.Y] != null && pieces[i, p.Y].owner != p.owner)
            {
                i++;
            }
            if (i < size && i != (p.X + 1) && pieces[i, p.Y] != null && pieces[i, p.Y].owner == p.owner)
            {
                result = pieces[i, p.Y];
            }
            return result;
        }

        /**
         * GetUpNeighbor - finds the neighbor from the up direction.
         * 
         * @p: the piece who's neighbor we want to find.
         * @return: the neighbor piece or null if there is no neighbor.
         */
        private Piece GetUpNeighbor(Piece p)
        {
            Piece result = null;
            int i = p.Y - 1;
            while (i > 0 && pieces[p.X, i] != null && pieces[p.X, i].owner != p.owner)
            {
                i--;
            }
            if (i != -1 && i != (p.Y - 1) && pieces[p.X, i] != null && pieces[p.X, i].owner == p.owner)
            {
                result = pieces[p.X, i];
            }
            return result;
        }

        /**
         * GetDownNeighbor - finds the neighbor from the down direction.
         * 
         * @p: the piece who's neighbor we want to find.
         * @return: the neighbor piece or null if there is no neighbor.
         */
        private Piece GetDownNeighbor(Piece p)
        {
            Piece result = null;
            int i = p.Y + 1;
            while (i < (size - 1) && pieces[p.X, i] != null && pieces[p.X, i].owner != p.owner)
            {
                i++;
            }
            if (i != size && i != (p.Y + 1) && pieces[p.X, i] != null && pieces[p.X, i].owner == p.owner)
            {
                result = pieces[p.X, i];
            }
            return result;
        }

        /**
         * GetCornerOneNeighbor - finds the neighbor from the up-right direction.
         * 
         * @p: the piece who's neighbor we want to find.
         * @return: the neighbor piece or null if there is no neighbor.
         */
        private Piece GetCornerOneNeighbor(Piece p)
        {
            Piece result = null;
            int i = p.X + 1;
            int j = p.Y - 1;
            while (i < (size - 1) && j > 0 && pieces[i, j] != null && pieces[i, j].owner != p.owner)
            {
                i++;
                j--;
            }
            if (i < size && i != (p.X + 1) && j > -1 && j != (p.Y - 1) && pieces[i, j] != null && pieces[i, j].owner == p.owner)
            {
                result = pieces[i, j];
            }
            return result;
        }

        /**
         * GetCornerTwoNeighbor - finds the neighbor from the up-left direction.
         * 
         * @p: the piece who's neighbor we want to find.
         * @return: the neighbor piece or null if there is no neighbor.
         */
        private Piece GetCornerTwoNeighbor(Piece p)
        {
            Piece result = null;
            int i = p.X - 1;
            int j = p.Y - 1;
            while (i > 0 && j > 0 && pieces[i, j] != null && pieces[i, j].owner != p.owner)
            {
                i--;
                j--;
            }
            if (i != -1 && i != (p.X - 1) && j != -1 && j != (p.Y - 1) && pieces[i, j] != null && pieces[i, j].owner == p.owner)
            {
                result = pieces[i, j];
            }
            return result;
        }

        /**
         * GetCornerThreeNeighbor - finds the neighbor from the down-left direction.
         * 
         * @p: the piece who's neighbor we want to find.
         * @return: the neighbor piece or null if there is no neighbor.
         */
        private Piece GetCornerThreeNeighbor(Piece p)
        {
            Piece result = null;
            int i = p.X - 1;
            int j = p.Y + 1;
            while (i > 0 && j < (size - 1) && pieces[i, j] != null && pieces[i, j].owner != p.owner)
            {
                i--;
                j++;
            }
            if (i != -1 && i != (p.X + 1) && j != size && j != (p.Y + 1) && pieces[i, j] != null && pieces[i, j].owner == p.owner)
            {
                result = pieces[i, j];
            }
            return result;
        }

        /**
         * GetCornerFourNeighbor - finds the neighbor from the down-right direction.
         * 
         * @p: the piece who's neighbor we want to find.
         * @return: the neighbor piece or null if there is no neighbor.
         */
        private Piece GetCornerFourNeighbor(Piece p)
        {
            Piece result = null;
            int i = p.X + 1;
            int j = p.Y + 1;
            while (i < (size - 1) && j < (size - 1) && pieces[i, j] != null && pieces[i, j].owner != p.owner)
            {
                i++;
                j++;
            }
            if (i != size && i != (p.X + 1) && j != size && j != (p.Y + 1) && pieces[i, j] != null && pieces[i, j].owner == p.owner)
            {
                result = pieces[i, j];
            }
            return result;
        }

        /**
         * GetScore - returns players score.
         * 
         * @player: player number: 1 or 2.
         * @return: the player's score.
         */
        public int GetScore(int player)
        {
            int result = -1;
            switch (player)
            {
                case 1:
                    result = playerOneScore;
                    break;
                case 2:
                    result = playerTwoScore;
                    break;
                default:
                    throw new MCTSException("[Board/GetScore()] - invalid parameter.");
            }
            return result;
        }

        /**
         * SetScore - sets the players score.
         * 
         * @player: player number: 1 or 2.
         * @score: new score value.
         */
        public void SetScore(int player, int score)
        {
            switch (player)
            {
                case 1:
                    playerOneScore = score;
                    break;
                case 2:
                    playerTwoScore = score;
                    break;
                default:
                    throw new MCTSException("[Board/SetScore()] - invalid parameter.");

            }
        }

        /**
         * IncPlayerScore - increments player score.
         * 
         * @p: the player whoose score needs to be incremented.
         */
        private void IncPlayerScore(IMCTSPlayer p)
        {
            if (p.GetColor() == player.Color.black)
            {   /* player 1. */
                playerOneScore++;
            }
            else
            {   /* player 2. */
                playerTwoScore++;
            }
            if (playerOneScore + playerTwoScore > 64)
            {
                throw new MCTSException("[Board/IncPlayerScore()] - players scores are invalid ( > 64).");
            }
        }

        /**
         * DecPlayerScore - decrements player score.
         * 
         * @p: current player.
         */
        private void DecPlayerScore(IMCTSPlayer p)
        {
            if (p.GetColor() == player.Color.black)
            {
                playerOneScore--;
                if (playerOneScore < 0)
                {
                    throw new MCTSException("[Board/DecPlayerScore()] - player one score is negative!");
                }
            }
            else
            {
                playerTwoScore--;
                if (playerTwoScore < 0)
                {
                    throw new MCTSException("[Board/DecPlayerScore()] - player two score is negative!");
                }
            }
        }

        /**
         * IsFinished - returns true if the game is finished and false otherwise.
         * 
         * @return: true / false.
         */
        public bool IsFinished(IMCTSPlayer player)
        {
            bool result = false;
            if (playerOneScore + playerTwoScore > 64)
            {
                throw new MCTSException("[Board/IsFinished()] - players score exceed maximum value.");
            }
            else if (playerOneScore + playerTwoScore == 64)
            {
                result = true;
            }
            else if (PlayerHasPossibleMoves(player) == false)
            {
                result = true;
            }
            return result;
        }

        /**
         * PlayerHasPossibleMoves - returns true if the current player has possible moves
         * and false otherwise.
         * 
         * @player: the current player.
         * @return: true / false.
         */
        public bool PlayerHasPossibleMoves(IMCTSPlayer player)
        {
            bool result = false;
            foreach (Piece p in pieces)
            {
                if (p != null && p.owner == player)
                {
                    if (GetValidMoves(p).Count > 0)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        public void PrintBoard()
        {
            Console.WriteLine(this.GetScore(1) + ":" + this.GetScore(2));
            for (int i = 0; i < this.size; ++i)
            {
                for (int j = 0; j < this.size; ++j)
                {
                    if (this.pieces[j, i] == null)
                    {
                        Console.Write(0 + " ");
                    }
                    else
                    {
                        char c = this.pieces[j, i].owner.GetColor().ToString()[0];
                        Console.Write(c + " ");
                    }
                }
                Console.WriteLine("");
            }
        }
        public void FreePieces()
        {
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    pieces[i, j] = null;
                }
            }
        }
    }
}
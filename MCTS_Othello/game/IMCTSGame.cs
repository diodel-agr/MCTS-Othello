using MCTS_Othello.player;
using MCTS_Othello.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTS_Othello.game
{
    /*
     * This interface shows the methods implemented by different types of games.
     * There are 3 types of games: computer vs computer, human vs computer and
     * human vs human. The game will be selected before the start of game.
     */
    interface IMCTSGame<T> : IObservable<T>
    {
        void Start();
        void InitBoard();
        void RestartGame();
        Piece[,] GetPieces();
        IMCTSPlayer GetCurrentPlayer();
        IMCTSPlayer GetWinner();
        void PlayerClicked(int x, int y);
        List<Piece> GetOptions();
        int GetScore(int player);
        bool PlayerHasPossibleMoves();
        void SetGameState(GameState state);
        IMCTSPlayer GetPlayer(int player);
        bool IsFinished();
        string GetGameResult();
    }
}

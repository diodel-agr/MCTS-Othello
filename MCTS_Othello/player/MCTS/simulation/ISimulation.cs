using MCTS_Othello.ui;

namespace MCTS_Othello.player.MCTS
{
    interface ISimulation
    {
        void StartSimulation();
        void StartSimulation(Piece p);
        Node GetSimulationResult();
        void SetBoard(Board board);
    }
}

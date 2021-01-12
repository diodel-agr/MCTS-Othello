# MCTS-Othello
Monte Carlo Tree Search (MCTS) bot for the Othello game.

## TO DO
- simplify IMCTSGame interface.
- make the ui update after bot move automated. [done for HC game]
- the score does not update after the player places a piece.
- refator comments.
- create an enum or something with the color of each player.
- create an enum with the configuration of the MCTSPlayer player.
- solve the 'out of memory issue, get rid of GV.Collect calls.
- simplify for loops with foreach (where possible).
- solve null reference on option list when clicked on an empty tile.
- create game factory.
- fix NullReferenceException in SandomSimulation:191.
- both Play methods from MCTSPlayer are empty...not cool, man
- simplify the Board::AddPiece method. It should not be necessary to do a 
manual Board::AddPiece(p1, p2) afterwards.
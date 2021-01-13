# MCTS-Othello
Monte Carlo Tree Search (MCTS) bot for the Othello game.

## TO DO
- simplify IMCTSGame interface.
- the score does not update after the player places a piece.
- refator comments.
- remove commented code.
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
- treat move == null in CCGame:207.
- bot1 and bot2 combo box implementations are too similar.
- human player generates method not implemented exception in subscribe method.
- create a greedy bot :D
- parameterize Thread.Sleep() parameters.
- error in simulation when starting the game after it was stopped.
- some thread remains in execution in the cc game after stop.

## DONE
- make the ui update after bot move automated. Done for all game types.
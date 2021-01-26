# MCTS-Othello
Monte Carlo Tree Search (MCTS) bot for the Othello game.

## TO DO
- MCTS Game restart map does not print initial pieces.
- some thread remains in execution in the cc game after stop (back propagation thread).
- make sure HCGame::BotThread function exits in any case.
- simplify IMCTSGame interface.
- make sure all worker threads stop when closing the app or stopping the game.
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

## DONE
- make the ui update after bot move automated. Done for all game types.
- the score does not update after the player places a piece.

## SOMETHING...
The subscribe method should start the thread which will then update the ui.
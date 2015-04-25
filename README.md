# OgreMaze
Submission for Reddit /r/DailyProgrammer Challenge #221
http://www.reddit.com/r/dailyprogrammer/comments/33hwwf/20150422_challenge_211_intermediate_ogre_maze/

This is a solution to the Ogre Maze daily programmer challenge including the bonus objective, using C#.

The pathfinding is done using the A* search algorithm (http://en.wikipedia.org/wiki/A*_search_algorithm).

Depending on whether OgreMaze.Core or OgreMaze.UI is set as the startup project, the application can operate in both console and UI modes.

CastleWindsor is used for dependency injection, and NUnit/Moq are used for unit testing although I didn't really stick to TDD for this project. 

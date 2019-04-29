# LD44

# Changes - Post Compo
GameController - Entites can spawn endlessly and lead to a crash.
	Added spawnEntityLimit into GameController.cs to stop spawning when that number of entities is reached. 
Jump aftfer unpausing - This can be a bad experience, jumping into enemies. Menus were a late addition, and the player controls are grabbed in the update method. Small check added to ensure that a jump is not requested while the game is paused. 
	In PlayerMovement.cs, added a reference to the GameController and did not set the bool jump if the game was not in the active state. Added GetGameState in GameController to be able to pass the pause status to the input class. 
Spamming Score - once frozen, iceblocks could be spammed with the whip giving unintended scores.
	In IceblockControl.cs, changed the tag of the Iceblock to ensure that the hit would no longer register "Iceblock"
	
	
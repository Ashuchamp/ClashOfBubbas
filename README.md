# Clash of Bubbas

## Controls
| Key | Function
| ------ | ------ |
| A | Move Left (will be teleported to other side if character on edge)
| D | Move right (will be teleported to other side if character on edge)
| SPACE | Shooting bullets in the game
| Right Arrow | Home screen -> character selection
| P | Pause when you are in the game
| P | Restart game when on end screen
| H | Takes you to home screen from end screen
| S | Starts game from home screen
| S | Starts game from character selection screen
| 1 (on number row) | Lets you choose "Spidey Legs" when on "Character Selection" screen (Difficulty 1)
| 2 (on number row) | Lets you choose "The Good Dinosaur" when on "Character Selection" screen (Difficulty 2)
| 3 (on number row) | Lets you choose "MiraBot" when on "Character Selection" screen (Difficulty 3)
| 4 (on number row) | Lets your choose "Bubba Junior" when on the "Character Selection" screen (Difficulty God Level) (Bubba Jr. is Bubba's secret twin that no one knows about - the enemy is Bubba Sr. who has turned to the dark side ) 




## Feature List
| Feature | Dependencies | Estimated time | Point value
| ------ | ------ | ------ | ------ |
| Create textures for platforms, character, background | None | 3 hours | 5
| Have a high score leaderboard | Need a scoring system for the game | 3.5 hours | 5
| Emergency Pause Button | Implement pause screeen and dynamic user interface| 3.5 hours | 5 
| Object collision algorithm | Need pbjects, textures for objects to determine locations | 3 hours | 5
| Realistic jump | Basic gameplay and object/platform texture  | 4 hours | 5 
| Implement enemies/hazards (stationary) | Create enenmy textures and hit algorithms | 5 hours | 5 
| Start (home) and end (game over) screens | create textures for start and end screens | 3 hours | 5
| | | | 
| Custom character texture/color | saving mechanisim, dynamic user interface, character has move ability, abiltiy to load user created fiuled and textures | 3 hours | 10
| Allow the player to fire projectiles | texture for bullet, sound effects for bullets, dynamic user interface, animation for bullet firing, hitboxes for bullets | 5 hours | 10
|Different power ups (Shield, flying cap, trampoline) | hitboxes for powerups, textures for powerups, implement algorithms for powerups, game physics, character jump, platform collision | 5 hours | 10
| Moving hazards and enemies | Implemention of stationary enemies | 10 hours | 10
||
|Gameplay synchronized with music | have in-game music | 5 hours | 15
|Infinite gameplay map that keeps generating platforms until player dies | random platforms and hazard generations that the player can overcome, scoring system based on relative height | 6 hours | 15
| Bubba boss fight | algorithms for bubba's movements, infinite world map with random platforms, hit detections between plater and bubba, scoring system based on defeating bubba |  15 hours | 15
| Custom diffuculty depending on the character chosen | multiple character textures, screens, hit boxes | 3 hours | 15
| Dynamic difficulty throughout the game | randomized platform and enenmy generation, scoring, enemies, and power-ups | 5 hours | 15
| --- | --- | 79 hours | 150 


## Testing Documentation 
### Test Plan

1.	Most common way of testing was play testing 
2.	After implementing a feature run the game and play it until it until the feature is thoroughly tested and fixed
3.	While changing structure from one class to multi class make sure to test every feature as it is implemented 

### Test Case 

1.	The rendering of textures
2.	Movement of player character via A and D keys (left and right)
3.	Realistic auto-jump for player
4.	Collision with platforms, enemies, and power-ups (ex. The trampoline, flying cap, and shield)
5.	Whether hitting an enemy (without the shield) triggers a game over
6.	Whether the player is still able to escape an enemy after they hit the enemy with a shield
7.	The updating of the player’s score based on their relative height above the starting platform
8.	Randomly generated platforms and objects
9.	The ability for the player to shoot
10.	Collision between bullets and enemies (where the enemy should disappear upon being hit by the bullet)
11.	Modification of local high score leaderboard, including file I/O
12.	Sound of objects/gameplay (ex. Game over noises)
13.	Whether the Bubba Boss fight triggers properly and functions as intended
14.	The functions of different characters (whether they work differently in the game, besides just texture)

### Test Case Results
1.	Textures do render properly, although some of our textures were underdeveloped and included some “copycat” textures from Doodle Jump
2.	The character is able to respond to the press of A and D keys
3.	The player can jump as intended, rather than instantly teleporting to the peak of their jump
4.	The character does collide with platforms if not jumping, and triggers a game over if they touch an enemy, but sometimes power-up collision may not function properly (ex. When landing on the trampolines, or touching the shield)
5.	A game over screen is triggered if the player hits an enemy without the shield
6.	The player has a timer that lasts for a couple seconds after they lose their shield due to an enemy collision, allowing them to get away before the game over is triggered
7.	The score increments based on the player’s accumulated jump height above their starting platform
8.	There is a series of algorithms that randomly generates platforms in a way that allows the player to keep moving upwards, and randomly places power-ups/enemies on some platforms (instead of floating in mid-air)
9.	The player can shoot with a press of the spacebar
10.	The enemy does disappear upon being hit by the bullet, although we initially had some glitches with where the enemy should be hit (to be counted as “defeated” by the bullet)
11.	A text file containing the game’s 10 highest scores is scanned so the scores can be loaded into the game, before being changed and exported if the player attains a score higher than the #10 score on the leaderboard
12.	Music is played throughout the course of the game, and sound is produced when the player touches the platforms or enemies
13.	The player can change their character by using the numerical keys “1,” “2,” “3,” or “4.”

### Test Failures
1.	Previously, we had a glitch where the player would keep jumping even if they touched a platform from below. This was fixed so that the player’s character would go through a platform but only jump if they land on top of it.
2.	The player doesn’t always jump higher if they land on a trampoline.
3.	At first, when the player touches an enemy with the shield on, they would immediately get a game over once the shield disappears.
4.	Initially, the score would increment every time the player jumps off the platform, even if the player isn’t trying to move towards a higher place.
5.	The enemy wouldn’t disappear when hit by the bullet, since the collision algorithm for the enemy and bullet was glitched (the “hitbox” of the enemy was to the left of the enemy sprite).
6.	An exception would be thrown when trying to handle I/O for the game’s high scores, because the game was unable to find the place where the text file (with the 10 highest scores) is stored.
7.	Bubba wouldn’t appear as a boss for the player to fight, even when the game’s condition meets the criteria for the boss fight to happen.
8.	When Bubba did appear as a boss, the player almost instantly gets eliminated because Bubba would typically spawn at the same Y-coordinate as that of the player.
9.	Bubba and the shield don’t disappear when the game is paused (their textures can still be seen over the pause screen).


### Mechanics - Dynamics - Aesthetics
#### Mechanics 
The player uses the A and D keys to move a character from side to side, so they can move up an infinite series of randomly-generated platforms. This game would seemingly continue forever, until the player gets a game over from colliding with an enemy or falling off the screen. There will also be randomly-generated enemies and items in the game, in addition to a Bubba boss fight. Additionally, the player gets to change their game’s difficulty by changing their character, with the type of character affecting the difficulty of the current game.

#### Dynamics
Because of the random platform, enemy, and item generation, the player doesn’t experience the same type of level all over again, which prevents them from familiarizing themselves with the exact arrangement of the game’s objects. However, the platforms, enemies, and items are arranged in a way that still allows the player to continue jumping up and improving their score. Moreover, the Bubba boss fight occurs with the random platform generation in mind, which presents an additional challenge for the player because they must multitask and focus on hitting the cat with “ink” while avoiding his attacks and making sure to stay on the platforms. Finally, the ability to change difficulty and character type lets the player have some control over the game’s difficulty, even if the difficulty may increase as the player progresses in the game.


#### Aesthetics
Our “Doodle Jump”-inspired game is very challenging, but the player can easily master it if they understand how each power up and enemy works. Additionally, with the presence of Bubba the cat as a boss, the player gets to appreciate some cuteness while trying to master the game’s controls and items. Lastly, through the ability to change the game’s difficulty based on the character chosen, the player may initially have a preference for a character that makes the game “easier,” but will eventually shift to trying characters that make the game “harder” once they become more experienced with the gameplay.





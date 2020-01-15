# Sheridan Lander
## Lunar Lander Game

Relevant code is located in the C# scripts under /Assets/Scripts/.

This is a modern Unity 3D recreation of the classic 1979 Atari game, Lunar Lander: <br>
https://www.youtube.com/watch?v=McAhSoAEbhM

The goal of this game is safely land a lunar space craft on designated spots on the moon for points. The player must carefully balance the physics of the lander by rotating it left/right and applying a constant thrust force when needed, while also managing fuel consumption. If the lander collides with the moon's surface at an incorrect angle and/or high velocity, it will crash.

The lander is a multi-part model, held together by Fixed Joints, which breaks a apart upon crashing. <br>
For each game, the moon's surface mesh and landing platforms are randomly generated. <br>
The camera will zoom in and focus on the lander when it lands or crashes.


-----------------


### Game Loop

The player can keep landing on platforms until their fuel runs out. 
Each time they land a platform successfully, the lander is placed back into the sky
and the stage is regenerated.

### Controls

Click on the UI buttons to activate them.
Control the lunar lander using the up, left, and right arrows.
ARROW UP = add force in the lander's forward directon
ARROW LEFT/RIGHT = add torque to the lander 

Note: both types of lander movement consume fuel

### Crashing

The lander will crash instantly if any part of it other than its feet touch the moon's surface.
The lander will also crash if it's feet touch the surface AND:

1) It is leaning more than 10 degrees to either the left or right.
2) It's landing speed is greater than a specific threshold.
3) It has ran out of fuel and lands on a part of the surface that has no platform.

### Points

Each platform is represented by a flat purple/white particle line on the moon's surface.
3 or 4 are randomly placed with the generated moon surface.
The points associated with each platform are based on their width. A smaller width = less points.

The player will be awarded the points associated with a platform if:

1) The lander lands on a platform without crashing and 
2) remains completetly upright (leaning less than 1 degree left or right) for 2 continous seconds

note: The player can till be awarded points even after their fuel runs out, if they are lucky enough to 
have the lander fall on a platform successfully. 
If this happens, they will get the points but the game will end (since they have no more fuel).

Screenshots:

![Screenshot1](https://user-images.githubusercontent.com/15040875/72311665-7026d500-3653-11ea-9cbc-8c8350faa451.PNG)
![Screenshot2](https://user-images.githubusercontent.com/15040875/72311666-7026d500-3653-11ea-962f-a36232fca539.PNG)
![Screenshot3](https://user-images.githubusercontent.com/15040875/72311667-7026d500-3653-11ea-90f2-2cfe4b3c39be.PNG)
![Screenshot4](https://user-images.githubusercontent.com/15040875/72311668-7026d500-3653-11ea-8ad2-a8718020683b.PNG)
![Screenshot5](https://user-images.githubusercontent.com/15040875/72311669-70bf6b80-3653-11ea-8c18-408f335b35d1.PNG)


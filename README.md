PROG50102 Assignmnet 1 Submission
Alessandro Profenna
Sept. 25, 2019

-----------------

Some notes:

To build the game, add the two scenes in Assets/Scenes in the Build Settings in this order:

MainMenu
MainGame

Then delete the SampleScene that comes default with the new project.
Then build as standalone.

My implementation of the Sheridan Lander Game has all of the required features.
Possible considerations for bonus points:

1) Randomly generated mesh for the moon's surface (custom code in StageBuilder.cs)
2) Camera zooms in on the lunar lander when you successfully land or crash (custom code in CameraController.cs)

Thanks!

-----------------

Game information below:

GAME LOOP:

The player can keep landing on platforms until their fuel runs out. 
Each time they land a platform successfully, the lander is placed back into the sky
and the stage is randomly regenerated.

CONTROLS:

Click on the UI buttons to activate them.
Control the lunar lander using the up, left, and right arrows.
ARROW UP = add force in the lander's forward directon
ARROW LEFT/RIGHT = add torque to the lander 

Note: both types of lander movement consume fuel

CRASHING:

The lander will crash instantly if any part of it other than its feet touch the moon's surface.
The lander will also crash if it's feet touch the surface AND:

1) It is leaning more than 10 degrees to either the left or right.
2) It's landing speed is greater than a specific threshold.
3) It has ran out of fuel and lands on a part of the surface that has no platform.

POINTS: 

Each platform is represented by a flat purple/white particle line on the moon's surface.
3 or 4 are randomly placed with the generated moon surface.
The points associated with each platform are based on their width. A smaller width = less points.

The player will be awarded the points associated with a platform if:

1) The lander lands on a platform without crashing and 
2) remains completetly upright (leaning less than 1 degree left or right) for 2 continous seconds

note: The player can till be awarded points even after their fuel runs out, if they are lucky enough to 
have the lander fall on a platform successfully. 
If this happens, they will get the points but the game will end (since they have no more fuel).

![Screenshot1](https://user-images.githubusercontent.com/15040875/72311665-7026d500-3653-11ea-9cbc-8c8350faa451.PNG)
![Screenshot2](https://user-images.githubusercontent.com/15040875/72311666-7026d500-3653-11ea-962f-a36232fca539.PNG)
![Screenshot3](https://user-images.githubusercontent.com/15040875/72311667-7026d500-3653-11ea-90f2-2cfe4b3c39be.PNG)
![Screenshot4](https://user-images.githubusercontent.com/15040875/72311668-7026d500-3653-11ea-8ad2-a8718020683b.PNG)
![Screenshot5](https://user-images.githubusercontent.com/15040875/72311669-70bf6b80-3653-11ea-8c18-408f335b35d1.PNG)


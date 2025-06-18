
Player

Player character camera
    change this when you are changing the player number
    CinemachineVirtualCamera-> Follow ----[drag the player character that you want to display]

direct linked scripts
    player mouse controller
    A star path finding
    

if there are errors like NullRefference exception
	this might be due to the player_mouse_controller script running ahead of the map manager script in the grid
	you can change the execution order of the scripts in the Edit-> Project Settings --> Execution order  


indirectly linked scripts 
    map manager in the grid

map manager in the grid
    - generates a grid like 2d array of the specific level the player is in and 
    - represents the walkable areas in floor1 & ground
    - so it returns two arrays 
        - ground
        - floor1 

 


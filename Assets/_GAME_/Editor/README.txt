
Editor 
    - making graph nodes and storing them in a level file
    - 




--------------------------------------------------------------------------------------------------------------------
Assumptions or the Implementation level for the current game

    - consider the player can move only in one floor so that no need of floor complexity in the level data and don't store it in the level data
    - the file format of the level data will be in 
        Store the graph in a separate file per level (like a .json or ScriptableObject)
        Json ---> easier for the graph to load and modify at run time. best for load/save in runtime
        ScriptableObject ---> best for initial loading of static data/ for storing static data which don't change at runtime,  Multiple components can reference the same ScriptableObject without duplicating memory.



--------------------------------------------------------------------------------------------------------------------






level graph editor
    - makes a unity custom tool that does what ever we say to do 
    - we need this to select the nodes or graph node tiles in the tilemap and store these in a seperate level file to get the required graph structure for the best path finding
    - make the neccessary GUI for the node selection and edge drawing in the level
    - need a feature to show up the level graph nodes on top of the current level tilemap 
    - need features to modify the level graph nodes
    - need a feature to store the new level graph nodes or the modified graph nodes 
    - what functions does it contain
        - 



level data
    - we store the fields what to store in the level in this file 
    - 

levels --> level1.json
    - stores the node data and the connecting nodes and its tile position according to the array









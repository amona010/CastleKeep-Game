# CastleKeep-Game
An Interactive Demonstration of Network Security Concepts

***How to Create Maps***

Step 1: Download project folder and make sure it can be run on Unity (Select 'Open' option and open folder on the Unity project menu). 

Step 2: On the Unity editor, make sure to view the project folder and open the scenes folder. From there you will select "SmallNetwork". Double click to open this scene in editor. This is the scene in which all networks will be generated.

Step 3: In order to get a good idea of what the network is going to look like, what I did was open the prefabs folder and drag and drop the "Point" Prefab onto the screen. I continued to do this until all the nodes in the network map were placed. **Remember that for the sake of the game, try to avoid diagonal edges between nodes**

Step 4: Once the nodes are placed, all you need now is the x and y values of each node, simply click each "Point" in the heirarchy tab to see the X and Y values. 

Step 5: Go to the file explorer, enter the game file, and go to Assets -> StreamingAssets, It is in there where you will identify the data.JSON file, it is in this file where you will configure the maps and settings for each map. 

The default map has these fields set:

"name": "SmallNetwork" - Name of the map

"rounds": -1 - Number of rounds in the map, setting the value to -1 means that the user will play that map until they lose. 

"startingCurrency": 1000 - Initial amount of currency the user will have in the specific map. 

"currencyGainedPerRound": 100 - Amount of currency the user will be awarded after every round. 

"waves": 3 - Number of waves per round in the map

"redTileMaximumHealth": 300 - Health of the red tile, health of red tiles will degrade -1 per second. 

"redTileLowHealthAccuracyAttackers": 90 - A red tile's chance of identifying an attacker at 50%. 

"redTileCriticalHealthAccuracyAttackers": 80 - A red tile's chance of identifying an attacker at 25% health.

"redTileLowHealthAccuracyCitizens": 95 - A red tile's chance of identifying and not eliminating citizens at 50% health.

"redTileCriticalHealthAccuracyCitizens": 90 - A red tile's chance of identifying and not eliminating citizens at 25% health.

"orangeTileAccuracyCitizens": 95 - An orange tile's chance of identifying and not eliminating citizens.

"orangeTileAttackerLimit": 5 - Number of times a specific attacker can pass through an orange tile and be identified.

"attackerProbability": 30 - Chance of an attacker appearing per spawn. 

"packetsPerWave": 10 - Packets that are spawned per wave, each packet is either an attacker or citizen as determined by the attacker probability.

**List of vertices (AKA nodes) are written like so:**

"vertices": 
[

{

"name": "Point9",

"isHost": false,

"isMainHost": false,

"isSpawnPoint": false,

"isSwitch": true,

"hasUpperTile": false,

"hasLowerTile": false,

"hasLeftTile": false,

"hasRightTile": false,

"location": {

"x": 7.1529998779296879,

"y": 1.559000015258789

}

},

...

{

"name": "Point3",

"isHost": true,

"isMainHost": false,

"isSpawnPoint": false,

"isSwitch": false,

"hasUpperTile": false,

"hasLowerTile": true,

"hasLeftTile": true,

"hasRightTile": false,

"location": {

"x": -2.929574728012085,

"y": 2.341060161590576

}

}

],

**Lists of edges are written like so:**

"edges": 
[

{

"v1": "Point9",

"v2": "Point12"

},

...

{

"v1": "Point3",

"v2": "Point5"

}


Step 6: Once the settings are configured. Test the game and maps out in the unity editor. (Do this by pressing the play button at the top of the screen) and test the game. 

Step 7: In order to build the game and have it ready for distribution, go to file -> Build settings. In the build settings menu, set the 
platform to WebGL and click on player settings. Make sure the canvas width is set to 1500 and the height is set to 600, have the "Run in Background" option unchecked. Once those settings are set, then go back to the build settings menu and click the build option and select the folder in which the program will be built in. 

Step 8: Once the project is finished building, zip the file holding the program and use it to upload to itch.io, the website only needs a zipped file of the program in order to work. 

Step 9: Once the game is uploaded, you can then distribute the game to however many subjects you would like. 
    
     
 

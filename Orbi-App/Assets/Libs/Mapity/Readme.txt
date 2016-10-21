Welcome to Map-ity!

Map-ity Version 2.1.0 © RewindGameStudio 2016

Support:
http://www.rewindgamestudio.com/

Documentation:
http://www.rewindgamestudio.com/documentation/current/index.html

Please see the link above for detailed documentation on Map-ity.

Map-ity allows you to model your environment on any real world location by providing access to open street map data.
Use it to get a lists of roads, waterways and buildings to use in your scene!
Use it to get the classification: Motorway, Primary or Secondary road. Canal or River.

Features:

Download the map data for your current location on mobile devices, or use a previously saved file.
Get lists of: Roads, Waterways, Buildings, Relations( e.g. Bus Routes ), Ways( Any map defined path ), Nodes( Any map feature )
Render a Gizmo showing the map in Editor.

Now compatible with WorldComposer, BuildR, RoadArchitect, SuperSplines.

Getting Started:

1. Add the Mapity prefab to your scene
2. Select it and setup any options
3. Either hit run or the Build Map button to see the map in Editor

The code has lots of comments and should be easy to understand.

OpenStreetMap Data:
http://wiki.openstreetmap.org/wiki/Main_Page

This contains a lot of information about the data that is parsed by map-ity.

GeoNames Data:

Map-ity can now query height data for any longitude & latitude. This uses GeoNames SRTM webservice which requires a free account.
http://www.geonames.org/login Accounts can be set up here or we can do it for you.
Note you must go to http://www.geonames.org/manageaccount and enable the free web service.
A limit of free accounts is a max number of coordinates per query of 20. This is reflected in the code.

Note: Performing a web request for every 20 nodes can be slow. Best suited to small data sets.

A general overview - 
All map data is represented by a base element called a node. This can be a single structure
or part of a road.

Ways are comprised of nodes and represent Highways, Waterways etc.

Relations can contain Nodes, Ways and other Relations. Examples include BusRoutes.

ChangeList:
Version 2.1.1 -
Fixed snap to terrain.
Added support for snapping to multiple terrains.

Version 2.1.0 -
Fixed on-device issues for iOS & Android.
Fixed settings being lost on play.
Fixed settings not saving.
Defined out WebGL.

Version 2.0 -
Re-wrote data parsing, now much faster, uses less memory. Can handle much larger data sets.
Data is downloaded and written to a file, then parsed. This saves a lot of memory.
Reworked UI.
Upgraded BuildR Samples.

Version 1.09.4 -
Improved UI.
Upgraded BuildR Samples.
Fixed snap to terrain bug.

Version 1.09.1 -
Bug fixes.

Version 1.09 -
Bug fixes.
Documentation updates.

Version 1.08 -
Added support for WorldComposer.
As a first step Map-ity will read the location of the current WorldComposer area.
This can be toggled on/off.
Added terrain snapping.
Map-ity will snap it's nodes to the terrain if it exists.

Version 1.07 -
Added support for Unitys 2D behaviour mode.
Switching behaviour will swap the Y & Z axis. i.e. Lattitude and elevation swap axis.
This gives a top down map in 2D mode.
There is an override flag in the Map-ity Data preferences to manually set the mode.
This allows you to mix 2D & 3D modes as you choose.
Added Code Sample

Version 1.06 -
Bug Fixes. 
Loading events added.
BuildR sample package added.
RoadArchitect sample package added.
SuperSplines sample package added.

Version 1.05 -
Bug Fixes. 
Custom Inspector.

Version 1.04 -
Bug Fixes. HasLoaded was set too early.
Added OSM Tags to all Map Nodes, Ways and Relations.
These are accessible through a Tags class containing a HashTable of all tags.

Version 1.03 - 
Fixed webplayer compilation bug.
Added Highway Labels.
Added Height data queries from GeoNames.org.

The Open Street Map Data is © OpenStreetMap contributors. http://www.openstreetmap.org/copyright

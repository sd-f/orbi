using UnityEngine;
using System;
using System.Threading;
using System.Collections;
using System.Linq;
using System.IO;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class Mapity : MonoBehaviour {

    /// <summary>
    /// Gets the singleton.
    /// </summary>
    /// <value>The singleton.</value>
	public static Mapity Singleton { get; private set; }

	/// <summary>
	/// Awake this instance.
	/// </summary>
	private void Awake()
	{
		if (Singleton == null)
		{
			Singleton = this as Mapity;
		}
	}

    #region Settings

    /// <summary>
    /// The auto load. Will we load in the Start() method or wait for a manual call to Load()
    /// </summary>
    public bool autoLoad = true;

    /// <summary>
    /// The loading in progress. Is a load in progress.
    /// </summary>
    private bool loadingInProgress = false;

    /// <summary>
    /// The has loaded. Has Mapity loaded and parsed the data.
    /// </summary>
    private bool hasLoaded = false;

    /// <summary>
    /// The download map data. Will we download data or use the local *.mapity data
    /// </summary>
	public bool downloadMapData = true;

	/// <summary>
	/// The download map data. Will we download data or use the local *.mapity data
	/// </summary>
	public bool DownloadMapData {
		get {
			return downloadMapData;
		}
		set {
			downloadMapData = value;
		}
	}

    /// <summary>
    /// The open street map API URL. We use this to download the map data.
    /// See http://wiki.openstreetmap.org/wiki/Main_Page for more info.
    /// http://www.overpass-api.de/api/xapi? ( Recommended for release ).
    /// http://api.openstreetmap.org/api/0.6/ ( Used for editing data, a good fall back for dev ).
    /// </summary>
    public string openStreetMapApiUrl = "http://www.overpass-api.de/api/xapi?";

    /// <summary>
    /// The query height data. Will we query height data from GeoNames
    /// </summary>
    public bool queryHeightData = false;

    /// <summary>
    /// Should we snap our height to match the terrain
    /// </summary>
    public bool snapToTerrain = false;

    /// <summary>
    /// The geonames API URL. This is used to get the srtm3 data for a specific longitude & latitude
    /// See http://www.geonames.org/export/web-services.html
    /// </summary>
    public string geonamesApiUrl = "http://api.geonames.org/srtm3?";

    /// <summary>
    /// The geonames username. You need a login to use GeoNames, it's free.
    /// See http://www.geonames.org/login for account creation
    /// </summary>
    public string geonamesUsername = "demo";

    /// <summary>
    /// The use device location. Will we try query device location to get map data.
    /// </summary>
	public bool useDeviceLocation = false;

	/// <summary>
	/// The use device location. Will we try query device location to get map data.
	/// </summary>
	public bool UseDeviceLocation {
		get {
			return useDeviceLocation;
		}
		set {
			useDeviceLocation = value;
		}
	}

    /// <summary>
    /// The Geographic Location. Longitude & Lattitude ( x & y ) of the currently loaded/downloaded map data.
    /// </summary>
    public Vector2 location = new Vector2(0.0f, 0.0f);

    /// <summary>
    /// Sets Longitude by a string
    /// </summary>
    /// <param name="strLongitude"></param>
    public void SetLongitude(string strLongitude )
    {
        try
        {
            location.x = Convert.ToSingle(strLongitude);
        }
        catch (FormatException)
        {
            MapityLog("Invalid format for Longitude");
        }
        
    }

    /// <summary>
    /// Sets Lattitude by a string
    /// </summary>
    /// <param name="strLattitude"></param>
    public void SetLattitude(string strLattitude)
    {
        try
        {
            location.y = Convert.ToSingle(strLattitude);
        }
        catch (FormatException)
        {
            MapityLog("Invalid format for Longitude");
        }
    }

    /// <summary>
    /// The name of the local mapity file we're loading from the MapData folder.
    /// </summary>
    public string localMapFileName = "";

    /// <summary>
    /// The save downloaded map data. Should we save the downloaded map data.
    /// </summary>
    public bool saveDownloadedMapData = false;

    /// <summary>
    /// The map file name save prefix. Use this string to prefix the downloaded mapity data for saving.
    /// </summary>
    public string mapFileNameSavePrefix = "MapData_";

    /// <summary>
    /// The enable debug logging. This is slow. It will output all mapity's debug logs. Useful for debugging.
    /// </summary>
	public bool enableLogging = false;

	/// <summary>
	/// The enable debug errors. It will output all mapity's debug error logs. Useful for debugging.
	/// </summary>
	public bool enableErrors = false;

    /// <summary>
    /// The persistent gizmos. Should gizmos render when mapity object not selected
    /// </summary>
    public bool persistentGizmos = false;

    /// <summary>
    /// The gizmo draw nodes. Draws a Gizmo at every map node - Caution!( Don't exceed Unity's max supported verts for gizmos )
    /// </summary>
    public bool gizmoDrawNodes = false;

    /// <summary>
    /// The gizmo draw ways. Should we draw the ways.
    /// </summary>
    public bool gizmoDrawWays = true;
    /// <summary>
    /// The gizmo draw high ways. Should we draw the HighWays.
    /// </summary>
    public bool gizmoDrawHighWays = true;
    /// <summary>
    /// The gizmo draw high ways labels. Should we draw the HighWays labels containg additional information.
    /// </summary>
    public bool gizmoDrawHighWaysLabels = false;
    /// <summary>
    /// The gizmo draw water ways. Should we draw the WaterWays.
    /// </summary>
    public bool gizmoDrawWaterWays = false;
    /// <summary>
    /// The gizmo draw buildings. Should we draw the Buildings.
    /// </summary>
    public bool gizmoDrawBuildings = false;
    /// <summary>
    /// The gizmo draw relations. Should we draw the Relations.
    /// </summary>
    public bool gizmoDrawRelations = false;

    /// <summary>
    /// Map zoom. Caution!( Higher Zoom Levels contain a lot of data ) 0,1,2 recommended as this is a manageble amount of data
    /// </summary>
    public enum MapZoom
    {
        ZoomLevel_0 = 1, // Approx 217m x 217m
        ZoomLevel_1,
        ZoomLevel_2,
        ZoomLevel_3,
        ZoomLevel_4,
        ZoomLevel_5,
        ZoomLevel_6,
        ZoomLevel_7,
        ZoomLevel_8,
        ZoomLevel_9,
        ZoomLevel_10,
        ZoomLevel_25 = 25,
        ZoomLevel_50 = 50,
        ZoomLevel_100 = 100
    }

    /// <summary>
    /// The map zoom level.
    /// </summary>
    public MapZoom mapZoom = MapZoom.ZoomLevel_0;

    /// <summary>
    /// Override default behaviour mode.
    /// </summary>
    public bool overrideDefaultBehaviourMode = false;

    /// <summary>
    /// Mapity default behavior mode.
    /// </summary>
    public enum MapityDefaultBehaviourMode
    {
        MapityMode_2D,
        MapityMode_3D
    }

    /// <summary>
    /// The mapity default behavior mode.
    /// </summary>
    public MapityDefaultBehaviourMode mapityDefaultBehaviourMode = MapityDefaultBehaviourMode.MapityMode_3D;

    /// <summary>
    /// Mapity's internal path to the map data
    /// </summary>
    private string m_path = "";

    #endregion

    #region Events

    public delegate void MapityLoadedEventHandler();
    public delegate void MapityUnloadedEventHandler();

    /// <summary>
    /// Occurs when mapity is loaded.
    /// </summary>
    public static event MapityLoadedEventHandler MapityLoaded;

    /// <summary>
    /// Occurs when mapity is unloaded.
    /// </summary>
    public static event MapityUnloadedEventHandler MapityUnloaded;

    #endregion

    #region Constants

    /// <summary>
    /// The base zoom. e.g. ZoomLevel_0 = degreeImMeters * baseZoom;
    /// </summary>
    private const float baseZoom = 0.0009765625f;

    /// <summary>
    /// The world to unity scale. e.g. When 1.0f, 1 meter = 1 unity unit.
    /// </summary>
    public float worldToUnityScale = 1.0f;

    /// <summary>
    /// Geonames no height data constant
    /// </summary>
    private const int geonamesNoHeightData = -32768;
    #endregion

    #region MapPosition

    public class Position
    {
        private float longitude;

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public float Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                longitude = value;
            }
        }

        private float latitude;

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public float Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                latitude = value;
            }
        }

        private float elevation;

        /// <summary>
        /// Gets or sets the elevation.
        /// </summary>
        /// <value>The elevation.</value>
        public float Elevation
        {
            get
            {
                return elevation;
            }
            set
            {
                elevation = value;
            }
        }

        /// <summary>
        /// Returns the position as a Vector3 taking into account swapping the elevation and lattitude in 2D Mode.
        /// </summary>
        /// <returns>Position converted to a Vector3.</returns>
        public Vector3 ToVector()
        {
            bool mode2D = false;

            if (Singleton.overrideDefaultBehaviourMode)
            {
				mode2D = (Singleton.mapityDefaultBehaviourMode == MapityDefaultBehaviourMode.MapityMode_2D);
            }
#if UNITY_EDITOR  
            else
            {              
                mode2D = (EditorSettings.defaultBehaviorMode == EditorBehaviorMode.Mode2D);
            }
#endif

            if (mode2D)
            {
                return new Vector3(longitude, latitude, elevation);
            }
            else
            {
                return new Vector3(longitude, elevation, latitude);
            }
        }

        public override string ToString()
        {
            return "Position - Longitude: " + longitude + ", Lattitude: " + latitude + ", Elevation: " + elevation;
        }

        public Position()
        {
            longitude = 0.0f;
            latitude = 0.0f;
            elevation = 0.0f;
        }

    }

    /// <summary>
    /// Map position. A class to store map positional data.
    /// </summary>
    public class MapPosition
    {
        /// <summary>
        /// The geographic position (Longitude, Lattiude).
        /// </summary>
        public Position geographic;

        /// <summary>
        /// The world position in Unity coordinates.
        /// </summary>	
        public Position world;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mapity.MapPosition"/> class.
        /// </summary>
        public MapPosition()
        {
            geographic = new Position();
            world = new Position();
        }
    }

    #endregion

    #region MapBounds

    /// <summary>
    /// Map bounds. A class to store the bounds( edges ) information.
    /// Also contains related helper functions.
    /// </summary>
    public class MapBounds
    {
        /// <summary>
        /// The minimum.
        /// </summary>
        public MapPosition min;

        /// <summary>
        /// The max.
        /// </summary>
        public MapPosition max;

        /// <summary>
        /// The center.
        /// </summary>
        public MapPosition center;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mapity.MapBounds"/> class.
        /// </summary>
        public MapBounds()
        {
            min = new MapPosition();
            max = new MapPosition();
            center = new MapPosition();
        }

        public void Initalise( float bounds, Vector2 location )
        {
            min.geographic.Longitude = location.x - bounds;
            min.geographic.Latitude = location.y - bounds;
            max.geographic.Longitude = location.x + bounds;
            max.geographic.Latitude = location.y + bounds;

            CalculateBoundsCenter();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Mapity.MapBounds"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents the current <see cref="Mapity.MapBounds"/>.
        /// </returns>
        public override string ToString()
        {
            return "World:" + min.world.Longitude + "," + min.world.Latitude + "," + max.world.Longitude + "," + max.world.Latitude + "\n" +
                "Geographic:" + min.geographic.Longitude + "," + min.geographic.Latitude + "," + max.geographic.Longitude + "," + max.geographic.Latitude + "\n";
        }

        /// <summary>
        /// Geographic to world coordinate. Converts a geographic coordinate to a world coordinate.
        /// </summary>
        /// <returns>
        /// The world coordinate.
        /// </returns>
        /// <param name='geographic'>
        /// Geographic position.
        /// </param>
        public Position GeographicToWorldCoordinate(Position geographic)
        {
            // Locals used in conversion

            float geoLat = geographic.Latitude;
            float geoLon = geographic.Longitude;

            Position world = new Position();

            // Convert latitude to radians

            float lat = geoLat * Mathf.Deg2Rad;

            // Set up "Constants"

            float m1 = 111132.92f;   // latitude calculation term 1

            float m2 = -559.82f;     // latitude calculation term 2

            float m3 = 1.175f;   // latitude calculation term 3

            float m4 = -0.0023f;     // latitude calculation term 4

            float p1 = 111412.84f;   // longitude calculation term 1

            float p2 = -93.5f;   // longitude calculation term 2

            float p3 = 0.118f;   // longitude calculation term 3	

            // Calculate the length of a degree of latitude and longitude in meters

            float latlen = m1 + (m2 * Mathf.Cos(2 * lat)) + (m3 * Mathf.Cos(4 * lat)) + (m4 * Mathf.Cos(6 * lat));

            float longlen = (p1 * Mathf.Cos(lat)) + (p2 * Mathf.Cos(3 * lat)) + (p3 * Mathf.Cos(5 * lat));

            geoLon -= center.geographic.Longitude;
            geoLat -= center.geographic.Latitude;

			world.Longitude = (longlen * geoLon) * Singleton.worldToUnityScale;
			world.Latitude = (latlen * geoLat) * Singleton.worldToUnityScale;

            world.Elevation = geographic.Elevation;

            return world;
        }

        public void CalculateBoundsCenter()
        {
            center.geographic.Longitude = min.geographic.Longitude + ((max.geographic.Longitude - min.geographic.Longitude) / 2);
            center.geographic.Latitude = min.geographic.Latitude + ((max.geographic.Latitude - min.geographic.Latitude) / 2);
        }
    }

    /// <summary>
    /// The map bounds.
    /// </summary>
    public MapBounds mapBounds = new MapBounds();

    #endregion

    #region Tags

    /// <summary>
    /// Tags. Contains a Hashtable of all the OSM Tags
    /// </summary>
    public class Tags
    {
        /// <summary>
        /// The tags.
        /// </summary>
        Hashtable tags;

        /// <summary>
        /// Adds the tag.
        /// </summary>
        /// <param name='k'>
        /// K. Key
        /// </param>
        /// <param name='v'>
        /// V. Value
        /// </param>
        public void AddTag(string k, string v)
        {
            tags.Add(k, v);
        }

        /// <summary>
        /// Gets a specific tag.
        /// </summary>
        /// <returns>
        /// The tag.
        /// </returns>
        /// <param name='k'>
        /// K. Key
        /// </param>
        public string GetTag(string k)
        {
            return tags[k] as string;
        }

        /// <summary>
        /// Gets the tags Hashtable.
        /// </summary>
        /// <returns>
        /// The tags.
        /// </returns>
        public Hashtable GetTags()
        {
            return tags;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mapity.Tags"/> class.
        /// </summary>
        public Tags()
        {
            tags = new Hashtable();
        }
    }

    /// <summary>
    /// Tagged. Base class for any object containing tags
    /// </summary>
    public class Tagged
    {
        /// <summary>
        /// The tags.
        /// </summary>
        public Tags tags;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mapity.Tagged"/> class.
        /// </summary>
        public Tagged()
        {
            tags = new Tags();
        }
    }

    #endregion

    #region MapNodes

    /// <summary>
    /// Map node. A Map Node is the basic map element. All other elements are contructed from Nodes.
    /// A Node defines a single geospatial point using a latitude and longitude.
    /// Nodes can be used to define standalone point features for example, a town or village.
    /// Nodes are also used to define the path of a Way.
    /// </summary>
    public class MapNode: Tagged
    {
        /// <summary>
        /// The position. The Nodes position.
        /// </summary>
        public MapPosition position;

        /// <summary>
        /// The identifier. A unique ID used for HashTable look ups.
        /// </summary>
        public uint id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mapity.MapNode"/> class.
        /// </summary>
        public MapNode() : base()
        {
            position = new MapPosition();

            id = 0u;
        }
    }

    /// <summary>
    /// The map nodes Hashtable.
    /// </summary>
    public Hashtable mapNodes = new Hashtable();

    #endregion

    #region MapWays

    /// <summary>
    /// Map way. A way is an ordered list of between 2 and 2,000 Nodes. 
    /// Ways are used to represent linear features, such as rivers or roads.
    /// Ways can also represent solid areas, such as buildings or forests. 
    /// In this case, the first and last node will be the same - a "closed way".
    /// Note that closed ways are occasionally linear loops, such as highway roundabouts, rather than 'filled' areas. 
    /// </summary>
    public class MapWay : Tagged
    {
        /// <summary>
        /// The way map nodes ArrayList. The Nodes that make up this Way.
        /// </summary>
        public ArrayList wayMapNodes;

        /// <summary>
        /// The identifier. A unique ID used for HashTable look ups.
        /// </summary>
        public uint id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mapity.MapWay"/> class.
        /// </summary>
        public MapWay(): base()
        {
            wayMapNodes = new ArrayList();

            id = 0u;
        }
    }

    /// <summary>
    /// The map ways Hashtable.
    /// </summary>
    public Hashtable mapWays = new Hashtable();

    #endregion

    #region Highways

    /// <summary>
    /// Highway classification. An enum used to identify different road types.
    /// </summary>
    public enum HighwayClassification
    {
        Road, // Road of unknown classification ( Default )
        Motorway,
        MotorwayLink,
        Trunk,
        TrunkLink,
        Primary,
        PrimaryLink,
        Secondary,
        SecondaryLink,
        Tertiary,
        TertiaryLink,
        Pedestrian,
        Residential
    }

    /// <summary>
    /// Highway. The Highway class is a convient wrapper to a Way tagged as a Highway.
    /// </summary>
    public class Highway : MapWay
    {
        /// <summary>
        /// The classification.
        /// </summary>
        public HighwayClassification classification;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mapity.Highway"/> class.
        /// </summary>
        public Highway(): base()
        {
            // Default classification to road
            classification = HighwayClassification.Road;
        }
    }

    /// <summary>
    /// The highways Hashtable.
    /// </summary>
    public Hashtable highways = new Hashtable();

    #endregion

    #region Waterways

    /// <summary>
    /// Waterway classification. An enum used to identify different water way types.
    /// </summary>
    public enum WaterwayClassification
    {
        River, // Default
        Stream,
        Canal,
        Lake
    }

    /// <summary>
    /// Waterway. The Waterway class is a convient wrapper to a Way tagged as a Waterway.
    /// </summary>
    public class Waterway : MapWay
    {
        /// <summary>
        /// The classification.
        /// </summary>
        public WaterwayClassification classification;

        /// <summary>
        /// The waterway ways. List of MapWays defining a waterway
        /// </summary>
        public ArrayList waterwayWays;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mapity.Waterway"/> class.
        /// </summary>
        public Waterway(): base()
        {
            id = 0u;

            // Default classification to river
            classification = WaterwayClassification.River;

            waterwayWays = new ArrayList();
        }
    }

    /// <summary>
    /// The waterways Hashtable.
    /// </summary>
    public Hashtable waterways = new Hashtable();

    #endregion

    #region Relations

    /// <summary>
    /// Relation type. An enum used to identify different Relation types.
    /// </summary>
    public enum RelationType
    {
        Multipolygon, // Default
        Route,
        RouteMaster,
        Restriction,
        Boundary,
        Street,
        AssociatedStreet,
        PublicTransport,
        DestinationSign,
        Waterway,
        Enforcement
    }

    /// <summary>
    /// Map relation. A Relation is an all-purpose data structure that documents a relationship between two or more other objects.
    /// Simple examples include:
    /// A Route relation assembles the ways that form a cycle route, bus route or long-distance highway.
    /// A Multipolygon describes an area (the 'outer way') with holes (the 'inner ways').
    /// </summary>
    public class MapRelation : Tagged
    {
        /// <summary>
        /// The relation nodes. List of MapNodes defining a relation
        /// </summary>
        public ArrayList relationNodes;

        /// <summary>
        /// The relation ways. List of MapWays defining a relation
        /// </summary>
        public ArrayList relationWays;

        /// <summary>
        /// The relation relations. List of MapRelations defining a relation
        /// </summary>
        public ArrayList relationRelations;

        /// <summary>
        /// The identifier. A unique ID used for HashTable look ups.
        /// </summary>
        public uint id;

        /// <summary>
        /// The type.
        /// </summary>
        public RelationType type;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mapity.MapRelation"/> class.
        /// </summary>
        public MapRelation(): base()
        {
            relationNodes = new ArrayList();
            relationWays = new ArrayList();
            relationRelations = new ArrayList();

            id = 0u;

            type = RelationType.Multipolygon;
        }
    }

    /// <summary>
    /// The map relations Hashtable.
    /// </summary>
    public Hashtable mapRelations = new Hashtable();

    #endregion

    #region Buildings

    /// <summary>
    /// Building type. An enum used to identify different Building types.
    /// </summary>
    public enum BuildingType
    {
        Building, // Default
    }

    /// <summary>
    /// Map building. The Building class is a convient wrapper to a Way or Relation tagged as a Building.
    /// </summary>
    public class MapBuilding
    {
        /// <summary>
        /// The identifier. A unique ID used for HashTable look ups.
        /// </summary>
        public uint id;

        /// <summary>
        /// The type.
        /// </summary>
        public BuildingType type;

        /// <summary>
        /// The building ways. List of MapWays defining a building
        /// </summary>
        public ArrayList buildingWays;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mapity.MapBuilding"/> class.
        /// </summary>
        public MapBuilding()
        {
            id = 0u;

            type = BuildingType.Building;

            buildingWays = new ArrayList();
        }
    }

    /// <summary>
    /// The map buildings Hashtable.
    /// </summary>
    public Hashtable mapBuildings = new Hashtable();

    #endregion

    #region Functions

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
	{
        if (autoLoad && Application.isPlaying)
        {
            Load();
        }
    }

    /// <summary>
    /// Load. Starts the Loading Coroutine.
    /// </summary>
    public void Load()
	{
        loadingInProgress = true;
        StartCoroutine(LoadingCoroutine());
    }

    /// <summary>
    /// Unload the map data.
    /// </summary>
    public void Unload()
    {
        // Force garbage collection to clear up file handles
        System.GC.Collect();

        AbortParsing();

        // Send event
        if (MapityUnloaded != null)
        {
            MapityUnloaded();
        }

#if !(UNITY_WEBPLAYER || UNITY_WEBGL)
        string tempDirectory = Application.temporaryCachePath + "/Mapity";

        if (Directory.Exists(tempDirectory))
        {
            string[] fileList = Directory.GetFiles(tempDirectory, "*", SearchOption.AllDirectories);

            string file = null;

            for( var fileEnumerator = fileList.GetEnumerator(); fileEnumerator.MoveNext();)
            {
                file = fileEnumerator.Current as string;

                File.Delete(file);
            }

            Directory.Delete(tempDirectory, true);
        }
#endif

        loadingInProgress = false;
        hasLoaded = false;

        mapBounds = new MapBounds();
        mapNodes = new Hashtable();
        mapWays = new Hashtable();
        highways = new Hashtable();
        waterways = new Hashtable();
        mapRelations = new Hashtable();
        mapBuildings = new Hashtable();
    }

    /// <summary>
    /// Checks if the load is complete
    /// </summary>
    void Update()
    {
#if UNITY_EDITOR
		// Workaround for issue(571898-Will Not Fix)
		if (Singleton == null)
		{
			Singleton = this as Mapity;
		}
#endif
        CheckForLoad();
    }

    /// <summary>
    /// Fires the loaded event when loading is finished.
    /// </summary>
    private void CheckForLoad()
    {
        if (HasLoaded() && IsLoadingInProgress())
        {
            loadingInProgress = false;            

            m_lastParsedObject = null;

			SnapToTerrain();

            if( Application.isPlaying )
            {
                StartCoroutine(QueryHeightData());
            }
        }
    }

	/// <summary>
	/// Sends the loaded event.
	/// </summary>
	private void SendLoadedEvent()
	{
		// Send Event
		if (MapityLoaded != null)
		{
			MapityLoaded();
		}
	}

	/// <summary>
	/// Snaps the map nodes to the terrain.
	/// </summary>
	void SnapToTerrain()
	{	
		// Check if we're snapping to terrain	
		if(snapToTerrain)
		{
            MapNode mapNode = null;
            Terrain activeTerrain = null;

            // Loop over all map nodes
            for (var nodeEnumerator = mapNodes.Values.GetEnumerator(); nodeEnumerator.MoveNext();)
			{
				mapNode = nodeEnumerator.Current as MapNode;

				// Check for valid node
				if(mapNode != null)
				{
					// Cache position
					Position world = mapNode.position.world;

					// Temporarily set the elevation negative so we can use the max value return from sampled terrain 
					world.Elevation = Mathf.NegativeInfinity;

					// Loop over all active terrains
					for(var terrainEnumerator = Terrain.activeTerrains.GetEnumerator(); snapToTerrain && terrainEnumerator.MoveNext();)
					{
						activeTerrain = terrainEnumerator.Current as Terrain;

						// Sample the terrain height
						float terrainHeight = activeTerrain.SampleHeight(mapNode.position.world.ToVector());

						// Set the max sampled height, between the sample and the current
						world.Elevation = Mathf.Max( world.Elevation, terrainHeight );
					}

					// If we're still set to our temporary negative value, set it back to zero
					if(world.Elevation == Mathf.NegativeInfinity)
					{
						world.Elevation = 0.0f;
					}
				}
			}
		}
	}

    /// <summary>
    /// Determines whether this instance is loading in progress.
    /// </summary>
    /// <returns><c>true</c> if this instance is loading in progress; otherwise, <c>false</c>.</returns>
    public bool IsLoadingInProgress()
    {
        return loadingInProgress;
    }

    /// <summary>
    /// Determines whether this instance has loaded.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance has loaded; otherwise, <c>false</c>.
    /// </returns>
    public bool HasLoaded()
    {
        return hasLoaded;
    }
    

    /// <summary>
    /// Called by the parsing thread to notify the main thread loading is complete
    /// </summary>
    private void LoadCompleted()
	{
        hasLoaded = true;
    }

    /// <summary>
    /// The loading coroutine.
    /// </summary>
    /// <returns>
    /// The coroutine.
    /// </returns>
    IEnumerator LoadingCoroutine()
    {
        // Reset Mapity load flag
        hasLoaded = false;

        // Local successful load flag
        bool loadSuccessful = true;
        
        WWW www = null;

        // Are we using the online version
        if (downloadMapData)
		{
            if (useDeviceLocation)
            {
                // First, check if user has location service enabled
                if (Input.location.isEnabledByUser)
                {
                    // Start service before querying location
                    Input.location.Start();

                    // Wait until service initializes
                    int maxWait = 20;

                    while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
                    {
                        yield return new WaitForSeconds(1);
                        maxWait--;
                    }

                    // Service didn't initialize in 20 seconds
                    if (maxWait < 1)
                    {
                        MapityLog("Timed out");
                    }
                    else // Not a timeout
                    {
                        // Connection has failed
                        if (Input.location.status == LocationServiceStatus.Failed)
                        {
                            MapityLog("Unable to determine device location");
                        }
                        // Access granted and location value could be retrieved
                        else
                        {
                            MapityLog("Location: " + Input.location.lastData.latitude + " " +
                                   Input.location.lastData.longitude + " " +
                                   Input.location.lastData.altitude + " " +
                                   Input.location.lastData.horizontalAccuracy + " " +
                                   Input.location.lastData.timestamp);

                            location.x = Input.location.lastData.longitude;
                            location.y = Input.location.lastData.latitude;
                        }

                        // Stop service since there is no need to query location updates continuously
                        Input.location.Stop();
                    }
                }
            }

            // Calculate a bounds half - The base zoom level * the current zoom level
            float bounds = baseZoom * (int)mapZoom;

            float longitudeMin = location.x - bounds; // Minus a half bound
            float longitudeMax = location.x + bounds; // Plus a half bound
            float latitudeMin = location.y - bounds; // Minus a half bound
            float latitudeMax = location.y + bounds; // Plus a half bound

            //Load XML data from a URL
            string url = openStreetMapApiUrl + "map?bbox="
                        + longitudeMin.ToString() + ","
                        + latitudeMin.ToString() + ","
                        + longitudeMax.ToString() + ","
                        + latitudeMax.ToString();

            www = new WWW(url);

            //Load the data and yield (wait) till it's ready before we continue executing the rest of this method.
            yield return www;

            // If we have don't an error
            if (www.error == null)
            {
                //Sucessfully loaded the XML
                MapityLog("Loaded following data: " + www.text);

                string filename = "/" + mapFileNameSavePrefix.ToString() + location.x.ToString() + "_"+ location.y.ToString() + ".mapity";
				string path = Application.temporaryCachePath;

                // Should we save what we downloaded
                if (saveDownloadedMapData)
                {
					MapityLog("Saving downloaded map data...");

                    path = Application.persistentDataPath;
                }
                
                SaveMap(filename, path, www.text);
            }
            else // Otherwise something went wrong
            {
                // Mark this as unsuccessful
                loadSuccessful = false;

                //Failed to download data
                MapityLog("Failed to download map XML data: " + www.error);
            }
        }
        else
        {
            SetPath(Application.streamingAssetsPath + "/MapData/" + localMapFileName.ToString() + ".mapity");

			string localFilePath = "";

			#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_WSA
			localFilePath = "file:///" + m_path;
			#elif UNITY_ANDROID && !UNITY_EDITOR
			localFilePath = m_path;
			#else
			localFilePath = "file://" + m_path;
			#endif

			www = new WWW(localFilePath);

			yield return www;

			// If we have don't an error
			if(String.IsNullOrEmpty(www.error))
			{
				memoryStream = new MemoryStream(www.bytes);
				memoryStream.Position = 0;
			}
			else
			{
				MapityError("Localfile error: " + www.error);
			}
        }

        // If we successfully loaded an xml file
        if (loadSuccessful)
		{
            // Get the default bounds of the map based on zoom
            float bounds = baseZoom * (int)mapZoom;

            // Initialise the map bounds based on zoom level( This can be overridden by parsed data )
            mapBounds.Initalise(bounds, location);

#if !(UNITY_WEBPLAYER || UNITY_WEBGL)
            LoadXML();
#else
            Debug.Log("Local map loading not supported for Webplayer builds");

            LoadCompleted();
#endif
        }
    }

    /// <summary>
    /// Saves the map.
    /// </summary>
    /// <param name='mapData'>
    /// Map data.
    /// </param>
    public void SaveMap(string filename, string path, string mapData)
    {
        SetPath( path + filename );

#if !(UNITY_WEBPLAYER || UNITY_WEBGL)

        // Save data
        if ( !Directory.Exists(path) )
		{
            Directory.CreateDirectory(path);
        }

        lock( m_path )
        {
			File.WriteAllText(m_path, mapData);
        }
#else
        Debug.Log("Map Saving not supported for Webplayer builds");
#endif

    }

    /// <summary>
    /// Adds the highway.
    /// </summary>
    /// <param name='mapWay'>
    /// Map way.
    /// </param>
    /// <param name='tagNode'>
    /// Tag node.
    /// </param>
    private void AddHighway(MapWay mapWay, string nodeValue)
    {
        Highway highway = new Highway();

        highway.id = mapWay.id;

        highway.wayMapNodes = mapWay.wayMapNodes;

        switch (nodeValue)
        {
            case "motorway":
                {
                    highway.classification = HighwayClassification.Motorway;
                    break;
                }
            case "motorway_link":
                {
                    highway.classification = HighwayClassification.MotorwayLink;
                    break;
                }
            case "trunk":
                {
                    highway.classification = HighwayClassification.Trunk;
                    break;
                }
            case "trunk_link":
                {
                    highway.classification = HighwayClassification.TrunkLink;
                    break;
                }
            case "primary":
                {
                    highway.classification = HighwayClassification.Primary;
                    break;
                }
            case "primary_link":
                {
                    highway.classification = HighwayClassification.PrimaryLink;
                    break;
                }
            case "secondary":
                {
                    highway.classification = HighwayClassification.Secondary;
                    break;
                }
            case "secondary_link":
                {
                    highway.classification = HighwayClassification.SecondaryLink;
                    break;
                }
            case "tertiary":
                {
                    highway.classification = HighwayClassification.Tertiary;
                    break;
                }
            case "tertiary_link":
                {
                    highway.classification = HighwayClassification.TertiaryLink;
                    break;
                }
            case "living_street":
            case "pedestrian":
                {
                    highway.classification = HighwayClassification.Pedestrian;
                    break;
                }
            case "residential":
                {
                    highway.classification = HighwayClassification.Residential;
                    break;
                }
        }
        MapityLog("Highway classification: " + highway.classification.ToString());

        // Add highway to Hashtable of highways
        highways.Add(highway.id, highway);
        MapityLog("Added Highway ID:" + highway.id);
    }

    /// <summary>
    /// Adds the waterway.
    /// </summary>
    /// <param name='ways'>
    /// Ways. List of MapWays.
    /// </param>
    /// <param name='tagNode'>
    /// Tag node. Tag Information.
    /// </param>
    /// <param name='id'>
    /// Identifier.
    /// </param>
    private void AddWaterway(ArrayList ways, string nodeValue, uint id)
    {
        Waterway waterway = new Waterway();

        waterway.id = id;

        waterway.waterwayWays = ways;

        switch (nodeValue)
        {
            case "river":
            case "riverbank":
                {
                    waterway.classification = WaterwayClassification.River;
                    break;
                }
            case "stream":
                {
                    waterway.classification = WaterwayClassification.Stream;
                    break;
                }
            case "canal":
                {
                    waterway.classification = WaterwayClassification.Canal;
                    break;
                }
            case "water":
                {
                    waterway.classification = WaterwayClassification.Lake;
                    break;
                }
        }
        MapityLog("Waterway classification: " + waterway.classification.ToString());

        // Add waterway to Hashtable of waterways
        waterways.Add(waterway.id, waterway);
        MapityLog("Added Waterway ID:" + waterway.id);
    }

    /// <summary>
    /// Adds the building.
    /// </summary>
    /// <param name='ways'>
    /// Ways. List of MapWays.
    /// </param>
    /// <param name='tagNode'>
    /// Tag node. Tag Information.
    /// </param>
    /// <param name='id'>
    /// Identifier.
    /// </param>
    private void AddBuilding(ArrayList ways, string nodeValue, uint id)
    {
        MapBuilding mapBuilding = new MapBuilding();

        // Use way ID for building ID since this way is defining a building
        mapBuilding.id = id;

        switch (nodeValue)
        {
            case "yes":
                {
                    mapBuilding.type = BuildingType.Building;
                    break;
                }
        }
        MapityLog("Building type: " + mapBuilding.type.ToString());

        mapBuilding.buildingWays = ways;

        // Add mapBuilding to Hashtable of mapBuildings
        mapBuildings.Add(mapBuilding.id, mapBuilding);
        MapityLog("Added MapBuilding ID:" + mapBuilding.id);
    }

    /// <summary>
    /// Raises the draw gizmos event.
    /// </summary>
    public void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Handles.Label(transform.position, transform.name);
#endif

        Gizmos.DrawIcon(transform.position, "Map.png", true);

        if (persistentGizmos)
        {
            OnDrawGizmosSelected();
        }
    }

    /// <summary>
    /// Raises the draw gizmos selected event.
    /// </summary>
    public void OnDrawGizmosSelected()
    {
        if (!HasLoaded())
            return;

        if (gizmoDrawNodes)
        {
            MapNode mapNode = null;

            for ( var mapNodeEnumerator = mapNodes.Values.GetEnumerator(); mapNodeEnumerator.MoveNext();)
            {
                mapNode = mapNodeEnumerator.Current as MapNode;

                Gizmos.DrawIcon(mapNode.position.world.ToVector(), "MapNode.png", true);
            }
        }

        Gizmos.color = Color.white;

        // Ways
        if (gizmoDrawWays)
        {
            MapWay mapWay = null;

            for( var mapWayEnumerator = mapWays.Values.GetEnumerator(); mapWayEnumerator.MoveNext();)
            {
                mapWay = mapWayEnumerator.Current as MapWay;

                Gizmos.color = Color.white;

                for (int i = 0; i < mapWay.wayMapNodes.Count - 1; i++)
                {
                    MapNode fromNode = (MapNode)mapWay.wayMapNodes[i];
                    MapNode toNode = (MapNode)mapWay.wayMapNodes[i + 1];

                    Gizmos.DrawLine(fromNode.position.world.ToVector(), toNode.position.world.ToVector());
                }
            }
        }

        // Highways
        if (gizmoDrawHighWays || gizmoDrawHighWaysLabels)
        {
            Highway highway = null;
            
            for(var highwayEnumerator = highways.Values.GetEnumerator(); highwayEnumerator.MoveNext();)
            {
                highway = highwayEnumerator.Current as Highway;

                Gizmos.color = Color.white;

                switch (highway.classification)
                {
                    case HighwayClassification.Motorway:
                        {
                            Gizmos.color = Color.blue;
                            break;
                        }
                    case HighwayClassification.MotorwayLink:
                        {
                            Gizmos.color = Color.blue;
                            break;
                        }
                    case HighwayClassification.Trunk:
                        {
                            Gizmos.color = Color.green;
                            break;
                        }
                    case HighwayClassification.TrunkLink:
                        {
                            Gizmos.color = Color.green;
                            break;
                        }
                    case HighwayClassification.Primary:
                        {
                            Gizmos.color = Color.red;
                            break;
                        }
                    case HighwayClassification.PrimaryLink:
                        {
                            Gizmos.color = Color.red;
                            break;
                        }
                    case HighwayClassification.Secondary:
                        {
                            // Orange
                            Gizmos.color = new Color(1.0f, 0.6f, 0.0f);
                            break;
                        }
                    case HighwayClassification.SecondaryLink:
                        {
                            // Orange
                            Gizmos.color = new Color(1.0f, 0.6f, 0.0f);
                            break;
                        }
                    case HighwayClassification.Tertiary:
                        {
                            Gizmos.color = Color.yellow;
                            break;
                        }
                    case HighwayClassification.TertiaryLink:
                        {
                            Gizmos.color = Color.yellow;
                            break;
                        }
                    case HighwayClassification.Pedestrian:
                        {
                            Gizmos.color = Color.cyan;
                            break;
                        }
                    case HighwayClassification.Residential:
                        {
                            Gizmos.color = Color.gray;
                            break;
                        }
                }
#if UNITY_EDITOR
                if (gizmoDrawHighWaysLabels)
                {
                    MapNode way = (MapNode)highway.wayMapNodes[(int)highway.wayMapNodes.Count / 2];
                    Handles.Label(way.position.world.ToVector(), "Highway #" + highway.id.ToString() + ",\n" + highway.classification.ToString());
                }
#endif

                if (gizmoDrawHighWays)
                {
                    for (int i = 0; i < highway.wayMapNodes.Count - 1; i++)
                    {
                        MapNode fromNode = (MapNode)highway.wayMapNodes[i];
                        MapNode toNode = (MapNode)highway.wayMapNodes[i + 1];

                        Gizmos.DrawLine(fromNode.position.world.ToVector(), toNode.position.world.ToVector());
                    }
                }
            }
        }

        // Waterways
        if (gizmoDrawWaterWays)
        {
            Waterway waterway = null;

            for (var waterwayEnumerator = waterways.Values.GetEnumerator(); waterwayEnumerator.MoveNext();)
            {
                waterway = waterwayEnumerator as Waterway;

                Gizmos.color = Color.cyan;

                switch (waterway.classification)
                {
                    case WaterwayClassification.River:
                        {
                            Gizmos.color = Color.cyan;
                            break;
                        }
                    case WaterwayClassification.Stream:
                        {
                            Gizmos.color = Color.cyan;
                            break;
                        }
                    case WaterwayClassification.Canal:
                        {
                            Gizmos.color = Color.cyan;
                            break;
                        }
                }
                for (int i = 0; i < waterway.waterwayWays.Count; i++)
                {
                    MapWay waterwayWay = (MapWay)waterway.waterwayWays[i];

                    if (waterwayWay != null)
                    {
                        for (int j = 0; j < waterwayWay.wayMapNodes.Count - 1; j++)
                        {
                            MapNode fromNode = (MapNode)waterwayWay.wayMapNodes[j];
                            MapNode toNode = (MapNode)waterwayWay.wayMapNodes[j + 1];

                            Gizmos.DrawLine(fromNode.position.world.ToVector(), toNode.position.world.ToVector());
                        }
                    }
                }
            }
        }

        // Buildings
        if (gizmoDrawBuildings)
        {
            MapBuilding mapBuilding = null;

            for( var mapBuildingEnumerator = mapBuildings.Values.GetEnumerator(); mapBuildingEnumerator.MoveNext();)
            {
                mapBuilding = mapBuildingEnumerator.Current as MapBuilding;

                Gizmos.color = Color.green;

                switch (mapBuilding.type)
                {
                    case BuildingType.Building:
                        {
                            Gizmos.color = Color.magenta;
                            break;
                        }
                }

                for (int i = 0; i < mapBuilding.buildingWays.Count; i++)
                {
                    MapWay buildingWay = (MapWay)mapBuilding.buildingWays[i];

                    if (buildingWay != null)
                    {
                        for (int j = 0; j < buildingWay.wayMapNodes.Count - 1; j++)
                        {
                            MapNode fromNode = (MapNode)buildingWay.wayMapNodes[j];
                            MapNode toNode = (MapNode)buildingWay.wayMapNodes[j + 1];

                            Gizmos.DrawLine(fromNode.position.world.ToVector(), toNode.position.world.ToVector());
                        }
                    }
                }
            }
        }

        // Relations
        if (gizmoDrawRelations)
        {
            MapRelation mapRelation = null;

            for (var mapRelationEnumerator = mapRelations.Values.GetEnumerator(); mapRelationEnumerator.MoveNext();)
            {
                mapRelation = mapRelationEnumerator.Current as MapRelation;

                Gizmos.color = Color.yellow;

                for (int i = 0; i < mapRelation.relationWays.Count; i++)
                {
                    MapWay relationWay = (MapWay)mapRelation.relationWays[i];

                    if (relationWay != null)
                    {
                        for (int j = 0; j < relationWay.wayMapNodes.Count - 1; j++)
                        {
                            MapNode fromNode = (MapNode)relationWay.wayMapNodes[j];
                            MapNode toNode = (MapNode)relationWay.wayMapNodes[j + 1];

                            Gizmos.DrawLine(fromNode.position.world.ToVector(), toNode.position.world.ToVector());
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Custom Debug Log we can switch off - Logging is very slow.
    /// </summary>
    /// <param name='message'>
    /// Message.
    /// </param>
    public static void MapityLog(string message)
    {
		if (Mapity.Singleton.enableLogging)
        {
            Debug.Log(message);
        }
    }

	/// <summary>
	/// Custom Debug Error we can switch off.
	/// </summary>
	/// <param name='message'>
	/// Message.
	/// </param>
	public static void MapityError(string message)
	{
		if (Mapity.Singleton.enableErrors)
		{
			Debug.LogError(message);
		}
	}

    /// <summary>
    /// Sets the path at which Map-ity will store temporary data
    /// </summary>
    /// <param name="path"></param>
    private void SetPath(string path)
	{
        lock (m_path)
        {
            m_path = path;
        }
    }

    /// <summary>
    /// Starts the threaded xml parsing job
    /// </summary>
    private void LoadXML()
	{
        ThreadStart job = new ThreadStart(ThreadFunc);
        Thread thread = new Thread(job);
        thread.Name = "XML2OSM";
        thread.Start();
    }
    
    /// <summary>
    /// Abort job flag
    /// </summary>
    private bool bAbortThread = false;

    /// <summary>
    /// Abort the threaded job
    /// </summary>
    private void AbortParsing()
    {
        bAbortThread = true;
    }

	/// <summary>
	/// The memory stream used when loading local files.
	/// </summary>
	private MemoryStream memoryStream = null;

    /// <summary>
    /// XML parsing job which runs in the XML2OSM thread
    /// </summary>
    private void ThreadFunc()
    {
		StreamReader sr = null;
		FileStream fileStream = null;

        bAbortThread = false;

        // Create parser
        XML2OSM xml2OsmParser = new XML2OSM();

        // Register Actions
        xml2OsmParser.OnParseTagAction += OnParseTag;
        xml2OsmParser.OnParseWayNodeRefAction += OnParseWayNodeRef;
        xml2OsmParser.OnParseNodeAction += OnParseNode;
        xml2OsmParser.OnParseMemberAction += OnParseMember;
        xml2OsmParser.OnParseWayAction += OnParseWay;
        xml2OsmParser.OnParseRelationAction += OnParseRelation;
        xml2OsmParser.OnParseBoundAction += OnParseBound;

		if(memoryStream == null)
		{
			lock(m_path)
			{
				// Read file path
				fileStream = File.Open(m_path, FileMode.Open, FileAccess.Read, FileShare.Read);
				sr = new StreamReader(fileStream);
			}

		} 
		else
		{
			lock(memoryStream)
			{
				sr = new StreamReader(memoryStream);
			}
		}

		string line;

		// Read data a line at a time
		while(!bAbortThread && sr.Peek() >= 0)
		{
			line = sr.ReadLine();
			xml2OsmParser.ParseLine(line);
		}

		memoryStream = null;

        // Unregister Actions
        xml2OsmParser.OnParseTagAction -= OnParseTag;
        xml2OsmParser.OnParseWayNodeRefAction -= OnParseWayNodeRef;
        xml2OsmParser.OnParseNodeAction -= OnParseNode;
        xml2OsmParser.OnParseMemberAction -= OnParseMember;
        xml2OsmParser.OnParseWayAction -= OnParseWay;
        xml2OsmParser.OnParseRelationAction -= OnParseRelation;
        xml2OsmParser.OnParseBoundAction -= OnParseBound;

        // Set the load completed flag on the main thread
        LoadCompleted();
    }
    
    /// <summary>
    /// The last object the parser parsed.
    /// </summary>
    object m_lastParsedObject = null;

    /// <summary>
    /// Callback for parsing a tag
    /// </summary>
    /// <param name="strKey"></param>
    /// <param name="strValue"></param>
    private void OnParseTag(string strKey, string strValue)
    {
        Tagged taggedObject = m_lastParsedObject != null ? m_lastParsedObject as Tagged : null;

        if (taggedObject != null)
        {
            taggedObject.tags.AddTag(strKey, strValue);
        }

        MapWay mapWay = m_lastParsedObject != null ? m_lastParsedObject as MapWay : null;

        if (mapWay != null)
        {
            ArrayList ways = new ArrayList();

            // Add the map way
            ways.Add(mapWay);

            if (strKey == "highway")
            {
                //Add the highway
                AddHighway(mapWay, strValue);
            }
            else if (strKey == "waterway")
            {
                // Avoid Hashtable collisions due to bad map data reusing ways e.g. a way tagged as both waterway and natural-water
                if (!waterways.Contains(mapWay.id))
                {
                    // Add the waterway
                    AddWaterway(ways, strValue, mapWay.id);
                }
            }
            else if (strKey == "natural")
            {
                if (strValue == "water")
                {
                    // Avoid Hashtable collisions due to bad map data reusing ways e.g. a way tagged as both waterway and natural-water
                    if (!waterways.Contains(mapWay.id))
                    {
                        // Add the waterway
                        AddWaterway(ways, strValue, mapWay.id);
                    }
                }
            }
            else if (strKey == "building")
            {
                // Add the building
                AddBuilding(ways, strValue, mapWay.id);
            }
        }

        MapRelation mapRelation = m_lastParsedObject != null ? m_lastParsedObject as MapRelation : null;

        if (mapRelation != null)
        {
            if (strKey == "type")
            {
                switch (strValue)
                {
                    case "multipolygon":
                        {
                            mapRelation.type = RelationType.Multipolygon;
                            break;
                        }
                    case "route":
                        {
                            mapRelation.type = RelationType.Route;
                            break;
                        }
                    case "restriction":
                        {
                            mapRelation.type = RelationType.Restriction;
                            break;
                        }
                    case "street":
                        {
                            mapRelation.type = RelationType.Street;
                            break;
                        }
                    case "associatedStreet":
                        {
                            mapRelation.type = RelationType.AssociatedStreet;
                            break;
                        }
                    case "public_transport":
                        {
                            mapRelation.type = RelationType.PublicTransport;
                            break;
                        }
                    case "destination_sign":
                        {
                            mapRelation.type = RelationType.DestinationSign;
                            break;
                        }
                    case "waterway":
                        {
                            mapRelation.type = RelationType.Waterway;
                            break;
                        }
                    case "enforcement":
                        {
                            mapRelation.type = RelationType.Enforcement;
                            break;
                        }
                }
                MapityLog("Relation type: " + mapRelation.type.ToString());
            }
            else if (strKey == "building")
            {
                AddBuilding(mapRelation.relationWays, strValue, mapRelation.id);
            }
            else if (strKey == "natural")
            {
                if (strValue == "water")
                {
                    AddWaterway(mapRelation.relationWays, strValue, mapRelation.id);
                }
            }
        }
    }

    /// <summary>
    /// Callback for parsing a way node ref
    /// </summary>
    /// <param name="uRef"></param>
    private void OnParseWayNodeRef(uint uRef)
    {
        MapWay mapWay = m_lastParsedObject != null ? m_lastParsedObject as MapWay : null;

        if (mapWay != null)
        {
            mapWay.wayMapNodes.Add(mapNodes[uRef]);

            MapityLog("Current Way ID: " + mapWay.id + " Adding node ID: " + uRef);
        }
    }

    /// <summary>
    /// Callback for parsing a node
    /// </summary>
    /// <param name="uId"></param>
    /// <param name="fLat"></param>
    /// <param name="fLon"></param>
    private void OnParseNode(uint uId, float fLat, float fLon)
    {
        // Reset last parsed object
        m_lastParsedObject = null;

        // Create MapNode
        MapNode mapNode = new MapNode();

        // Set Id
        mapNode.id = uId;

        // Set Geographic coordinates ( Lattitude & Longitude )
        mapNode.position.geographic.Latitude = fLat;
        mapNode.position.geographic.Longitude = fLon;

        // Convert Geographic coordinates to Unity world coordinates
        mapNode.position.world = mapBounds.GeographicToWorldCoordinate(mapNode.position.geographic);

        // Add mapNode to Hashtable of mapNodes
        mapNodes.Add(mapNode.id, mapNode);
        MapityLog("Added MapNode ID:" + mapNode.id);

        m_lastParsedObject = mapNode;
    }

    /// <summary>
    /// Callback for parsing a member
    /// </summary>
    /// <param name="strType"></param>
    /// <param name="uRef"></param>
    private void OnParseMember(string strType, uint uRef)
    {
        MapRelation mapRelation = m_lastParsedObject != null ? m_lastParsedObject as MapRelation : null;

        if (mapRelation != null)
        {
            if (strType == "node")
            {
                mapRelation.relationNodes.Add(mapNodes[uRef]);

                MapityLog("Adding node ID: " + uRef + " to relation ID:" + mapRelation.id);
            }
            else if (strType == "way")
            {
                mapRelation.relationWays.Add(mapWays[uRef]);

                MapityLog("Adding way ID: " + uRef + " to relation ID:" + mapRelation.id);
            }
            else if (strType == "relation")
            {
                mapRelation.relationRelations.Add(mapRelations[uRef]);

                MapityLog("Adding relation ID: " + uRef + " to relation ID:" + mapRelation.id);
            }
        }
    }

    /// <summary>
    /// Callback for parsing a way
    /// </summary>
    /// <param name="uId"></param>
    private void OnParseWay(uint uId)
    {
        // Reset last parsed object
        m_lastParsedObject = null;

        MapWay mapWay = new MapWay();

        mapWay.id = uId;

        // Add mapWay to Hashtable of mapWays
        mapWays.Add(mapWay.id, mapWay);
        MapityLog("Added MapWay ID:" + mapWay.id);

        m_lastParsedObject = mapWay;
    }

    /// <summary>
    /// Callback for parsing a relation
    /// </summary>
    /// <param name="uId"></param>
    private void OnParseRelation(uint uId)
    {
        // Reset last parsed object
        m_lastParsedObject = null;

        MapRelation mapRelation = new MapRelation();

        // Get Relation ID				
        mapRelation.id = uId;
        MapityLog("Relation ID:" + mapRelation.id.ToString());

        // Add mapRelation to Hashtable of mapRelations
        mapRelations.Add(mapRelation.id, mapRelation);
        MapityLog("Added MapRelation ID:" + mapRelation.id);

        m_lastParsedObject = mapRelation;
    }

    /// <summary>
    /// Callback for parsing a bound
    /// </summary>
    /// <param name="fMinlat"></param>
    /// <param name="fMinlon"></param>
    /// <param name="fMaxlat"></param>
    /// <param name="fMaxlon"></param>
    private void OnParseBound(float fMinlat, float fMinlon, float fMaxlat, float fMaxlon)
    {
        // Query map data to calculate bounds
        mapBounds.min.geographic.Latitude = fMinlat;
        mapBounds.min.geographic.Longitude = fMinlon;
        mapBounds.max.geographic.Latitude = fMaxlat;
        mapBounds.max.geographic.Longitude = fMaxlon;

        // Calculate center
        mapBounds.CalculateBoundsCenter();

        MapityLog(mapBounds.ToString());
    }

    /// <summary>
    /// ToDo
    /// </summary>
    IEnumerator QueryHeightData()
    {
        // If we are querying height data
        if (queryHeightData)
        {
            // Strings used for height data queries
            string lats = "lats=";
            string lngs = "lngs=";

            // Array of temporary mapNodes that will have their height data queried
            List<MapNode> tempMapNodeList = new List<MapNode>();

            MapNode mapNode = null;

            for( var mapNodeEnumerator = mapNodes.Values.GetEnumerator(); mapNodeEnumerator.MoveNext();)
            {
                mapNode = mapNodeEnumerator.Current as MapNode;

                // Add the node to the query list
                tempMapNodeList.Add(mapNode);

                // Construct the query string
                lngs += mapNode.position.geographic.Longitude.ToString();
                lats += mapNode.position.geographic.Latitude.ToString();

                // Query every 20 points( free account limit ) OR at the end of the list
                if ((tempMapNodeList.Count % 20 == 0) || (tempMapNodeList.Count == Singleton.mapNodes.Count - 1))
                {
                    // Construct the URL
                    string url = geonamesApiUrl
                        + lats
                        + "&"
                        + lngs
                        + "&username="
                        + geonamesUsername.ToString();

                    WWW www = new WWW(url);

                    //Load the data and yield (wait) till it's ready before we continue executing the rest of this method.
                    yield return www;

                    // String contained in geonames errors
                    string geonamesErrorCode = "ERR";

                    // Did the www request succeed
                    bool wwwSuccess = (www.error == null);

                    // If it succeed, check if it contains an error
                    bool geonamesSuccess = wwwSuccess ? !www.text.Contains(geonamesErrorCode) : false;
                    
                    MapNode tempnode = null;

                    // If we have don't an error
                    if (geonamesSuccess)
                    {
                        //Sucessfully loaded the XML
                        MapityLog("Height Query succeeded");

                        // Convert the string to an int array
                        int[] ia = www.text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Select(n => Convert.ToInt32(n)).ToArray();

                        int count = 0;

                        // Set the height on every map node in our query list
                        for(var tempnodeEnumerator = tempMapNodeList.GetEnumerator(); tempnodeEnumerator.MoveNext();)
                        {
                            tempnode = tempnodeEnumerator.Current as MapNode;
                            tempnode.position.geographic.Elevation = ia[count] == geonamesNoHeightData ? 0 : ia[count];
                            tempnode.position.world.Elevation = ia[count] == geonamesNoHeightData ? 0 : ia[count];
                            count++;
                        }
                    }
                    else // Otherwise something went wrong
                    {
                        //Failed to download data, output why
                        MapityLog("Height Query failed: " + (wwwSuccess ? www.text : www.error));

                        int count = 0;

                        // Set the height on every map node in our query list to 0
                        for (var tempnodeEnumerator = tempMapNodeList.GetEnumerator(); tempnodeEnumerator.MoveNext();)
                        {
                            tempnode = tempnodeEnumerator.Current as MapNode;
                            tempnode.position.geographic.Elevation = 0;
                            tempnode.position.world.Elevation = 0;
                            count++;
                        }
                    }

                    // Clear the list ready for the next set of map nodes
                    tempMapNodeList.Clear();

                    // Reset the query string
                    lats = "lats=";
                    lngs = "lngs=";

                    // Skip adding the commas on a new set of nodes
                    continue;
                }

                // Construct query string, locations are comma delimited
                lats += ",";
                lngs += ",";
            }
		}

        // Fully loaded now, send event
		SendLoadedEvent();
    }
}

#endregion

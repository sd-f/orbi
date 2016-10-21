using UnityEngine;
using System;
using System.Reflection;
using UnityEditor;

[CustomEditor(typeof(Mapity))]
public class MapityEditor : Editor 
{
    // Buttons
	static bool settings = false;
	static bool location = false;
	static bool osm = false;
	static bool geonames = false;
	static bool worldcomposer = false;
	static bool saveLoad = false;
	static bool gizmos = false;
	static bool debug = false;

    // Loading
	static int  dotCount = 0;

    // World Composer
	static bool useWorldComposerLocationData = true;
	object      worldComposerObject = null;

    // Icon
    Texture   mapityIcon = null;

	// Properties
	SerializedProperty autoLoadProp;
	SerializedProperty downloadMapDataProp;
	SerializedProperty openStreetMapApiUrlProp;
	SerializedProperty queryHeightDataProp;
	SerializedProperty snapToTerrainProp;
	SerializedProperty geonamesApiUrlProp;
	SerializedProperty geonamesUsernameProp;
	SerializedProperty useDeviceLocationProp;
	SerializedProperty locationProp;
	SerializedProperty localMapFileNameProp;
	SerializedProperty saveDownloadedMapDataProp;
	SerializedProperty mapFileNameSavePrefixProp;
	SerializedProperty enableLoggingProp;
	SerializedProperty enableErrorsProp;
	SerializedProperty persistentGizmosProp;
	SerializedProperty gizmoDrawNodesProp;
	SerializedProperty gizmoDrawWaysProp;
	SerializedProperty gizmoDrawHighWaysProp;
	SerializedProperty gizmoDrawHighWaysLabelsProp;
	SerializedProperty gizmoDrawWaterWaysProp;
	SerializedProperty gizmoDrawBuildingsProp;
	SerializedProperty gizmoDrawRelationsProp;
	SerializedProperty mapZoomProp;
	SerializedProperty overrideDefaultBehaviourModeProp;
	SerializedProperty mapityDefaultBehaviourModeProp;
	SerializedProperty worldToUnityScaleProp;

    /// <summary>
    /// OnEnable
    /// </summary>
    void OnEnable()
    {
        // Load Icon
        mapityIcon = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Gizmos/Logo.png", typeof(Texture));

        // World Composer Support
        Type worldComposerMapDataType = Type.GetType("Map_tc, Assembly-UnityScript-Editor");

		if( worldComposerMapDataType != null )
		{
			object[] worldComposerArray = Resources.FindObjectsOfTypeAll(worldComposerMapDataType);

			if(worldComposerArray.Length >0)
			{
				worldComposerObject = worldComposerArray[0];
			}
		}

		// Setup properties
		autoLoadProp = serializedObject.FindProperty("autoLoad");
		downloadMapDataProp = serializedObject.FindProperty("downloadMapData");
		openStreetMapApiUrlProp = serializedObject.FindProperty("openStreetMapApiUrl");
		queryHeightDataProp = serializedObject.FindProperty("queryHeightData");
		snapToTerrainProp = serializedObject.FindProperty("snapToTerrain");
		geonamesApiUrlProp = serializedObject.FindProperty("geonamesApiUrl");
		geonamesUsernameProp = serializedObject.FindProperty("geonamesUsername");
		useDeviceLocationProp = serializedObject.FindProperty("useDeviceLocation");
		locationProp = serializedObject.FindProperty("location");
		localMapFileNameProp = serializedObject.FindProperty("localMapFileName");
		saveDownloadedMapDataProp = serializedObject.FindProperty("saveDownloadedMapData");
		mapFileNameSavePrefixProp = serializedObject.FindProperty("mapFileNameSavePrefix");
		enableLoggingProp = serializedObject.FindProperty("enableLogging");
		enableErrorsProp = serializedObject.FindProperty("enableErrors");
		persistentGizmosProp = serializedObject.FindProperty("persistentGizmos");
		gizmoDrawNodesProp = serializedObject.FindProperty("gizmoDrawNodes");
		gizmoDrawWaysProp = serializedObject.FindProperty("gizmoDrawWays");
		gizmoDrawHighWaysProp = serializedObject.FindProperty("gizmoDrawHighWays");
		gizmoDrawHighWaysLabelsProp = serializedObject.FindProperty("gizmoDrawHighWaysLabels");
		gizmoDrawWaterWaysProp = serializedObject.FindProperty("gizmoDrawWaterWays");
		gizmoDrawBuildingsProp = serializedObject.FindProperty("gizmoDrawBuildings");
		gizmoDrawRelationsProp = serializedObject.FindProperty("gizmoDrawRelations");
		mapZoomProp = serializedObject.FindProperty("mapZoom");
		overrideDefaultBehaviourModeProp = serializedObject.FindProperty("overrideDefaultBehaviourMode");
		mapityDefaultBehaviourModeProp = serializedObject.FindProperty("mapityDefaultBehaviourMode");
		worldToUnityScaleProp = serializedObject.FindProperty("worldToUnityScale");
    }

    /// <summary>
    /// OnInspectorGUI
    /// </summary>
    public override void OnInspectorGUI()
	{
		serializedObject.Update();

		Mapity mapityScript = (Mapity)target;

        #region SceneEditor
        EditorGUILayout.LabelField("Scene Editor", EditorStyles.largeLabel, GUILayout.MinHeight(18));
        GUILayout.BeginHorizontal();
        if (!mapityScript.IsLoadingInProgress())
        {
            if (GUILayout.Button("Build Map"))
            {
                if (mapityScript.HasLoaded())
                {
                    mapityScript.Unload();
                }
                mapityScript.Load();

                dotCount = 0;

                EditorUtility.SetDirty(mapityScript);
            }
        }
        else
        {
            if (GUILayout.Button("Cancel Build"))
            {
                mapityScript.Unload();

                EditorUtility.SetDirty(mapityScript);
            }
        }
        GUILayout.EndHorizontal();

        string boxText = "Ready";

        if (mapityScript.IsLoadingInProgress())
        {
            dotCount = dotCount % 11;

            boxText = "Loading";

            for (int i = 0; i < dotCount; i++)
            {
                boxText += " .";
            }

            dotCount++;

            EditorUtility.SetDirty(mapityScript);
        }

        EditorGUILayout.HelpBox(boxText, MessageType.Info);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear Map"))
        {
            mapityScript.Unload();

            EditorUtility.SetDirty(mapityScript);
        }
        GUILayout.EndHorizontal();

        #endregion

        #region Location
        EditorGUILayout.LabelField("Location", EditorStyles.largeLabel, GUILayout.MinHeight(18));
        location = EditorGUILayout.Foldout(location, "");
        if (location)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 60;

			float mapLongitude = locationProp.vector2Value.x;
            float mapLatitude = locationProp.vector2Value.y;

            // World Composer current area support
            if (worldComposerObject != null && useWorldComposerLocationData)
            {
                Type WorldComposerType = worldComposerObject.GetType();
                FieldInfo currentAreaFieldInfo = WorldComposerType.GetField("current_area");
                object currentArea = currentAreaFieldInfo.GetValue(worldComposerObject);

                if (currentArea != null)
                {
                    Type CurrentAreaType = currentArea.GetType();
                    FieldInfo centerFieldInfo = CurrentAreaType.GetField("center");
                    object center = centerFieldInfo.GetValue(currentArea);

                    if (center != null)
                    {
                        Type centerType = center.GetType();
                        FieldInfo longitudeFieldInfo = centerType.GetField("longitude");
                        object longitudeObject = longitudeFieldInfo.GetValue(center);
                        float longitude = Convert.ToSingle(longitudeObject);

                        mapLongitude = longitude;

                        FieldInfo latitudeFieldInfo = centerType.GetField("latitude");
                        object latitudeObject = latitudeFieldInfo.GetValue(center);
                        float latitude = Convert.ToSingle(latitudeObject);

                        mapLatitude = latitude;
                    }
                }
            }

			mapLatitude = EditorGUILayout.FloatField("Latitude", mapLatitude);
			mapLatitude = Mathf.Clamp(mapLatitude, -90.0f, 90.0f);

			mapLongitude = EditorGUILayout.FloatField("Longitude", mapLongitude);
			mapLongitude = Mathf.Clamp(mapLongitude, -180.0f, 180.0f);

			locationProp.vector2Value = new Vector2(mapLongitude, mapLatitude);
            EditorGUILayout.EndHorizontal();

            EditorGUIUtility.labelWidth = 180;
			useDeviceLocationProp.boolValue = EditorGUILayout.Toggle("Use Mobile Device Location",useDeviceLocationProp.boolValue);
			EditorGUILayout.Space();
            EditorGUIUtility.labelWidth = 0;
        }
        #endregion

        #region Settings
        EditorGUILayout.LabelField("Settings", EditorStyles.largeLabel, GUILayout.MinHeight(18) );
		settings = EditorGUILayout.Foldout( settings, "" );
		if(settings)
		{
			autoLoadProp.boolValue = EditorGUILayout.Toggle("Auto Load Data", autoLoadProp.boolValue );
			downloadMapDataProp.boolValue = EditorGUILayout.BeginToggleGroup("Download Map Data", downloadMapDataProp.boolValue );
			EditorGUILayout.PropertyField(mapZoomProp);
			EditorGUILayout.EndToggleGroup();

			float worldToUnityScale = EditorGUILayout.FloatField("World To Unity Scale:", mapityScript.worldToUnityScale);
			worldToUnityScaleProp.floatValue = Mathf.Clamp( worldToUnityScale, 1.0f, 1000.0f );

			EditorGUILayout.PropertyField(snapToTerrainProp);
			EditorGUILayout.PropertyField(localMapFileNameProp, new GUIContent("Offline Map Filename:"));

			overrideDefaultBehaviourModeProp.boolValue = EditorGUILayout.BeginToggleGroup("Override Default Behaviour Mode", overrideDefaultBehaviourModeProp.boolValue );	
			EditorGUILayout.PropertyField(mapityDefaultBehaviourModeProp);
			EditorGUILayout.EndToggleGroup();

			EditorGUILayout.Space();
		}
		#endregion
		
		#region Open Street Map
		EditorGUILayout.LabelField("Open Street Map", EditorStyles.largeLabel, GUILayout.MinHeight(18) );
		osm = EditorGUILayout.Foldout( osm, "" );
		if(osm)
		{
			EditorGUILayout.PropertyField(openStreetMapApiUrlProp, new GUIContent("Api URL:"));
			EditorGUILayout.Space();
		}
		#endregion
		
		#region Geonames
		EditorGUILayout.LabelField("Geonames", EditorStyles.largeLabel, GUILayout.MinHeight(18) );
		geonames = EditorGUILayout.Foldout( geonames, "" );
		if(geonames)
		{
			EditorGUILayout.PropertyField(queryHeightDataProp);
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(geonamesApiUrlProp, new GUIContent("Api URL:"));
			EditorGUILayout.PropertyField(geonamesUsernameProp, new GUIContent("Username:"));
			EditorGUILayout.HelpBox("Remember to visit http://www.geonames.org/manageaccount and enable the free web service.", MessageType.Info );
		}
		#endregion

		#region WorldComposer
		if( worldComposerObject != null )
		{
			EditorGUILayout.LabelField("WorldComposer", EditorStyles.largeLabel, GUILayout.MinHeight(18) );
			worldcomposer = EditorGUILayout.Foldout( worldcomposer, "" );
			if(worldcomposer)
			{
				EditorGUIUtility.labelWidth = 210;
				useWorldComposerLocationData = EditorGUILayout.Toggle("Use WorldComposer Location Data", useWorldComposerLocationData );	
				EditorGUILayout.Space();
			}
		}
		#endregion
		
		#region Save/Load
		EditorGUILayout.LabelField("Save / Load", EditorStyles.largeLabel, GUILayout.MinHeight(18) );
		saveLoad = EditorGUILayout.Foldout( saveLoad, "" );
		if(saveLoad)
		{
			EditorGUIUtility.labelWidth = 180;
			EditorGUILayout.PropertyField(saveDownloadedMapDataProp);
			EditorGUILayout.PropertyField(mapFileNameSavePrefixProp, new GUIContent("Filename Save Prefix:") ); 
			EditorGUILayout.Space();
			EditorGUIUtility.labelWidth = 0;
		}
		#endregion
				
		#region Gizmos
		EditorGUILayout.LabelField("Gizmo Drawing", EditorStyles.largeLabel, GUILayout.MinHeight(18) );
		gizmos = EditorGUILayout.Foldout( gizmos, "" );
		if(gizmos)
		{
			EditorGUILayout.PropertyField(persistentGizmosProp);
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(gizmoDrawNodesProp, new GUIContent("Nodes"));
			if( gizmoDrawNodesProp.boolValue )
			{
				EditorGUILayout.HelpBox("If the number of map nodes is too large it will cause a Unity rendering error.", MessageType.Warning );
			}
			EditorGUILayout.PropertyField(gizmoDrawWaysProp, new GUIContent("Ways"));
			EditorGUILayout.PropertyField(gizmoDrawHighWaysProp, new GUIContent("Highways"));
			EditorGUILayout.PropertyField(gizmoDrawHighWaysLabelsProp, new GUIContent("Highway Labels"));
			EditorGUILayout.PropertyField(gizmoDrawWaterWaysProp, new GUIContent("Waterways"));
			EditorGUILayout.PropertyField(gizmoDrawBuildingsProp, new GUIContent("Building Plans"));
			EditorGUILayout.PropertyField(gizmoDrawRelationsProp, new GUIContent("Relations"));	
			EditorGUILayout.Space();
		}
		#endregion

		#region Debug
		EditorGUILayout.LabelField("Debug", EditorStyles.largeLabel, GUILayout.MinHeight(18) );
		debug = EditorGUILayout.Foldout( debug, "" );
		if(debug)
		{
            GUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(enableLoggingProp, new GUIContent("Enable Mapity Logs"));

            if (GUILayout.Button("Open Temp Download Location"))
            {
                string tempDirectory = Application.temporaryCachePath + "/Mapity";
                EditorUtility.RevealInFinder(tempDirectory);
            }
            GUILayout.EndHorizontal();

			EditorGUILayout.PropertyField(enableErrorsProp, new GUIContent("Enable Mapity Errors"));

            EditorGUILayout.Space();
		}
		#endregion
        
        #region Support
        EditorGUILayout.LabelField("Support:", EditorStyles.largeLabel, GUILayout.MinHeight(18));
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Documentation"))
		{
			Help.BrowseURL("http://www.rewindgamestudio.com/documentation/mapity_v200/index.html");
		}

        if (GUILayout.Button("Manual"))
        {
            Help.BrowseURL("http://www.rewindgamestudio.com/documentation/current/Map-ity.pdf");
        }

        if (GUILayout.Button("Contact"))
		{
			Help.BrowseURL("mailto:contact@rewindgamestudio.com");
		}
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        GUILayout.Label(mapityIcon,GUILayout.Width(80), GUILayout.Height(80));

        EditorGUILayout.LabelField("Rewind Game Studio (c) 2016.", EditorStyles.largeLabel, GUILayout.MinHeight(18));
		#endregion

		if (GUI.changed)
        {
			EditorUtility.SetDirty(mapityScript);
        }

		serializedObject.ApplyModifiedProperties();
	}
}

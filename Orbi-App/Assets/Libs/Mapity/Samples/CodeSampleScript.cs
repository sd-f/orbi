using UnityEngine;

public class CodeSampleScript : MonoBehaviour {

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		// Register Map-ity's Loaded Event
		Mapity.MapityLoaded += OnMapityLoaded;
	}
	
	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		// Un-Register Map-ity's Loaded Event
		Mapity.MapityLoaded -= OnMapityLoaded;
	}
	
	/// <summary>
	/// Raises the mapity loaded event.
	/// </summary>
	void OnMapityLoaded()
	{
        // Example #1
        // Loop over all the roads in Map-ity

        Mapity.MapWay mapWay = null;

        for(var mapWayEnumerator = Mapity.Singleton.mapWays.Values.GetEnumerator(); mapWayEnumerator.MoveNext();)
		{
            mapWay = mapWayEnumerator.Current as Mapity.MapWay;

            if ( mapWay.tags != null )
			{
				if( mapWay.tags.GetTag("name") != null )
				{
					Debug.Log( mapWay.tags.GetTag("name").ToString() );
				}
			}
		}

        // Example #2
        // Loop over all the buildings in Map-ity

        Mapity.MapBuilding mapBuilding = null;

        for(var mapBuildingEnumerator = Mapity.Singleton.mapBuildings.Values.GetEnumerator(); mapBuildingEnumerator.MoveNext();)
		{
            mapBuilding = mapBuildingEnumerator.Current as Mapity.MapBuilding;

            // Log building id
            Debug.Log( "Building: "+mapBuilding.id.ToString() );
			
			// Loop over the ways which define this building(usually 1)
			for (int i = 0; i < mapBuilding.buildingWays.Count; i++)
			{
				// Get the way
				Mapity.MapWay buildingWay = (Mapity.MapWay)mapBuilding.buildingWays[i];
				
				// NULL check
				if( buildingWay != null )
				{			
					if( buildingWay.tags != null )
					{
						if( buildingWay.tags.GetTag("name") != null )
						{
							Debug.Log( buildingWay.tags.GetTag("name").ToString() );
						}
					}

					// Loop over the nodes
					for (int j = 0; j < buildingWay.wayMapNodes.Count; j++)
					{
						// Get the node
						Mapity.MapNode node = (Mapity.MapNode)buildingWay.wayMapNodes[j];
						
						Debug.Log( node.position.world.ToString() );
					}
				}
			}
		}

	}
}

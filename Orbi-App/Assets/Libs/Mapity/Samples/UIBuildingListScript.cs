using UnityEngine;
using UnityEngine.UI;

public class UIBuildingListScript : MonoBehaviour {

    [SerializeField]
    Text m_BuildingInfo = null;

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
        string strUIText = "Building Info:\n";

        strUIText += "\nFound " + Mapity.Singleton.mapBuildings.Count.ToString() + " buildings.";

        int iBuildingCount = 0;

        Mapity.MapBuilding mapBuilding = null;

        // Loop over all the buildings in Map-ity
        for( var buildingEnumerator = Mapity.Singleton.mapBuildings.Values.GetEnumerator(); buildingEnumerator.MoveNext();)
        {
            mapBuilding = buildingEnumerator.Current as Mapity.MapBuilding;

            ++iBuildingCount;
			strUIText += "\n\n#" + iBuildingCount.ToString();

            // Loop over the ways which define this building(usually 1)
            for (int i = 0; i < mapBuilding.buildingWays.Count; i++)
            {
                // Get the way
                Mapity.MapWay buildingWay = (Mapity.MapWay)mapBuilding.buildingWays[i];

                // NULL check
                if (buildingWay != null)
                {
                    if (buildingWay.tags != null)
                    {
                        strUIText += "\nName: ";

                        if (buildingWay.tags.GetTag("name") != null)
                        {
                            strUIText += buildingWay.tags.GetTag("name").ToString() + ".";
                        }
                        else
                        {
                            strUIText += "Unknown.";
                        }

                        strUIText += "\nAddress: ";

                        if (buildingWay.tags.GetTag("addr:housenumber") != null)
                        {
                            strUIText += buildingWay.tags.GetTag("addr:housenumber").ToString() + ", ";
                        }

                        if (buildingWay.tags.GetTag("addr:street") != null)
                        {
                            strUIText += buildingWay.tags.GetTag("addr:street").ToString() + ", ";
                        }

                        if (buildingWay.tags.GetTag("addr:city") != null)
                        {
                            strUIText += buildingWay.tags.GetTag("addr:city").ToString() + ", ";
                        }

                        if (buildingWay.tags.GetTag("addr:postcode") != null)
                        {
                            strUIText += buildingWay.tags.GetTag("addr:postcode").ToString() + ", ";
                        }

                        if (buildingWay.tags.GetTag("addr:state") != null)
                        {
                            strUIText += buildingWay.tags.GetTag("addr:state").ToString() + ".";
                        }
                    }
                }
            }
        }

        m_BuildingInfo.text = strUIText;
    }

	public void Clear()
	{
		m_BuildingInfo.text = "Building Info:";
	}
}

using UnityEngine;
using UnityEngine.UI;

public class CategoryContainerScript : MonoBehaviour {

    private ServerModel.GameObjectTypeCategory category;
    private int itemsDiscovered = 0;

	void Start () {
        GameObject header = transform.Find("CategoryHeader").gameObject;
        header.transform.Find("CategoryTitle").GetComponent<Text>().text = category.name;
        header.transform.Find("CategoryStatus").gameObject.SetActive(itemsDiscovered == category.types.Count);
        header.transform.Find("CategoryStatusText").GetComponent<Text>().text = itemsDiscovered + " / " + category.types.Count;
        
    }

    public void SetItemsDiscovered(int items)
    {
        this.itemsDiscovered = items;
    }
	
	public void SetCategory(ServerModel.GameObjectTypeCategory category)
    {
        this.category = category;
    }
}

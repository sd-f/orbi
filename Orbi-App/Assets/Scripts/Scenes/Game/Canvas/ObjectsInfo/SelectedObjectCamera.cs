using UnityEngine;
using GameController;

public class SelectedObjectCamera : MonoBehaviour {

    public GameObject objectContainer;
    public Camera objectCamera;
    private GameObject realObject;
    private Vector3 offset = Vector3.zero;
    private float size;

	public void SetGameObject(GameObject container, GameObject real)
    {
        this.objectContainer = container;
        this.realObject = real;
        this.size = GameObjectUtility.GetMaxSize(real);
        objectCamera.orthographicSize = size * 2.5f;
        offset = /*(Vector3.back * objectCamera.orthographicSize * 2f)*/ (Vector3.back * 10f) + (Vector3.up / 7f * objectCamera.orthographicSize) + (Vector3.left / 2f * objectCamera.orthographicSize);
        GameObjectUtility.SetLayer(objectContainer, LayerMask.NameToLayer("SelectedObject"));
    }

    void OnEnable()
    {
        transform.localPosition = new Vector3(0, 0, size);
        //if (inverseLook)
            //transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    void OnDisable()
    {
        GameObjectUtility.SetLayer(objectContainer, LayerMask.NameToLayer("Objects"));
    }
	
	// Update is called once per frame
	void Update () {
        if (realObject != null)
        {
            transform.position = realObject.transform.position + offset;
            //transform.localPosition = new Vector3(1,1, size);
            realObject.transform.LookAt(realObject.transform);
        }
        
        //transform.position = realObject.transform.position + (Vector3.left * 0.1f);
    }
}

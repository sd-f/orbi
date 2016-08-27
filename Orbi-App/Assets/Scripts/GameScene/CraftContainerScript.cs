using UnityEngine;
using System.Collections;
using Assets.Control;
using Assets.Control.util;
using Assets.Control.services;
using Assets.Model;

public class CraftContainerScript : MonoBehaviour {

    private UnityEngine.GameObject container;
    private MenuScript menu;
    private Vector3 firstpoint;
    private Vector3 secondpoint;
    private float xAngle = 0.0f;
    private float xAngTemp = 0.0f;
    private float yDistance = 0.0f;
    private Vector3 velocityDown = Vector3.zero;
    private Vector3 velocityBack = Vector3.zero;
    private string prefab;

    private Vector3 tmpLocalPosition;
    private Vector3 tmpPosition;

    private float KEY_ROTATION_SPEED = 7.0F;

    UnityEngine.GameObject newObject;
    UnityEngine.GameObject effectGameObject;
    UnityEngine.GameObject selectorEffectGameObject;
    public LayerMask layers;

    void Awake () {
        menu = UnityEngine.GameObject.Find("Menu").GetComponent<MenuScript>();
        container = this.gameObject;
	}
	
    public void StartDestroying() { 
        ClearContainer();
        Game.GetInstance().player.selectedObjectId = 0;
        selectorEffectGameObject = UnityEngine.GameObject.Instantiate(Resources.Load<UnityEngine.GameObject>("Prefabs/SelectLaserEffect"));
        
        selectorEffectGameObject.name = "SelectLaser";
        selectorEffectGameObject.transform.parent = container.transform;
        selectorEffectGameObject.transform.localPosition = new Vector3(0, -1, 0);
        selectorEffectGameObject.transform.localRotation = Quaternion.Euler(0,0,0);


    }

    public void DoDestroy()
    {
        if (Game.GetInstance().player.selectedObjectId == 0)
        {
            Error.Show("Nothing selected");
        } else
        {
            StartCoroutine(Game.GetInstance().GetGameObjectsService().RequestDestroy(this));
        }
    }

    public void CancelDestroying()
    {
        Game.GetInstance().player.selectedObjectId = 0;
        ClearContainer();
    }

    public void StartCrafting()
    {
        // delete all children
        ClearContainer();
        prefab = Game.GetInstance().GetCraftPrefab();
        effectGameObject = UnityEngine.GameObject.Find("CraftingEffect");
        //container.transform.localPosition = new Vector3(0, 0, 6);
        effectGameObject = UnityEngine.GameObject.Instantiate(Resources.Load<UnityEngine.GameObject>("Prefabs/CraftingEffect"));
        effectGameObject.transform.parent = container.transform;
        
        newObject = GameObjectTypes.CreateObject(container.transform, Game.GetInstance().GetCraftPrefab(), -1, gameObject.name, false, "ObjectToCraft");
        GameObjectTypes.SetLayer(newObject, LayerMask.NameToLayer("Default"));
        newObject.transform.localPosition = new Vector3(0, 0, 7);
    }

    public void ClearContainer()
    {
        foreach (Transform child in container.transform)
        {
            //if (child.gameObject.tag.Equals("ObjectToCraft"))
              Destroy(child.gameObject);
        }
    }

    public void DoCrafting()
    {
        Assets.Model.GameObject gameObject = new Assets.Model.GameObject();
        gameObject.name = "NEW_" + System.DateTime.Now.ToString().Replace(' ', '_');
        gameObject.prefab = prefab;
        gameObject.rotation = new Rotation();
        gameObject.rotation.y = newObject.transform.eulerAngles.y;
        gameObject.geoPosition = new GeoPosition(newObject.transform.position.z, newObject.transform.position.x, newObject.transform.position.y);
        Game.GetInstance().GetAdapter().ToReal(gameObject.geoPosition, Game.GetInstance().player);
        Game.GetInstance().player.gameObjectToCraft = gameObject;
        ClearContainer();
        StartCoroutine(Game.GetInstance().GetPlayerService().RequestCraft(this));
        
    }

    public void CancelCrafting()
    {
        ClearContainer();
    }

    void Update()
    {
        if (menu.IsCrafting())
        {
            tmpPosition = newObject.transform.position;
            newObject.transform.position = Vector3.SmoothDamp(tmpPosition, new Vector3(tmpPosition.x, GetMinHeightForObject(newObject), tmpPosition.z), ref velocityDown, 0.1f);
            //newObject.transform.rotation = Quaternion.Euler(0, newObject.transform.rotation.y, 0);
            container.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
            // container.transform.localPosition = new Vector3(0, 0, 7);
            //newObject.transform.position = new Vector3(newObject.transform.position.x, GetMinHeightForObject(newObject), newObject.transform.position.z);
            //Info.Show(tmpPosition.x + "," + tmpPosition.z + " - " + GetHeight(tmpPosition.x, tmpPosition.z));

            effectGameObject.transform.position = newObject.transform.position;
            if (SystemInfo.deviceType == DeviceType.Desktop)
                keyboardObjectMovement(newObject);
            else
                touchObjectMovement(newObject);
        }
    }

    void keyboardObjectMovement(UnityEngine.GameObject objectToMove)
    {
        var d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0f)
            objectToMove.transform.localPosition = new Vector3(0, tmpLocalPosition.y, tmpLocalPosition.z + (KEY_ROTATION_SPEED * Time.deltaTime));
        else if (d < 0f)
            objectToMove.transform.localPosition = new Vector3(0, tmpLocalPosition.y, tmpLocalPosition.z + (KEY_ROTATION_SPEED * Time.deltaTime));

        tmpLocalPosition = objectToMove.transform.localPosition;
        if (Input.GetKey(KeyCode.KeypadPlus))
            objectToMove.transform.localPosition = new Vector3(0, tmpLocalPosition.y,  tmpLocalPosition.z + (KEY_ROTATION_SPEED * Time.deltaTime));
        if (Input.GetKey(KeyCode.KeypadMinus))
            objectToMove.transform.localPosition = new Vector3(0, tmpLocalPosition.y,  tmpLocalPosition.z - (KEY_ROTATION_SPEED * Time.deltaTime));
        if (Input.GetKey(KeyCode.KeypadDivide))
            objectToMove.transform.Rotate(new Vector3(0, -(KEY_ROTATION_SPEED*10) * Time.deltaTime, 0));
        if (Input.GetKey(KeyCode.KeypadMultiply))
            objectToMove.transform.Rotate(new Vector3(0, (KEY_ROTATION_SPEED*10) * Time.deltaTime, 0));
    }

    void touchObjectMovement(UnityEngine.GameObject objectToMove)
    {
        
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                firstpoint = Input.GetTouch(0).position;
                xAngTemp = xAngle;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                secondpoint = Input.GetTouch(0).position;
                xAngle = xAngTemp + (secondpoint.x - firstpoint.x) * 180.0f / Screen.width;
                yDistance = ((secondpoint.y - firstpoint.y) / (Screen.height/5)) + tmpLocalPosition.z;
                //Error.Show("distance = "+ yDistance);
                //Rotate camera
                tmpLocalPosition = objectToMove.transform.localPosition;
                if (yDistance < 2f)
                    yDistance = 2f;
                if (yDistance > 30f)
                    yDistance = 30f;
                objectToMove.transform.localPosition = Vector3.SmoothDamp(tmpLocalPosition, new Vector3(0, tmpLocalPosition.y, yDistance), ref velocityBack, 0.1f);

                objectToMove.transform.rotation = Quaternion.Euler(new Vector3(0.0f, xAngle, 0.0f));
               // this.
            }
        }
    }

    public float GetMinHeightForObject(UnityEngine.GameObject objectToMove)
    {
        float newHeight = 0.0f;
        float height = 0.0f; // GetHeight(objectToMove.transform.position.x, objectToMove.transform.position.y);
        //Debug.Log(objectToMove.transform.position);
        UnityEngine.GameObject realObject;
        float x = 0.0f;
        float z = 0.0f;
        foreach (Transform child in objectToMove.transform)
        {
            realObject = child.gameObject;
            x = realObject.transform.position.x;
            z = realObject.transform.position.z;
            Vector3 box = objectToMove.GetComponentInChildren<Collider>().bounds.size;
            box = box / 2;
            //Debug.Log(box);

            // checking bounds (no loop)
            height = GetHeight(x, z);
            newHeight = GetHeight(x - box.x, z - box.z);
            if (newHeight > height)
                height = newHeight;
            newHeight = GetHeight(x + box.x, z - box.z);
            if (newHeight > height)
                height = newHeight;
            newHeight = GetHeight(x - box.x, z + box.z);
            if (newHeight > height)
                height = newHeight;
            newHeight = GetHeight(x + box.x, z + box.z);
            if (newHeight > height)
                height = newHeight;
            break; // should be only one object
        }
        
        return height + 0.0000001f;
    }

    public float GetHeight(float x, float z)
    {
        
        Vector3 rayOrigin = new Vector3(x, 100, z);
        Ray ray = new Ray(rayOrigin, Vector3.down);
        //Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        float distanceToCheck = 110f;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distanceToCheck, layers))
            return hit.point.y;
        //Debug.Log(hit.point);
        return 0.0f;
    }

}

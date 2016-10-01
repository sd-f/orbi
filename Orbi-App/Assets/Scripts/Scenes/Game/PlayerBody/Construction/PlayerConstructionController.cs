using UnityEngine;
using GameController;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/PlayerConstructionController")]
    class PlayerConstructionController : MonoBehaviour
    {
        public GameObject camera;
        private bool isDesktopMode = false;
        private bool crafting = false;
        private GameObject newObject;
        private Rigidbody body;
        private Vector3 rotation = new Vector3(0, 0, 0);
        private float distance = 10f;

        void Start()
        {
            Game.GetPlayer().GetCraftingController().SetCrafting(false, null);
        }

        void Awake ()
        {
            isDesktopMode = Game.GetGame().GetSettings().IsDesktopInputEnabled();
        }

        void Update()
        {
            if (crafting)
            {
                if (isDesktopMode)
                {
                    if (Input.GetKeyDown(KeyCode.Q))
                        rotation.y = rotation.y - 10f;
                    if (Input.GetKeyDown(KeyCode.E))
                        rotation.y = rotation.y + 10f;
                    // object distance
                    float d = Input.GetAxis("Mouse ScrollWheel");
                    if ((d > 0f) && (distance >= 5f))
                        distance -= 0.25f;
                    else if ((d < 0f) && (distance <= 100f))
                        distance += 0.25f;
                }
                newObject.transform.position = Vector3.Lerp(newObject.transform.position, 
                    CheckFloor(transform.position), Time.deltaTime * 20f);
                //newObject.transform.localPosition = Vector3.Lerp(newObject.transform.position, CheckFloor(transform.position), Time.deltaTime * 20f);
                //body.MovePosition(transform.position);
                //body.MoveRotation(Quaternion.Slerp(newObject.transform.rotation, Quaternion.Euler(rotation), Time.deltaTime));
                newObject.transform.rotation = Quaternion.Slerp(newObject.transform.rotation, Quaternion.Euler(rotation) , Time.deltaTime * 20f);
                transform.localPosition = new Vector3(0, 0, distance);
            }
            
            
        }

        public Vector3 CheckFloor(Vector3 newPosition)
        {
            float height = Game.GetWorld().GetMinHeightForObject(newObject);
            return new Vector3(newPosition.x, height, newPosition.z);
            //return newPosition;
        }

        public void StartCrafting()
        {
            CreateObjectToCraft();
            //this.body = GameObjectUtility.GetRigidBody(newObject);
            transform.localPosition = new Vector3(0,0,distance);
            newObject.transform.position = CheckFloor(transform.position);
            //this.body.velocity = new Vector3(1,1,1);
            //this.body.constraints = RigidbodyConstraints.FreezeRotation;
            Game.GetPlayer().GetCraftingController().SetCrafting(true, newObject);
            this.crafting = true;
        }

        public void StopCrafting()
        {
            this.crafting = false;
            Game.GetPlayer().GetCraftingController().SetCrafting(false, null);
            CleanUp();
        }

        public void Craft()
        {
            Debug.Log("Not implemented, TODO");
        }


        private void CreateObjectToCraft()
        {
            newObject = GameObjectFactory.CreateObject(transform, Game.GetPlayer().GetCraftingController().GetSelectedPrefab(), -1, "new_", null, LayerMask.NameToLayer("Default"));
            newObject.transform.rotation = Quaternion.Euler(rotation);
            GameObjectUtility.Freeze(newObject);
            Game.GetPlayer().GetCraftingController().SetCrafting(true, newObject);
        }

        private void CleanUp()
        {
            GameObjectUtility.DestroyAllChildObjects(this.gameObject);
        }

    }
}
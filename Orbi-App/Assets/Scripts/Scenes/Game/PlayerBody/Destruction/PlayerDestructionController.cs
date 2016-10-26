using UnityEngine;
using System.Collections;
using GameController;
using System;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Body/PlayerDestructionController")]
    class PlayerDestructionController : MonoBehaviour
    {
#pragma warning disable 0649
        public GameObject Shot1;
        public GameObject Wave;
        public GameObject explosionEffect;
        public GameObject earnedXPTextPrefab;
        public Camera cam;
        private bool isDesktopMode = false;
        

        void Awake ()
        {
            isDesktopMode = Game.Instance.GetSettings().IsDesktopInputEnabled();
        }

        void Update()
        {
            //create BasicBeamShot

            if (Input.GetButtonDown("Fire2") && isDesktopMode)
                Shoot();

        }

        public void Shoot()
        {
            GameObject s1 = (GameObject)Instantiate(Shot1, this.transform.position, this.transform.rotation);
            s1.GetComponent<BeamParam>().SetBeamParam(this.GetComponent<BeamParam>());
            GameObject wav = (GameObject)Instantiate(Wave, s1.transform.position, s1.transform.rotation);
            wav.transform.localScale *= 0.25f;
            wav.transform.Rotate(Vector3.left, 90.0f);
            wav.GetComponent<BeamWave>().col = this.GetComponent<BeamParam>().BeamColor;
        }

        public void Destroy(GameObject objectToDestroy)
        {
            GameObject realContainer = FindObject(objectToDestroy);
            if (realContainer != null)
            {
                // start effect
                long id = GameObjectUtility.GetId(realContainer);
                if (id != 0)
                {
                    GameObject effect = GameObject.Instantiate(explosionEffect) as GameObject;
                    GameObject earnedText = GameObject.Instantiate(earnedXPTextPrefab) as GameObject;
                    earnedText.GetComponent<XPEarnedText>().SetAmount(ServerModel.CharacterDevelopment.XP_DESTROY);
                    Rigidbody body = GameObjectUtility.GetRigidBody(realContainer);
                    effect.transform.position = body.transform.position; // realContainer.transform.position;
                    effect.transform.localScale = effect.transform.localScale * GameObjectUtility.GetSize(realContainer);
                    earnedText.transform.position = body.transform.position + (Vector3.up * 2f);
                    earnedText.transform.rotation = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0);
                    
                    Game.Instance.GetWorld().GetGameObjectService().RemoveObject(realContainer);
                    GameObject.Destroy(effect, 3f);
                    GameObject.Destroy(realContainer);
                    StartCoroutine(Game.Instance.GetPlayer().GetDestructionController().Destroy(id));
                }
                    
            }
        }

        private GameObject FindObject(GameObject gameObject)
        {
            if (gameObject.tag == "DynamicGameObject")
            {
                return gameObject;
            }
            else
            {
                if (gameObject.transform.parent != null)
                {
                    return FindObject(gameObject.transform.parent.gameObject);
                }
            }
            return null;
        }
    }

    
}
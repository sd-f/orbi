using UnityEngine;
using System.Collections;
using GameController;
using System;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/DestructionController")]
    class DestructionController : GameMonoBehaviour
    {
#pragma warning disable 0649
        public GameObject removeEffect;
        public GameObject earnedXPTextPrefab;
        public Camera cam;

        public void RemoveObject(GameObject obj)
        {
            GameObject objectToDestroy = obj;
            GameObject effect = GameObject.Instantiate(removeEffect) as GameObject;
            GameObject earnedText = GameObject.Instantiate(earnedXPTextPrefab) as GameObject;
            earnedText.GetComponent<XPEarnedText>().SetAmount(ServerModel.CharacterDevelopment.XP_DESTROY);
            Rigidbody body = GameObjectUtility.GetRigidBody(objectToDestroy);
            effect.transform.position = body.transform.position; // realContainer.transform.position;
            effect.transform.localScale = effect.transform.localScale * GameObjectUtility.GetMaxSize(objectToDestroy);
            earnedText.transform.position = body.transform.position + (Vector3.up * 2f);
            earnedText.transform.rotation = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0);

            Game.Instance.GetWorld().GetGameObjectService().RemoveObject(objectToDestroy);
            GameObject.Destroy(effect, 1.1f);
            GameObject.Destroy(objectToDestroy,1f);
        }

    }

    
}
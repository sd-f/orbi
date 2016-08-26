using UnityEngine;
using System.Collections;
using Assets.Control;
using System;

public class BeamHitsObjectsScript : MonoBehaviour {


    void OnParticleCollision(GameObject collisionObject)
    {
        if (gameObject.transform.parent.gameObject.name.Contains("object_"))
        {
            Game.GetInstance().player.selectedObjectId = Convert.ToInt64(gameObject.transform.parent.gameObject.name.Replace("object_", ""));
        }
        
    }
}

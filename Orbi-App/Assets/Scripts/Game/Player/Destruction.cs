using GameScene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameController
{
    class Destruction
    {

        public void Shoot(PlayerDestructionController controller)
        {
            GameObject s1 = GameObject.Instantiate(GameObjectFactory.GetPrefab("Prefabs/BasicBeamShot/GeroBeam"), 
                controller.transform.position, controller.transform.rotation) as GameObject;
            s1.GetComponent<BeamParam>().SetBeamParam(controller.GetComponent<BeamParam>());

            GameObject wav = GameObject.Instantiate(GameObjectFactory.GetPrefab("Prefabs/BasicBeamShot/Parts/BeamWave"),
                controller.transform.position, controller.transform.rotation) as GameObject;
            wav.transform.localScale *= 0.25f;
            wav.transform.Rotate(Vector3.left, 90.0f);
            wav.GetComponent<BeamWave>().col = controller.GetComponent<BeamParam>().BeamColor;
        }

    }
}

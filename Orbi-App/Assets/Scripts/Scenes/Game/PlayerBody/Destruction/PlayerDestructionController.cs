using UnityEngine;
using System.Collections;
using GameController;
using System;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/PlayerDestructionController")]
    class PlayerDestructionController : MonoBehaviour
    {
        public GameObject Shot1;
        public GameObject Wave;
        private bool isDesktopMode = false;
        

        void Awake ()
        {
            isDesktopMode = Game.GetGame().GetSettings().IsDesktopInputEnabled();
        }

        void Update()
        {
            //create BasicBeamShot

            if (Input.GetButtonDown("Fire2") && isDesktopMode)
            {
                Shoot();
            }

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

    }
}
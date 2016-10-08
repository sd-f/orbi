using UnityEngine;
using System.Collections;
using GameController;
using System;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/Body/PlayerInteractionController")]
    class PlayerInteractionController : MonoBehaviour
    {

        private bool isDesktopMode = false;
        private Vector2 selectionPoint = new Vector2();
        private Vector2 centerOfScreen = new Vector2();

        void Awake()
        {
            isDesktopMode = Game.GetGame().GetSettings().IsDesktopInputEnabled();
            centerOfScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);
            selectionPoint = centerOfScreen; 
        }

        void Update()
        {
            if (isDesktopMode)
            {
                if (Input.GetKeyDown(KeyCode.F))
                    checkTouchObjectSingleTouch();
            }
            else
            {
                if (Input.touchCount > 0)
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        selectionPoint = Input.GetTouch(0).position;
                        checkTouchObjectSingleTouch();
                    }
            }

        }

        private void checkTouchObjectSingleTouch()
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(selectionPoint);
            if (Physics.Raycast(ray, out hit))
            {
                CharacterProperties characterProperties = hit.transform.gameObject.GetComponent<CharacterProperties>();
                if (characterProperties != null)
                    characterProperties.OnTouched();
            }
        }

    }

    
}
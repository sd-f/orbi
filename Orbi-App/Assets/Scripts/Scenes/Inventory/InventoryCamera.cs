﻿using UnityEngine;
using System.Collections;
using GameController;
using System.Collections.Generic;

namespace InventoryScene
{
    [AddComponentMenu("App/Scenes/Inventory/InventoryCamera")]
    public class InventoryCamera : MonoBehaviour
    {
        private static float OFFSET_LEFT = -946f;
        private static float OFFSET_TOP = -2f;
        private float boundRight = 20f;
        private float boundBottom = 20f;

        private bool isDesktopMode = false;
        private float moveSpeedY = 1.5f;
        private float moveSpeedX = 5f;
        private Vector3 firstpoint;
        private Vector3 secondpoint;
        private SortedList<int, ServerModel.InventoryItem> objectsList = new SortedList<int, ServerModel.InventoryItem>();

        private Vector2 target = new Vector2(0,0);
        private Vector3 realTarget = new Vector3(0, 0, 0);
        // Use this for initialization
        void Start()
        {
            isDesktopMode = Game.GetGame().GetSettings().IsDesktopInputEnabled();
            SetTarget(0, 0);
        }


        public void SetBounds(float right, float bottom)
        {
            
            this.boundBottom = bottom;
            this.boundRight = right;
        }

        // Update is called once per frame
        void Update()
        {
            if (isDesktopMode)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    Left(1);
                if (Input.GetKeyDown(KeyCode.RightArrow))
                    Right(1);
                if (Input.GetKeyDown(KeyCode.UpArrow))
                    Up(1);
                if (Input.GetKeyDown(KeyCode.DownArrow))
                    Down(1);
                float d = Input.GetAxis("Mouse ScrollWheel");
                if (d > 0f)
                    Up(1);
                else if (d < 0f)
                    Down(1);
                if (Input.GetButtonDown("Fire1"))
                {
                    checkTouchObjectSingleTouch(Input.mousePosition);
                }
            } else
            {
                if (Input.touchCount > 0)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        firstpoint = Input.GetTouch(0).position;
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {

                        secondpoint = Input.GetTouch(0).position;
                        float movedX = (secondpoint.x - firstpoint.x) / Screen.width * 20;
                        float movedY = (secondpoint.y - firstpoint.y) / Screen.height * 20;
                        /*
                        Text text = UnityEngine.GameObject.Find("DebugText").GetComponent<Text>();
                        text.text = "secondpoint.x: " + secondpoint.x
                            + "\nmoved: " + moved; */
                        if (movedX >= 5)
                            Left(1.5f);
                        if (movedX <= -5)
                            Right(1.5f);
                        if (movedY >= 5)
                            Down(2);
                        if (movedY <= -5)
                            Up(2);
                        if ((movedX > -5) && (movedX < 5) && (movedY > -5) && (movedY < 5))
                        {
                            checkTouchObjectSingleTouch(secondpoint);
                        }
                    }
                    
                }
            }
            realTarget = new Vector3(OFFSET_LEFT + target.x, OFFSET_TOP - target.y);
            transform.position = Vector3.Lerp(transform.position, realTarget, 0.1f);
        }

        private void checkTouchObjectSingleTouch(Vector2 position)
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out hit))
            {
                UnityEngine.GameObject container = GameObjectUtility.GetObjectContainer(hit.transform.gameObject);
                container.SendMessage("OnTouched", this);
            }
        }
        public SortedList<int, ServerModel.InventoryItem> GetObjectsList()
        {
            return this.objectsList;
        }


        private void Up(float amount)
        {
            SetTarget(target.x, target.y - moveSpeedY * amount);
        }

        private void Down(float amount)
        {
            SetTarget(target.x, target.y + moveSpeedY * amount);
        }

        private void Left(float amount)
        {
            SetTarget(target.x - moveSpeedX * amount, target.y);
        }

        private void Right(float amount)
        {
            SetTarget(target.x + moveSpeedX * amount, target.y);
        }

        public void SetTarget(float x, float y)
        {
            float targetX = Mathf.Clamp(x, 0, boundRight);
            float targetY = Mathf.Clamp(y, 0, boundBottom);
            this.target = new Vector2(targetX, targetY);
        }
    }
}
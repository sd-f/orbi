﻿using UnityEngine;
using System.Collections;
using UMA;
using GameController;
using UnityStandardAssets.Characters.ThirdPerson;

namespace GameScene
{
    [AddComponentMenu("App/Scenes/Game/UMACreator")]
    public class UMACreator : MonoBehaviour
    {

        public GameObject character;
        public UMAGeneratorBase generator;
        public SlotLibrary slotLibrary;
        public OverlayLibrary overlayLibrary;
        public RaceLibrary raceLibrary;
        public RuntimeAnimatorController animController;
        

        public GameObject GenerateUMA(GameObject parent, string containerName)
        {

            GameObject container = GameObject.Instantiate(character) as GameObject;
            container.name = containerName;
            container.transform.SetParent(parent.transform);
            container.transform.localPosition = Vector3.zero;
            container.transform.localRotation = Quaternion.identity;
            
            GameObject umaObject = new GameObject(containerName + "_dynamic_avatar");
            umaObject.transform.SetParent(container.transform);
            umaObject.transform.localPosition = Vector3.zero;
            umaObject.transform.localRotation = Quaternion.identity;

            Uma uma = umaObject.AddComponent<Uma>();
            uma.umaDynamicAvatar = umaObject.AddComponent<UMADynamicAvatar>();
            //uma.umaDynamicAvatar.loadOnStart = true; 
            

            // setup data
            uma.umaDynamicAvatar.Initialize();
            
            uma.umaData = uma.umaDynamicAvatar.umaData;
            uma.umaData.OnCharacterCreated += Created;

            // Attach our generator
            uma.umaDynamicAvatar.umaGenerator = generator;
            uma.umaData.umaGenerator = generator;


            // slot array
            uma.umaData.umaRecipe.slotDataList = new SlotData[uma.numberOfSlots];

            // morph
            uma.umaDna = new UMADnaHumanoid();
            uma.umaTutorialDna = new UMADnaTutorial();
            uma.umaData.umaRecipe.AddDna(uma.umaDna);
            uma.umaData.umaRecipe.AddDna(uma.umaTutorialDna);

            CreateMale(uma);

            uma.umaDynamicAvatar.animationController = animController;
            //container.transform.position = Vector3.zero;
            //container.transform.rotation = Quaternion.identity;

            // generate uma
            uma.umaDynamicAvatar.UpdateNewRace();
            
            return container;
        }

        void Created(UMAData data)
        {
            GameObjectUtility.SetLayer(data.gameObject, LayerMask.NameToLayer("Objects"));
            ThirdPersonCharacter character = data.gameObject.transform.parent.gameObject.GetComponent<ThirdPersonCharacter>();
            Animator animator = data.gameObject.GetComponent<Animator>();
            character.SetAnimator(animator);
        }

        void CreateMale(Uma uma)
        {
            // grab ref to recipe
            UMAData.UMARecipe umaRecipe = uma.umaDynamicAvatar.umaData.umaRecipe;
            umaRecipe.SetRace(raceLibrary.GetRace("HumanMale"));


            uma.umaData.umaRecipe.slotDataList[0] = slotLibrary.InstantiateSlot("MaleFace");
            uma.umaData.umaRecipe.slotDataList[0].AddOverlay(overlayLibrary.InstantiateOverlay("MaleHead02"));
            uma.umaData.umaRecipe.slotDataList[0].AddOverlay(overlayLibrary.InstantiateOverlay("MaleHair02", Color.black));
            uma.umaData.umaRecipe.slotDataList[0].AddOverlay(overlayLibrary.InstantiateOverlay("MaleEyebrow01", Color.black));

            uma.umaData.umaRecipe.slotDataList[1] = slotLibrary.InstantiateSlot("MaleEyes");
            uma.umaData.umaRecipe.slotDataList[1].AddOverlay(overlayLibrary.InstantiateOverlay("EyeOverlay"));

            uma.umaData.umaRecipe.slotDataList[2] = slotLibrary.InstantiateSlot("MaleInnerMouth");
            uma.umaData.umaRecipe.slotDataList[2].AddOverlay(overlayLibrary.InstantiateOverlay("InnerMouth"));



            uma.umaData.umaRecipe.slotDataList[3] = slotLibrary.InstantiateSlot("MaleTorso");
            //umaData.umaRecipe.slotDataList[3].SetOverlayList(umaData.umaRecipe.slotDataList[4].GetOverlayList());
            uma.umaData.umaRecipe.slotDataList[3].AddOverlay(overlayLibrary.InstantiateOverlay("MaleBody02"));
            uma.umaData.umaRecipe.slotDataList[3].AddOverlay(overlayLibrary.InstantiateOverlay("MaleUnderwear01"));
            uma.umaData.umaRecipe.slotDataList[3].AddOverlay(overlayLibrary.InstantiateOverlay("MaleShirt01"));
            //umaData.umaRecipe.slotDataList[3].AddOverlay(overlayLibrary.InstantiateOverlay("MaleJeans01", Color.blue));

            uma.umaData.umaRecipe.slotDataList[4] = slotLibrary.InstantiateSlot("MaleHands");
            uma.umaData.umaRecipe.slotDataList[4].SetOverlayList(uma.umaData.umaRecipe.slotDataList[3].GetOverlayList());

            //AddOverlay(overlayLibrary.InstantiateOverlay("MaleBody01"));

            uma.umaData.umaRecipe.slotDataList[5] = slotLibrary.InstantiateSlot("MaleJeans01");
            //umaData.umaRecipe.slotDataList[5].AddOverlay(overlayLibrary.InstantiateOverlay("MaleBody01"));
            //umaData.umaRecipe.slotDataList[5].AddOverlay(overlayLibrary.InstantiateOverlay("MaleUnderwear01"));
            //umaData.umaRecipe.slotDataList[5].AddOverlay(overlayLibrary.InstantiateOverlay("MaleUnderwear01"));
            uma.umaData.umaRecipe.slotDataList[5].AddOverlay(overlayLibrary.InstantiateOverlay("MaleJeans01", Color.blue));

            uma.umaData.umaRecipe.slotDataList[6] = slotLibrary.InstantiateSlot("MaleFeet");
            uma.umaData.umaRecipe.slotDataList[6].SetOverlayList(uma.umaData.umaRecipe.slotDataList[3].GetOverlayList());

            //umaDna.chinPosition = 0.5f;
            //umaTutorialDna.eyeSpacing = 0.5f;
        }


    }
}

using UnityEngine;
using System.Collections;
using UMA;

public class UMACreator : MonoBehaviour {

    public UMAGeneratorBase generator;
    public SlotLibrary slotLibrary;
    public OverlayLibrary overlayLibrary;
    public RaceLibrary raceLibrary;
    public RuntimeAnimatorController animController;

    private UMADynamicAvatar umaDynamicAvatar;
    private UMAData umaData;
    private UMADnaHumanoid umaDna;
    private UMADnaTutorial umaTutorialDna;

    private int numberOfSlots = 7;

    void Start()
    {
        GenerateUMA(this.gameObject, "uma");
    }

    void GenerateUMA(GameObject parent, string containerName)
    {
        GameObject GO = new GameObject(containerName);
        GO.transform.SetParent(parent.transform);
        umaDynamicAvatar = GO.AddComponent<UMADynamicAvatar>();
        

        // setup data
        umaDynamicAvatar.Initialize();
        umaData = umaDynamicAvatar.umaData;

        // Attach our generator
        umaDynamicAvatar.umaGenerator = generator;
        umaData.umaGenerator = generator;


        // slot array
        umaData.umaRecipe.slotDataList = new SlotData[numberOfSlots];

        // morph
        umaDna = new UMADnaHumanoid();
        umaTutorialDna = new UMADnaTutorial();
        umaData.umaRecipe.AddDna(umaDna);
        umaData.umaRecipe.AddDna(umaTutorialDna);

        CreateMale();

        umaDynamicAvatar.animationController = animController;
        GO.transform.position = Vector3.zero;
        GO.transform.rotation = Quaternion.identity;

        // generate uma
        umaDynamicAvatar.UpdateNewRace();
    }

    void CreateMale()
    {
        // grab ref to recipe
        UMAData.UMARecipe umaRecipe = umaDynamicAvatar.umaData.umaRecipe;
        umaRecipe.SetRace(raceLibrary.GetRace("HumanMale"));


        umaData.umaRecipe.slotDataList[0] = slotLibrary.InstantiateSlot("MaleFace");
        umaData.umaRecipe.slotDataList[0].AddOverlay(overlayLibrary.InstantiateOverlay("MaleHead02"));
        umaData.umaRecipe.slotDataList[0].AddOverlay(overlayLibrary.InstantiateOverlay("MaleHair02", Color.black));
        umaData.umaRecipe.slotDataList[0].AddOverlay(overlayLibrary.InstantiateOverlay("MaleEyebrow01", Color.black)); 

        umaData.umaRecipe.slotDataList[1] = slotLibrary.InstantiateSlot("MaleEyes");
        umaData.umaRecipe.slotDataList[1].AddOverlay(overlayLibrary.InstantiateOverlay("EyeOverlay"));

        umaData.umaRecipe.slotDataList[2] = slotLibrary.InstantiateSlot("MaleInnerMouth");
        umaData.umaRecipe.slotDataList[2].AddOverlay(overlayLibrary.InstantiateOverlay("InnerMouth"));

        

        umaData.umaRecipe.slotDataList[3] = slotLibrary.InstantiateSlot("MaleTorso");
        //umaData.umaRecipe.slotDataList[3].SetOverlayList(umaData.umaRecipe.slotDataList[4].GetOverlayList());
        umaData.umaRecipe.slotDataList[3].AddOverlay(overlayLibrary.InstantiateOverlay("MaleBody02"));
        umaData.umaRecipe.slotDataList[3].AddOverlay(overlayLibrary.InstantiateOverlay("MaleUnderwear01"));
        umaData.umaRecipe.slotDataList[3].AddOverlay(overlayLibrary.InstantiateOverlay("MaleShirt01"));
        //umaData.umaRecipe.slotDataList[3].AddOverlay(overlayLibrary.InstantiateOverlay("MaleJeans01", Color.blue));

        umaData.umaRecipe.slotDataList[4] = slotLibrary.InstantiateSlot("MaleHands");
        umaData.umaRecipe.slotDataList[4].SetOverlayList(umaData.umaRecipe.slotDataList[3].GetOverlayList());

        //AddOverlay(overlayLibrary.InstantiateOverlay("MaleBody01"));

        umaData.umaRecipe.slotDataList[5] = slotLibrary.InstantiateSlot("MaleJeans01");
        //umaData.umaRecipe.slotDataList[5].AddOverlay(overlayLibrary.InstantiateOverlay("MaleBody01"));
        //umaData.umaRecipe.slotDataList[5].AddOverlay(overlayLibrary.InstantiateOverlay("MaleUnderwear01"));
        //umaData.umaRecipe.slotDataList[5].AddOverlay(overlayLibrary.InstantiateOverlay("MaleUnderwear01"));
        umaData.umaRecipe.slotDataList[5].AddOverlay(overlayLibrary.InstantiateOverlay("MaleJeans01", Color.blue));

        umaData.umaRecipe.slotDataList[6] = slotLibrary.InstantiateSlot("MaleFeet");
        umaData.umaRecipe.slotDataList[6].SetOverlayList(umaData.umaRecipe.slotDataList[3].GetOverlayList());

        
        

        //umaDna.chinPosition = 0.5f;
        //umaTutorialDna.eyeSpacing = 0.5f;



    }



}

using UnityEngine;
using System.Collections.Generic;
using UMA;
using System;

/// <summary>
/// Base class for UMA slot libraries.
/// </summary>
public abstract class SlotLibraryBase : MonoBehaviour 
{
    // UWP fixed switched to generic Exception
	public virtual void AddSlotAsset(SlotDataAsset slot) { throw new Exception(); }
	public virtual SlotDataAsset[] GetAllSlotAssets() { throw new Exception(); }
	public abstract SlotData InstantiateSlot(string name);
	public abstract SlotData InstantiateSlot(int nameHash);
	public abstract SlotData InstantiateSlot(string name, List<OverlayData> overlayList);
	public abstract SlotData InstantiateSlot(int nameHash, List<OverlayData> overlayList);

	public abstract void UpdateDictionary();
    public abstract void ValidateDictionary();

	[Obsolete("SlotLibrary.AddSlot(SlotData slot) is obsolete use SlotLibrary.AddSlotAsset(SlotDataAsset slot) instead", false)]
	public virtual void AddSlot(SlotData slot) { throw new Exception(); }
	[Obsolete("SlotLibrary.GetAllSlots() is obsolete use SlotLibrary.GetAllSlotAssets() instead", false)]
	public virtual SlotData[] GetAllSlots() { throw new Exception(); }

}

using UnityEngine;
using System.Collections;
using GameScene;

public class GameCanvas : GameMonoBehaviour
{

    public GameObject crosshair;
    public ObjectSelector selector;

    public override void Awake()
    {
        base.Awake();
        crosshair.SetActive(desktopMode);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        selector.enabled = true;
        crosshair.SetActive(desktopMode);
    }

    public override void OnInputModeChanged()
    {
        crosshair.SetActive(desktopMode);
    }

    public void OnDisable()
    {
        selector.enabled = false;
    }
}

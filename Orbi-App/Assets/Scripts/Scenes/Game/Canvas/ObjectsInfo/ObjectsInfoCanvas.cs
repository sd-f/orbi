using UnityEngine;
using System.Collections;
using GameController;
using UnityEngine.UI;
using System;
using System.Globalization;
using GameScene;

public class ObjectsInfoCanvas : MonoBehaviour {

    public GameObject objectInfoContainer;
    public GameObject characterInfoContainer;
    public InputField messageInputField;
    public GameObject removingEffect;
    public SelectedObjectCamera selectedObjectCamera;
    public MainCanvas canvas;
    public Button takeButton;
    public Button sendButton;

    private GameObject obj;

    public Text objectInfoDate;
    public Text characterName;

    private long characterId;
    private long objectId;

    private bool objSet = false;

    void OnEnable()
    {
        Game.Instance.EnterTypingMode();
    }

    void OnDisable()
    {
        CleanUp();
        canvas.Reset();
    }

    public void SetObject(ServerModel.GameObject obj)
    {
        try
        {
            this.obj = obj.gameObject;
            objSet = true;
            if (obj != null)
            {
                characterInfoContainer.SetActive(false);
                objectInfoContainer.SetActive(true);
                DateTime dt = DateTime.ParseExact(obj.createDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                objectInfoDate.text = dt.ToString("s", CultureInfo.InvariantCulture);
                objectId = obj.id;
                selectedObjectCamera.SetGameObject(obj.gameObject, GameObjectUtility.GetObject(obj.gameObject));
                selectedObjectCamera.gameObject.SetActive(true);
            }
            else
            {
                OnClose();
            }
        }
#pragma warning disable 0168
        catch (Exception e)
        {
            // silent
            OnClose();
        }

    }


    public void SetCharacter(ServerModel.Character character)
    {
        this.obj = character.gameObject;
        try
        {
            if (obj != null)
            {
                objSet = true;
                characterInfoContainer.SetActive(true);
                objectInfoContainer.SetActive(false);
                characterName.text = character.name;
                characterId = character.id;

                selectedObjectCamera.SetGameObject(character.gameObject, GameObjectUtility.GetObject(character.gameObject));
                selectedObjectCamera.gameObject.SetActive(true);
            }
            else
            {
                OnClose();
            }
#pragma warning disable 0168
        } catch(Exception e)
        {
            OnClose();
        }
    }

    void CleanUp()
    {
        selectedObjectCamera.gameObject.SetActive(false);
        objectInfoDate.text = "Date";
        characterName.text = "Name";
        characterId = -1;
        messageInputField.text = "";
        takeButton.interactable = true;
        sendButton.interactable = true;
        objSet = false;
    }

    public void OnSendMessage()
    {
        StartCoroutine("SendMessage");
    }

    void Update()
    {
        if (obj == null && objSet)
        {
            OnClose();
        }
    }

    IEnumerator SendMessage()
    {
        sendButton.interactable = false;
        yield return Game.Instance.GetPlayer().GetMessageService().RequestMessage(messageInputField.text, characterId);
        OnClose();
    }

    public void OnTake()
    {
        StartCoroutine("Take");
    }

    public IEnumerator Take()
    {
        takeButton.interactable = false;
        // effect
        ServerModel.Player player = Game.Instance.GetPlayer().GetModel();
        player.selectedObjectId = objectId;
        GameObject effect = GameObject.Instantiate(removingEffect) as GameObject;
        GameObject objToDestroy = GameObjectUtility.GetObject(obj.gameObject);
        Rigidbody body = GameObjectUtility.GetRigidBody(obj);
        effect.transform.SetParent(body.transform, false);
        yield return Game.Instance.GetPlayer().GetPlayerService().RequestDestroy(player, objToDestroy);
        OnClose();
        StartCoroutine(Game.Instance.GetWorld().UpdateObjects());
    }

    public void OnClose()
    {
        this.gameObject.SetActive(false);
        Game.Instance.LeaveTypingMode();
    }
}

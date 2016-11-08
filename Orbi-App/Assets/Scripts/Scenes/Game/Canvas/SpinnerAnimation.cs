using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameController.Services;
using GameController;

public class SpinnerAnimation : MonoBehaviour {

    private bool show = false;
    private Text icon;
    private Outline iconOutline;
    private float normalizedLoad = 0f;
    private float alpha = 0f;

    void Start()
    {
        icon = GetComponent<Text>();
        iconOutline = GetComponent<Outline>();
        if (!IsInvoking("UpdateLoadingIndicator"))
            InvokeRepeating("UpdateLoadingIndicator", 1, 1);
    }

    void OnEnable()
    {
        AbstractHttpService.OnNumberOfRequestsChanged += OnNumberOfRequestsChanged;
    }

    void OnDisable()
    {
        AbstractHttpService.OnNumberOfRequestsChanged -= OnNumberOfRequestsChanged;
    }

    void UpdateLoadingIndicator()
    {
       SetColor(normalizedLoad, 1-normalizedLoad);
    }

    void OnNumberOfRequestsChanged()
    {
        show = Game.Instance.GetClient().RunningRequests() > 1;
        if (show)
            normalizedLoad = 1f - ( (float)Game.Instance.GetClient().RunningRequests() / (float)AbstractHttpService.MAX_REQUESTS);
        else
            normalizedLoad = 0f;
    }

    void SetColor(float redAmount, float greenAmount)
    {
        Color color = icon.color;
        color.r = redAmount;
        icon.color = color;
    }

    Color SetAlpha(Color color, float alhpa)
    {
        Color newColor = color;
        newColor.a = alhpa;
        return newColor;
    }

    // Update is called once per frame
    void FixedUpdate () {
        //icon.text = normalizedLoad.ToString();
        if (show)
        {
            alpha = Mathf.Clamp(alpha + 0.05f, 0f, 1f);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, transform.localRotation.eulerAngles.z - 15f), Time.deltaTime * 20);
            icon.color = SetAlpha(icon.color, alpha);
            iconOutline.effectColor = SetAlpha(iconOutline.effectColor, alpha);
        } else
        {
            alpha = Mathf.Clamp(alpha - 0.05f, 0f, 1f);
            icon.color = SetAlpha(icon.color, alpha);
            iconOutline.effectColor = SetAlpha(iconOutline.effectColor, alpha);
        }
            
    }
}

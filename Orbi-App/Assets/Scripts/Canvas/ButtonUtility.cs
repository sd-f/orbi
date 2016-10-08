using UnityEngine;
using UnityEngine.UI;

namespace CanvasUtility
{
    public class ButtonUtility
    {
        public static void SetButtonState(GameObject button, bool state)
        {
            button.GetComponent<Button>().interactable = state;
            button.GetComponent<Image>().enabled = state;
            foreach (Image image in button.GetComponentsInChildren<Image>())
            {
                image.enabled = state;
            }
        }
    }
}

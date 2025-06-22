using UnityEngine;
using UnityEngine.UI;

public class ButtonHider : MonoBehaviour
{
    public Button[] buttonsToHide;
    public Button buttonPrefab;
    public Transform buttonParent;

    public void HideButtons()
    {
        foreach (Button btn in buttonsToHide)
        {
            btn.gameObject.SetActive(false);
        }
    }

    public void AddButton()
    {
        if (buttonPrefab == null || buttonParent == null)
            return;

        Instantiate(buttonPrefab, buttonParent);
    }
}
using UnityEngine;

public class ToggleCanvas : MonoBehaviour
{
    public GameObject canvasToToggle;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
    }

    // This function can be linked to a UI button's OnClick event
    public void Toggle()
    {
        if (canvasToToggle != null)
        {
            bool isActive = canvasToToggle.activeSelf;
            canvasToToggle.SetActive(!isActive);
        }
    }
}

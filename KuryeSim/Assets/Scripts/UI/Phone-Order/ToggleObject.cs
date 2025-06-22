using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject garageButton;
    
    public void ToggleActive()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(!targetObject.activeSelf);
            garageButton.SetActive(!targetObject.activeSelf);
            
        }
    }
}
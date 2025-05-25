using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    public GameObject targetObject;

    public void ToggleActive()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}
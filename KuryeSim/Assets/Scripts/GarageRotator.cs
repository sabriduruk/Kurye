using UnityEngine;

public class GarageRotator : MonoBehaviour
{
    public float autoRotateSpeed = 20f; // Kendili�inden d�n�� h�z�
    public float dragRotateSpeed = 0.5f; // Mouse ile d�nd�rme hassasiyeti

    private bool isDragging = false;
    private Vector3 lastMousePosition;

    void Update()
    {
        HandleMouseInput();

        // E�er mouse ile d�nd�r�lm�yorsa otomatik d�nd�r
        if (!isDragging)
        {
            transform.Rotate(Vector3.up, autoRotateSpeed * Time.deltaTime, Space.World);
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float rotationY = delta.x * dragRotateSpeed;

            transform.Rotate(Vector3.up, -rotationY, Space.World); // sola �ekince sola d�ner
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}

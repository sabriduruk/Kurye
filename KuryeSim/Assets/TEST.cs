using UnityEngine;

public class TEST : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Renderer renderer;
    public Vector2 offset;
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        renderer.material.mainTextureScale = offset;  // Texture’ı yatayda yarım kaydır
    }
}

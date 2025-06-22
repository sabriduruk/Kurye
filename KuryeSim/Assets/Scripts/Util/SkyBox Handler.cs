using UnityEngine;

public class SkyBoxHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Material default_skyBox;
    public Material night_skybox;
    public Light directionalLight;
    public Light motorSpotLight;
    void Start()
    {
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
        RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Skybox;
        RenderSettings.skybox = default_skyBox;
        directionalLight.gameObject.SetActive(true);
        DynamicGI.UpdateEnvironment();
        motorSpotLight.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
            RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Skybox;
            RenderSettings.skybox = default_skyBox;
            directionalLight.gameObject.SetActive(true);
            DynamicGI.UpdateEnvironment();
            motorSpotLight.gameObject.SetActive(false);

        }
        if(Input.GetKeyDown(KeyCode.Y)) {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
            RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Custom;
            RenderSettings.skybox = night_skybox;
            directionalLight.gameObject.SetActive(false);
            motorSpotLight.gameObject.SetActive(true);
            DynamicGI.UpdateEnvironment();
        }
    }
    void setGunduz() 
    {
        
    }
}

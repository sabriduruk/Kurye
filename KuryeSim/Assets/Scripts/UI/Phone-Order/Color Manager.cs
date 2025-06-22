using UnityEngine;

public class ColorManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Material motorMaterial;
    public MotorData globalMotorData;
    void Start()
    {
        motorMaterial.mainTexture = globalMotorData.motorTexture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;

public class RandomizeObjects : MonoBehaviour
{
    [SerializeField]
    Vector3 localRotationMin = Vector3.zero;
    [SerializeField]
    Vector3 localRotationMax = Vector3.zero;
    

    [SerializeField]
    float localScaleMultiplerMin = 0.8f;
    [SerializeField]
    float localScaleMultiplerMax = 1.5f;

    void OnEnable()
    {
        transform.localRotation = Quaternion.Euler(Random.Range(localRotationMin.x,localRotationMax.x),Random.Range(localRotationMin.y,localRotationMax.y),Random.Range(localRotationMin.z,localRotationMax.z));    
        transform.localScale = transform.localScale * Random.Range(localScaleMultiplerMin,localScaleMultiplerMax);

    }

}

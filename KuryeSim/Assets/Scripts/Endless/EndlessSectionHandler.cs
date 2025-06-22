using UnityEngine;
using UnityEngine.Rendering;

public class EndlessSectionHandler : MonoBehaviour
{
    [SerializeField] Transform playerCamTransform;
    void Start()
    {
        playerCamTransform = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = transform.position.z - playerCamTransform.position.z;
        float lerpPercentage = 1.0f - ((distanceToPlayer - 100) / 150.0f);
        lerpPercentage = Mathf.Clamp01(lerpPercentage);


        transform.position = Vector3.Lerp(new Vector3(transform.position.x,-10,transform.position.z),new Vector3(transform.position.x,0f,transform.position.z),lerpPercentage);
        


    }
}

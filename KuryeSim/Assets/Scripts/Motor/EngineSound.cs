using UnityEngine;

public class EngineSound : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody rb;
    AudioSource motorAudio;
    public MotorData motordata;
    public float minPitch = 0.2f;
    public float maxPitch = 1.0f;
    float maxSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        motorAudio = GetComponent<AudioSource>();    
        maxSpeed = motordata.maxForwardVelocity;
    }


    // Update is called once per frame
    void Update()
    {
        float speed = rb.linearVelocity.magnitude;
        float normalizedSpeed = Mathf.Clamp01(speed  / maxSpeed);
        motorAudio.pitch = Mathf.Lerp(minPitch,maxPitch,normalizedSpeed);
    }
}

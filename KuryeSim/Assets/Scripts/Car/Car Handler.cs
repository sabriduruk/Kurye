
using UnityEngine;


public class CarHandler : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    Transform gameModel;

    float maxsteerVelocity = 2;
    float maxForwardVelocity = 30;

    public float accelerationmultipler = 3;
    public float brakemultipler = 15;
    public float steeringmultipler = 15;
    Vector2 input = Vector2.zero;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 0;
    }


    void Update()
    {
        gameModel.transform.rotation = Quaternion.Euler(0, rb.linearVelocity.x * 5, 0);
    }

    private void FixedUpdate()
    {
        if (input.y > 0)
            Accelerate();
        else
            rb.linearDamping = 0.2f;
        if (input.y < 0)
            Brake();
        if (rb.linearVelocity.z <= 0)
            rb.linearVelocity = Vector3.zero;
        Steer();
    }

    void Accelerate()
    {
        rb.linearDamping = 0.0f;

        if (rb.linearVelocity.z >= maxForwardVelocity)
            return;

        rb.AddForce(rb.transform.forward * 10 * accelerationmultipler * input.y);

    }
    void Brake()
    {
        if (rb.linearVelocity.z <= 0)
            return;
        rb.AddForce(rb.transform.forward * 10 * brakemultipler * input.y);

    }
    void Steer()
    {
        if (Mathf.Abs(input.x) > 0)
        {
            float speedBaseSteerLimit = rb.linearVelocity.z / 5.0f;
            speedBaseSteerLimit = Mathf.Clamp01(speedBaseSteerLimit);

            rb.AddForce(rb.transform.right * steeringmultipler * input.x * speedBaseSteerLimit);

            float normalizedX = rb.linearVelocity.x / maxsteerVelocity;

            normalizedX = Mathf.Clamp(normalizedX, -1.0f, 1.0f);

            rb.linearVelocity = new Vector3(normalizedX * maxsteerVelocity, 0, rb.linearVelocity.z);

        }
        else
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, new Vector3(0, 0, rb.linearVelocity.z), Time.fixedDeltaTime * 3);
        }
    }
    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();
        input = inputVector;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
public class InputHandlerMotor : MonoBehaviour
{
    [SerializeField]
    MotorcycleHandler vehicleHandler;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = Vector2.zero;
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        vehicleHandler.SetInput(input);
        if(Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
    }
}

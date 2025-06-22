using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DistanceUtil : MonoBehaviour
{
    [Header("distance")]
    public float targetDistance;
    public TextMeshProUGUI distanceText;
    private Vector3 _startPos;
    
    public Rigidbody rb;
    private float _traveled;
    
    private void Start() {
       targetDistance = OrderManager.selectedOrder.distance;
    }
    private void Update() 
    {
        distancePerRigidbody();
    }

    private void distancePerRigidbody() {
         _traveled += rb.linearVelocity.magnitude * Time.deltaTime;
        float remaining = Mathf.Max(targetDistance - _traveled,0f);
        distanceText.text = $"Kalan: {remaining:F0} m";
    }
    private void distancePerVector() 
    {
        float _traveled = Vector3.Distance(_startPos,transform.position);
        float remaining = Mathf.Max(targetDistance - _traveled,0f);
        distanceText.text = $"Kalan: {remaining:F0} m";
    }

}

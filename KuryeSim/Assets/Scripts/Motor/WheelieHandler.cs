using DG.Tweening;
using UnityEngine;

public class WheelieHandler : MonoBehaviour
{
    public Transform rearPivot; // Arka teker hizasında pivot
    public float maxWheelieAngle = 30f;
    public float duration = 0.5f;      // Dönüş süresi
    public float wheelieHoldTime = 3f; // Wheelie pozisyonunda kalma süresi

    public bool canWheelie = true;
    private bool isWheeling = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canWheelie && !isWheeling)
        {
            StartWheelie();
        }
    }

   void StartWheelie()
    {
        isWheeling = true;

        // Wheelie pozisyonuna dön (X rotasyonu)
        rearPivot.DOLocalRotate(new Vector3(maxWheelieAngle, 0f, 0f), duration).OnComplete(() =>
        {
            // 3 saniye bekle, sonra normale dön
            DOVirtual.DelayedCall(wheelieHoldTime, () =>
            {
                rearPivot.DOLocalRotate(Vector3.zero, duration).OnComplete(() =>
                {
                    canWheelie = true;  // Wheelie tamamlandıktan sonra false yap
                    isWheeling = false;
                });
            });
        });
    }
    
    // private float currentAngle = 0f;

    // void Update()
    // {
    //     if (Input.GetKey(KeyCode.Space)) // Boşlukla kaldır
    //     {
    //         currentAngle = Mathf.Lerp(currentAngle, maxWheelieAngle, Time.deltaTime * wheelieSpeed);
    //     }
    //     else // Boşluk bırakınca geri indir
    //     {
    //         currentAngle = Mathf.Lerp(currentAngle, 0f, Time.deltaTime * wheelieSpeed);
    //     }

    //     rearPivot.localRotation = Quaternion.Euler(currentAngle, 0, 0);
    // }

}

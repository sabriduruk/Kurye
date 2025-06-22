using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using TMPro;
public class MotorcycleHandler : MonoBehaviour
{
    private float frontWheelRotation = 0f;
    private float rearWheelRotation = 0f;
    public MotorData motorData;
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlin;

    public TextMeshProUGUI memnuniyetText;

    [Header("Hareket Sınırları")]
    public float maxXPosition = 0.84f; // X eksenindeki maksimum pozisyon sınırı
    public float minXPosition = -0.84f; // X eksenindeki minimum pozisyon sınırı

    [SerializeField] Rigidbody rb;
    [SerializeField] Transform bikeModel; // motorsiklet modeli
    [SerializeField] Transform riderModel; // sürücü modeli
    [SerializeField] Animator animator;
    
    [Header("Tekerlek ve Direksiyon Referansları")]
    [SerializeField] Transform frontWheel; // ön tekerlek modeli
    [SerializeField] Transform rearWheel; // arka tekerlek modeli
    [SerializeField] Transform handlebar; // gidon/direksiyon modeli

    [SerializeField] GameObject trail;
    [SerializeField] TrailRenderer skidmark;
    

    
    [Header("Skidmark Ayarları")]
    public float skidmarkSpeedThreshold = 0.95f; // Maksimum hızın yüzde kaçında iz bırakmayı durduracak
    
    
    Vector2 input = Vector2.zero;
    private float currentLean = 0f;
    private float currentSteeringAngle = 0f;
    private bool isNearMaxSpeed = false;
    
    // Animasyon durumları için değişkenler
    private bool wasMoving = false;
    private bool isInGoAnimation = false;
    private float goAnimationStartTime = 0f;
    public float slowDownFactor;


    public static int memnuniyetYuzdesi = 100;
    void Start()
    {
        
        memnuniyetText.text = "Memnuniyet " + memnuniyetYuzdesi.ToString();
        if (virtualCamera == null)
            virtualCamera = Camera.main.GetComponent<CinemachineVirtualCamera>();

        perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        
        if(rb == null)
            rb = GetComponent<Rigidbody>();
        
        rb.linearDamping = 0;
        animator.applyRootMotion = false;
        if (bikeModel == null)
            bikeModel = transform.GetChild(0);
        
        // Animator kontrolü
        if (animator == null)
            Debug.LogWarning("Animator bileşeni atanmamış! Inspector'dan 'Animator' alanını doldurunuz.");
        
        // Sürücü modeli kontrolü
        if (riderModel == null)
            Debug.LogWarning("Sürücü modeli atanmamış! Inspector'dan 'Rider Model' alanını doldurunuz.");
        
        skidmark = trail.GetComponent<TrailRenderer>();
        skidmark.emitting = false;
        memnuniyetYuzdesi = 100;
        // Başlangıçta Idle animasyonunda olduğundan emin ol
        //ResetAnimatorParameters();
    }

    void Update()
    {
        UpdateVisuals();
        //UpdateAnimations();
        //Debug.Log(rb.linearVelocity.z);
    }
    
    void UpdateVisuals()
    {
        
        // Yatma efekti için hedef açı hesaplama
        float targetLean = -input.x * motorData.maxLeanAngle; // Sağa/sola tuşa basınca ters yöne yatma
        
        // Hıza bağlı olarak yatma etkisini artırma (isteğe bağlı)
        float speedFactor = Mathf.Clamp01(rb.linearVelocity.z / 10f);
        targetLean *= speedFactor;
        
        // Yumuşak geçiş için lerp kullanımı
        currentLean = Mathf.Lerp(currentLean, targetLean, Time.deltaTime * motorData.leanSpeed);
        
        // Direksiyon açısı hesaplama ve yumuşatma
        float targetSteeringAngle = input.x * motorData.maxSteeringAngle;
        currentSteeringAngle = Mathf.Lerp(currentSteeringAngle, targetSteeringAngle, Time.deltaTime * 5f);
        
        // Modelin dönüşlerini set etme
        float forwardRotation = rb.linearVelocity.x * 3f; // Dönüş sırasındaki yön
        
        // Model rotasyonunu ayarlama (Z eksenindeki roll değeri yatmayı gösterir)
        bikeModel.localRotation = Quaternion.Euler(0, forwardRotation, currentLean);
        
        // Sürücü rotasyonunu ayarlama (motorsiklet ile birlikte yatsın)
        if (riderModel != null)
        {
            // Sürücünün motorsikletle aynı açıda (veya çarpan ile modifiye edilmiş) yatmasını sağla
            float riderLean = currentLean * motorData.riderLeanMultiplier;
            riderModel.localRotation = Quaternion.Euler(
                motorData.riderLeanOffset.x, 
                forwardRotation + motorData.riderLeanOffset.y, 
                riderLean + motorData.riderLeanOffset.z
            );
        }
        
        // Direksiyonu (gidon) döndürme
        // if (handlebar != null)
        // {
        //     // Y ekseni etrafında döndürme (sağa/sola)
        //     handlebar.localRotation = Quaternion.Euler(-125.5f, 0, currentSteeringAngle * motorData.handlebarRotationMultiplier);
        // }
        
        // Tekerlekleri döndürme
        if (frontWheel != null && rearWheel != null)
        {
            float rotationSpeed = rb.linearVelocity.z / (2f * Mathf.PI * motorData.wheelRadius) * 360f; // derece/saniye
            frontWheelRotation += rotationSpeed * Time.deltaTime;
            rearWheelRotation += rotationSpeed * Time.deltaTime;
            frontWheel.localRotation = Quaternion.Euler(frontWheelRotation, currentSteeringAngle, 0f);
            rearWheel.localRotation = Quaternion.Euler(rearWheelRotation, 0f, 0f);
            
        }
    }
    
    // void UpdateAnimations()
    // {
    //     if (animator == null) return;
        
    //     float currentSpeed = rb.linearVelocity.z;
    //     bool isMoving = currentSpeed > animationSpeedThreshold;
    //     bool isAccelerating = input.y > 0;
    //     bool isStopping = wasMoving && currentSpeed < stopAnimationThreshold;
        
    //     // Animator'a hız değerini gönder (blend tree kullanımı için)
    //     animator.SetFloat(Speed, currentSpeed / motorData.maxForwardVelocity);
        
    //     // Durumlar arası geçiş kontrolü
    //     if (!wasMoving && isAccelerating)
    //     {
    //         // Duruyorken hızlanıyorsa "Go" animasyonuna geç
    //         ResetAnimatorParameters();
    //         animator.SetBool(IsAccelerating, true);
    //         isInGoAnimation = true;
    //         goAnimationStartTime = Time.time;
    //     }
    //     else if (isInGoAnimation && (Time.time - goAnimationStartTime) > goToRunIdleTransitionTime)
    //     {
    //         // "Go" animasyonundan sonra belirli süre geçince "RunIdle" ye geç
    //         ResetAnimatorParameters();
    //         animator.SetBool(IsRunning, true);
    //         isInGoAnimation = false;
            
    //     }
    //     else if (wasMoving && isStopping && !animator.GetBool(IsStopping))
    //     {
    //         // Hareket halindeyken durmaya başladıysa "Test Idle" animasyonuna geç
    //         ResetAnimatorParameters();
    //         animator.SetBool(IsStopping, true);
    //     }
    //     else if (!isMoving && !isInGoAnimation && animator.GetBool(IsStopping))
    //     {
    //         // "Test Idle" animasyonu tamamlandıktan sonra ana "Idle" animasyonuna dön
    //         // Burada Test Idle animasyonu bitimini algılamak için AnimationEvent kullanılabilir
    //         // Şimdilik basitçe durduktan sonra geçiş yapalım
    //         if (currentSpeed <= 0)
    //         {
    //             ResetAnimatorParameters();
    //         }
    //     }
        
    //     wasMoving = isMoving;
    // }
    
    // // Tüm animator parametrelerini sıfırla (temiz durum geçişleri için)
    // void ResetAnimatorParameters()
    // {
    //     animator.SetBool(IsRunning, false);
    //     animator.SetBool(IsAccelerating, false);
    //     animator.SetBool(IsStopping, false);
    // }

    private void FixedUpdate()
    {
        // Skidmark hızını kontrol et ve flickering'i önlemek için kontrol
        float speedRatio = rb.linearVelocity.z / motorData.maxForwardVelocity;
        
        // Maksimum hıza yakın mı kontrol et (histerezis kullanarak)
        if (!isNearMaxSpeed && speedRatio >= skidmarkSpeedThreshold) {
            isNearMaxSpeed = true;
            skidmark.emitting = false;
        }
        else if (isNearMaxSpeed && speedRatio < skidmarkSpeedThreshold * 0.95f) {
            isNearMaxSpeed = false;
        }
        
        if (input.y > 0) {
            Accelerate();
        }
        else
            rb.linearDamping = 0.2f;
        if (input.y < 0)
            Brake();
        
        Steer();
        
        // X ekseninde pozisyon sınırlaması
        LimitXPosition();
    }

    void Accelerate()
    {
        rb.linearDamping = 0.0f;
        
        if (!isNearMaxSpeed && input.y > 0) {
            skidmark.emitting = true;
        }
        if (rb.linearVelocity.z >= motorData.maxForwardVelocity) {
            return;
        }
        rb.AddForce(rb.transform.forward * 10 * motorData.accelerationMultiplier * input.y);
    }
    
    void Brake()
    {
        if (rb.linearVelocity.z <= 0) {
            rb.linearVelocity = Vector3.zero;
            return;
        }
        rb.AddForce(rb.transform.forward * 10 * motorData.brakeMultiplier * input.y);
        
    }
    
    void Steer()
    {
        if (Mathf.Abs(input.x) > 0)
        {
            // Sınır kontrolü - eğer sınıra yaklaşıyorsa o yöndeki hareketi engelle
            bool canMoveRight = transform.position.x < maxXPosition;
            bool canMoveLeft = transform.position.x > minXPosition;
            
            // Sadece izin verilen yönde hareket etmeye izin ver
            float allowedInput = input.x;
            if (input.x > 0 && !canMoveRight) allowedInput = 0; // Sağa gitmeye çalışıyor ama sınırda
            if (input.x < 0 && !canMoveLeft) allowedInput = 0; // Sola gitmeye çalışıyor ama sınırda
            
            // Hıza bağlı olarak dönüş limitini ayarlama (daha yüksek hızlarda daha az dönüş)
            float speedBasedSteerLimit = rb.linearVelocity.z / 5.0f;
            speedBasedSteerLimit = Mathf.Clamp01(speedBasedSteerLimit);

            rb.AddForce(rb.transform.right * motorData.steeringMultiplier * allowedInput * speedBasedSteerLimit);

            float normalizedX = rb.linearVelocity.x / motorData.maxSteerVelocity;
            normalizedX = Mathf.Clamp(normalizedX, -1.0f, 1.0f);
            
            rb.linearVelocity = new Vector3(normalizedX * motorData.maxSteerVelocity, 0, rb.linearVelocity.z);
        }
        else
        {
            // Direksiyonu düz konuma getir
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, new Vector3(0, 0, rb.linearVelocity.z), Time.fixedDeltaTime * 3);
        }
    }
    
    void LimitXPosition()
    {
        // Pozisyon sınırlaması (ekstra güvenlik için)
        Vector3 pos = transform.position;
        if (pos.x > maxXPosition)
        {
            pos.x = maxXPosition;
            transform.position = pos;
            // X hızını sıfırla eğer sınırı aştıysa
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, rb.linearVelocity.z);
        }
        else if (pos.x < minXPosition)
        {
            pos.x = minXPosition;
            transform.position = pos;
            // X hızını sıfırla eğer sınırı aştıysa
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, rb.linearVelocity.z);
        }
    }
    
    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();
        input = inputVector;
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Engel"))
        {
            rb.linearVelocity *= slowDownFactor;
            StartCoroutine(ShakeCamera(0.3f, 2f));
            memnuniyetYuzdesi -= 10;
            memnuniyetText.text = "Memnuniyet " + memnuniyetYuzdesi.ToString();
        }
    }
    IEnumerator ShakeCamera(float duration, float intensity)
    {
        perlin.AmplitudeGain = intensity;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        perlin.AmplitudeGain = 0f;
    }
}
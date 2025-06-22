using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeAndDistance : MonoBehaviour
{
    private bool isGameFinished = false;
    private bool canGoOrderScene = false;
    private bool isWinned = false;
    private bool fiveSecondsWarningPlayed = false; // 5 saniye uyarısının çalınıp çalınmadığını kontrol eder
    
    [Header("Audio Clips")]
    public AudioClip motorClip;
    public AudioClip succesClip;
    public AudioClip failedClip;
    public AudioSource playerAudioSource; // Motor sesi için
    public AudioSource warningAudioSource; // Uyarı sesleri için ayrı AudioSource
    private AudioClip clipToRun;
    public AudioClip fiveSecondsCountDown;

    [Header("UI Objects")]
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI memnuniyet;
    
    [Header("Order Details")]
    public OrderData currentOrder;

    [Header("Player Vars")]
    public PlayerVars playerVars;
    
    [Header("SkyBox Handler")]
    public Material default_skyBox;
    public Material night_skybox;
    public Light directionalLight;
    public Light motorSpotLight;

    [Header("Distance and TIME UTIL")]
    public Rigidbody motorRigidBody;
    private float targetDistance;
    private float _traveled;
    private float targetduration;
    private float remainingDistance;

    [Header("Complate Canvas")]
    public GameObject orderComplateCanvas;
    public GameObject normalCanvas;

    public Image titleImage;
    public TextMeshProUGUI titleText;
    
    public TextMeshProUGUI resultText;

    public Image rewardImage;
    public TextMeshProUGUI rewardtext;

    public TextMeshProUGUI durationText;

    
    void Start()
    {
        currentOrder = OrderManager.selectedOrder;
        if(currentOrder.deliveryTime == "Gündüz")
            setGunduz();
        else
            setGece();
        distanceText.text = OrderManager.selectedOrder.distance.ToString();
        timerText.text = OrderManager.selectedOrder.duration.ToString();

        targetDistance = OrderManager.selectedOrder.distance;
        targetduration = OrderManager.selectedOrder.duration;

        orderComplateCanvas.SetActive(false);

        // Motor sesi için ana AudioSource'u ayarla
        playerAudioSource = GameObject.FindGameObjectWithTag("Player")?.GetComponent<AudioSource>();
        playerAudioSource.clip = motorClip;
        
        // Uyarı sesleri için ikinci AudioSource'u ayarla (eğer yoksa oluştur)
        if (warningAudioSource == null)
        {
            GameObject warningGO = new GameObject("WarningAudioSource");
            warningGO.transform.SetParent(transform);
            warningAudioSource = warningGO.AddComponent<AudioSource>();
            warningAudioSource.playOnAwake = false;
            warningAudioSource.volume = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameFinished == false) {
            distancePerRigidbody();
            durationPerSeconds();
            
            // 5 saniye uyarısını kontrol et
            CheckFiveSecondsWarning();
        }
        if(!isGameFinished && remainingDistance <= 0) {
            isGameFinished = true;
            isWinned = true;
            ShowComplateUI();
            
        }
        else if(!isGameFinished && targetduration <= 0f)
        {
            isGameFinished = true;
            isWinned = false;
            ShowComplateUI();

        }
    }
    
    private void CheckFiveSecondsWarning()
    {
        // Eğer süre 5 saniyeye düştüyse ve daha önce uyarı çalmadıysa
        if (targetduration <= 5f && targetduration > 0f && !fiveSecondsWarningPlayed)
        {
            fiveSecondsWarningPlayed = true;
            
            // Motor sesini geçici olarak kıs
            float originalVolume = playerAudioSource.volume;
            playerAudioSource.volume = originalVolume * 0.3f; // Motor sesini %30'una düşür
            
            // 5 saniye uyarı sesini çal
            if (fiveSecondsCountDown != null)
            {
                warningAudioSource.PlayOneShot(fiveSecondsCountDown);
                
                // Uyarı sesi bittiğinde motor sesini eski haline getir
                StartCoroutine(RestoreMotorVolumeAfterWarning(originalVolume, fiveSecondsCountDown.length));
            }
        }
    }
    
    private System.Collections.IEnumerator RestoreMotorVolumeAfterWarning(float originalVolume, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (playerAudioSource != null)
        {
            playerAudioSource.volume = originalVolume;
        }
    }
    
    void setGunduz() 
    {
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
        RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Skybox;
        RenderSettings.skybox = default_skyBox;
        directionalLight.gameObject.SetActive(true);
        DynamicGI.UpdateEnvironment();
        motorSpotLight.gameObject.SetActive(false);
    }
    void setGece() {
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Custom;
        RenderSettings.skybox = night_skybox;
        directionalLight.gameObject.SetActive(false);
        motorSpotLight.gameObject.SetActive(true);
        DynamicGI.UpdateEnvironment();
    }
    
    private void distancePerRigidbody() {
         _traveled += motorRigidBody.linearVelocity.magnitude * Time.deltaTime;
        remainingDistance = Mathf.Max(targetDistance - _traveled,0f);
        distanceText.text = $"Kalan: {remainingDistance:F0} m";
    }
    private void durationPerSeconds() {
        if(targetduration <= 0f)
            return;
        if(targetduration > 0) {
            targetduration -= Time.deltaTime;
            targetduration = Mathf.Max(targetduration,0f);
        }
        timerText.text = Mathf.CeilToInt(targetduration).ToString() + " sn"; 
    }

    private void ShowComplateUI()
    {
        if(canGoOrderScene) return; // ✅ zaten gösterildiyse tekrar girme
        normalCanvas.SetActive(false);
        orderComplateCanvas.SetActive(true);
        float reward;
        if(isWinned) {
            if(OrderManager.CheckBagAgainstOrder(OrderManager.selectedOrder)) 
            {
                titleImage.color = Color.green;
                reward = OrderManager.selectedOrder.reward * ((float)MotorcycleHandler.memnuniyetYuzdesi / 100f);
                playerVars.totalMoney += reward;
                clipToRun = succesClip;
                resultText.text = OrderManager.CheckBagAgainstOrderWithString(OrderManager.selectedOrder);
            }
            else {
                titleImage.color = Color.red;
                reward = 0;
                playerVars.totalMoney += reward;
                playerAudioSource.loop = false;
                clipToRun = failedClip;
                resultText.text = OrderManager.CheckBagAgainstOrderWithString(OrderManager.selectedOrder);
            }
        }
        else {
            titleImage.color = Color.red;
            reward = 0;
            playerVars.totalMoney += reward;
            clipToRun = failedClip;
            resultText.text = "Geciktiniz...";
        }        
        rewardtext.text = $"{reward.ToString():F0} TL";
        playerVars.totalMoney += Mathf.CeilToInt(reward);
        durationText.text = Mathf.CeilToInt(targetduration).ToString();
        
        playerAudioSource.Stop(); // Motor sesini durdur
        playerAudioSource.clip = clipToRun; // Yeni sesi ata
        playerAudioSource.loop = false; // Loop'u kapat
        playerAudioSource.Play(); // Sonuç sesini çal
        Time.timeScale = 0.3f;        
        canGoOrderScene = true;
    }
    public void loadOrderScene() {
        SceneManager.LoadScene("OrderScene");
    }
}
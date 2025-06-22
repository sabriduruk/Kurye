using UnityEngine;
using TMPro;

public class OrderTimer : MonoBehaviour
{
    public float totalTime = 30f;
    private float timeRemaining;
    private bool timerRunning = false;

    public TMP_Text timerText;
    
    [Header("panel")]
    public GameObject orderPanel;
    public GameObject cantaPanel;
    public GameObject depoPanel;
    
    public GameObject canvasRoot; // Tüm UI’ları kapsayan üst canvas objesi
    public GameObject phoneIcon;
    public GameObject InventoryUI;
    public GameObject OrderComplateCanvas;

    public OrderManager orderManager; // Inspector'dan bağla

    void Update()
    {
        if (!timerRunning) return;

        timeRemaining -= Time.deltaTime;
        timerText.text = Mathf.CeilToInt(timeRemaining).ToString() + " sn";

        if (timeRemaining <= 0)
        {
            timerRunning = false;
            timerText.text = "Süre bitti!";

            // Süre bittiğinde OrderManager'a haber ver
            if (orderManager != null)
            {
                orderManager.OnTimeExpired();
            }
            else
            {
                Debug.LogWarning("OrderManager referansı yok!");
            }

            canvasRoot.SetActive(false); // Tüm canvas'ları kapat
            
        }
    }

    public void reactiveCanvas() 
    {
        canvasRoot.SetActive(true); // Tüm canvas'ları kapat
        orderPanel.SetActive(false);
        InventoryUI.SetActive(false);
        OrderComplateCanvas.SetActive(false);
        phoneIcon.SetActive(true);
    }
    public void StartTimer()
    {
        timeRemaining = totalTime;
        timerRunning = true;

        depoPanel.SetActive(true);
        cantaPanel.SetActive(true);
        timerText.gameObject.SetActive(true);
    }

    public void ApproveManually()
    {
        timerRunning = false;
        canvasRoot.SetActive(false); // Süre dolmadan da kapat
    }
    public void OnAcceptOrder()
    {
        StartTimer();                    // Sayaç başlat
        phoneIcon.SetActive(false);    // Detay ekranı kapanır
        
    }
}
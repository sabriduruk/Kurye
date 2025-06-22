using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    [FormerlySerializedAs("phoneUI")] [Header("Ana Sipariş UI")]
    public GameObject phoneCanvas; // 🔹 Tüm UI paneli (sipariş, detay, depo vs.)

    public GameObject phoneIcon;
    
    [Header("Sipariş Listesi")]
    public GameObject orderButtonPrefab;
    public Transform orderListParent;
    public GameObject ordersHeader;
    

    [Header("Detay Paneli")]
    public GameObject orderDetailPanel;
    public TMP_Text titleText;
    public TMP_Text itemsText;
    public TMP_Text rewardText;
    public TMP_Text durationText;
    public TMP_Text timeText;
    
    [FormerlySerializedAs("orderCompletePanel")] [Header("Tamamlandı Paneli")]
    public GameObject orderCompleteCanvas;
    public TMP_Text completeTitleText;
    public TMP_Text completeRewardText;
    public TMP_Text completeDurationText;
    public TMP_Text resultMessageText;


   
    public static OrderData selectedOrder;

    public List<OrderData> currentOrders = new();
    
    void Start()
        {
            for (int i = 0; i < 3; i++) // 3 rastgele sipariş
            {
                var order = GenerateRandomOrder();
                currentOrders.Add(order);

                GameObject newButton = Instantiate(orderButtonPrefab, orderListParent);
                newButton.GetComponentInChildren<TMP_Text>().text = order.orderName;

                newButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    ShowOrderDetail(order);
                });
            }
        }

    

    void ShowOrderDetail(OrderData order)
    {
        selectedOrder = order;
        
        
        ordersHeader.SetActive(false); 
        orderListParent.gameObject.SetActive(false);
        if (orderDetailPanel == null)
        {
            Debug.LogError("OrderDetailPanel bağlanmamış!");
            return;
        }
        orderDetailPanel.SetActive(true);

        titleText.text = order.orderName;

        itemsText.text = "";
        foreach (var orderItem in order.items)
        {
            itemsText.text += $"• {orderItem.item.itemName} x{orderItem.count}\n";
        }


        rewardText.text = order.reward.ToString() + " TL";
        durationText.text = order.duration + " sn";
        timeText.text = order.deliveryTime;
    }

    public void OpenPhoneOrders()
    {
        Debug.Log("Telefon butonuna basıldı.");
        
        ordersHeader.SetActive(true);
        orderListParent.gameObject.SetActive(true); // Sipariş listesi aç
        orderDetailPanel.SetActive(false); // Detay paneli kapanır
    }
    
    public OrderData GenerateRandomOrder()
    {
        OrderData order = new OrderData();
        order.orderName = "Sipariş #" + Random.Range(100, 999);
        order.duration = Random.Range(20, 40);
        
        string[] times = { "Gece", "Gündüz" };
        order.deliveryTime = times[Random.Range(0, times.Length)];
        if(order.deliveryTime == "Gece") {
            order.reward = Random.Range(125, 300);
        }
        else {
            order.reward = Random.Range(50, 200);
        }

        // Ana Yemek
        var main = InventoryManager.Instance.mainDishes;
        var mainItem = main[Random.Range(0, main.Count)];
        order.items.Add(new OrderItem { item = mainItem, count = Random.Range(1, 4) });

        // Ara Yemek
        var side = InventoryManager.Instance.sideDishes;
        var sideItem = side[Random.Range(0, side.Count)];
        order.items.Add(new OrderItem { item = sideItem, count = Random.Range(1, 3) });

        // İçecek
        var drinks = InventoryManager.Instance.drinks;
        var drinkItem = drinks[Random.Range(0, drinks.Count)];
        order.items.Add(new OrderItem { item = drinkItem, count = 1 });

        // uzaklık ayarlama kısmı burada aslında
        order.distance = Mathf.RoundToInt(order.duration * 8.5f);
        
        

        return order;
    }
    
    public static bool CheckBagAgainstOrder(OrderData order)
    {
        // Siparişteki ürün ve adet bilgilerini Dictionary yapalım
        Dictionary<string, int> orderItems = new();
        foreach (var orderItem in order.items)
        {
            orderItems[orderItem.item.itemName] = orderItem.count;
        }

        // Çantadaki ürünleri oku
        Dictionary<string, int> bagItems = new();
        foreach (var slot in InventoryManager.Instance.bagSlots.Values)
        {
            string name = slot.itemData.itemName;
            int count = slot.count;

            if (bagItems.ContainsKey(name))
                bagItems[name] += count;
            else
                bagItems[name] = count;
        }

        // Sipariş ile çantayı karşılaştır
        foreach (var kvp in orderItems)
        {
            string itemName = kvp.Key;
            int requiredCount = kvp.Value;

            bagItems.TryGetValue(itemName, out int bagCount);

            if (bagCount != requiredCount)
            {

                Debug.Log($"Ürün {itemName} adedi uyuşmuyor. Gerekli: {requiredCount}, Çanta: {bagCount}");
                return false;
            }
        }

        // Ayrıca çantada siparişte olmayan fazladan ürün varsa onu da kontrol et
        foreach (var kvp in bagItems)
        {
            if (!orderItems.ContainsKey(kvp.Key))
            {
                Debug.Log($"Çantada siparişte olmayan ürün var: {kvp.Key}");
                return false;
            }
        }
        return true; // Her şey uyuyorsa true döner
    }

    public static string CheckBagAgainstOrderWithString(OrderData order)
    {
        string str = "Çantadaki ürünler doğru teslim edildi";
        // Siparişteki ürün ve adet bilgilerini Dictionary yapalım
        Dictionary<string, int> orderItems = new();
        foreach (var orderItem in order.items)
        {
            orderItems[orderItem.item.itemName] = orderItem.count;
        }

        // Çantadaki ürünleri oku
        Dictionary<string, int> bagItems = new();
        foreach (var slot in InventoryManager.Instance.bagSlots.Values)
        {
            string name = slot.itemData.itemName;
            int count = slot.count;

            if (bagItems.ContainsKey(name))
                bagItems[name] += count;
            else
                bagItems[name] = count;
        }

        // Sipariş ile çantayı karşılaştır
        foreach (var kvp in orderItems)
        {
            string itemName = kvp.Key;
            int requiredCount = kvp.Value;

            bagItems.TryGetValue(itemName, out int bagCount);

            if (bagCount != requiredCount)
            {
                str = $"Ürün {itemName} adedi uyuşmuyor. Gerekli: {requiredCount}, Çanta: {bagCount}";
                return str;
            }
        }

        // Ayrıca çantada siparişte olmayan fazladan ürün varsa onu da kontrol et
        foreach (var kvp in bagItems)
        {
            if (!orderItems.ContainsKey(kvp.Key))
            {
                str = $"Çantada siparişte olmayan ürün var: {kvp.Key}";
                return str;
            }
        }
        return str; // Her şey uyuyorsa true döner
    }
    
    public void ConfirmBag()
    {
        if (selectedOrder == null)
        {
            Debug.LogWarning("Sipariş seçilmedi!");
            return;
        }

        bool isOrderCorrect = CheckBagAgainstOrder(selectedOrder);

        // 🔒 Tüm UI’ı kapat ama ayrı canvas’taki tamamlandı paneli açık kalacak
        // phoneCanvas.SetActive(false);
        // ShowOrderCompletePanel(selectedOrder, isOrderCorrect);
    }

    
    public void ShowOrderCompletePanel(OrderData order, bool isCorrect)
    {
        orderCompleteCanvas.SetActive(true);
        
        completeTitleText.text = "Sipariş Tamamlandı!";
        completeRewardText.text = "+" + order.reward + " TL";
        completeDurationText.text = order.duration + " saniyede teslim edildi";

        if (isCorrect)
        {
            resultMessageText.text = "Sipariş doğru teslim edildi!";
            resultMessageText.color = new Color(0.1f, 0.8f, 0.1f);
        }
        else
        {
            resultMessageText.text = "Siparişte eksik veya fazla ürün vardı!";
            resultMessageText.color = Color.red;
        }
    }

    
    public void CloseOrderCompletePanel()
    {
        orderCompleteCanvas.SetActive(false);
        ResetPhoneUI(); // 🔁 Her şeyi sıfırla, sade sipariş ekranına dön
    }

    
    public void ResetPhoneUI()
    {
        // Ana canvas açık
        phoneCanvas.SetActive(true);

        // Gereksiz tüm panelleri kapat
        orderDetailPanel.SetActive(false);
        InventoryManager.Instance.bagParent.parent.gameObject.SetActive(false); // çanta
        InventoryManager.Instance.depotParent.parent.gameObject.SetActive(false); // depo

        // (Eğer varsa farklı panelleri de buraya kapatabilirsin)

        phoneIcon.SetActive(true);
        // Sadece sipariş listesi görünür olsun
        ordersHeader.SetActive(true);
        orderListParent.gameObject.SetActive(true);
    }
    
    
    public void OnTimeExpired()
    {
        bool bagIsEmpty = InventoryManager.Instance.bagSlots.Count == 0;

        if (selectedOrder == null || bagIsEmpty)
        {
            // OrderManager instance'ı varsa ona ulaş
            // var orderManager = FindObjectOfType<OrderManager>();
            var orderManager = FindFirstObjectByType<OrderManager>();
            if (orderManager != null)
            {
                orderManager.orderCompleteCanvas.SetActive(true);

                // Textleri istediğin şekilde ayarla
                orderManager.completeTitleText.text = "Sipariş İptali!";
                orderManager.completeRewardText.text = "0 TL";
                orderManager.completeDurationText.text = "-";
                orderManager.resultMessageText.text = "Gereken Sürede Teslim Edilemedi.";
                orderManager.resultMessageText.color = Color.red;
            }

            // Çantayı temizle
            ClearBag();
        }
        else
        {
            Debug.Log("Süre bitti ama çantada ürün var, sipariş onayı bekleniyor.");
            LogBagContents();
        }
    }


    public void ClearBag()
    {
        foreach(var slot in InventoryManager.Instance.bagSlots.Values)
        {
            Destroy(slot.gameObject);
        }
        InventoryManager.Instance.bagSlots.Clear();
    }
    
    public void LogBagContents()
    {
        var bagSlots = InventoryManager.Instance.bagSlots;

        if (bagSlots.Count == 0)
        {
            Debug.Log("Çanta boş.");
            return;
        }

        Debug.Log("Çantadaki ürünler:");
        foreach (var slot in bagSlots.Values)
        {
            Debug.Log($"{slot.itemData.itemName} x{slot.count}");
        }
    }







    
    
}
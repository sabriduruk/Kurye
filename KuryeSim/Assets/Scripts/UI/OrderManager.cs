using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
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
   
    public OrderData selectedOrder;
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


        rewardText.text = order.reward + " TL";
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
        order.reward = Random.Range(50, 200);
        order.duration = Random.Range(20, 60);
        
        string[] times = { "gece", "gündüz" };
        order.deliveryTime = times[Random.Range(0, times.Length)];

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

        return order;
    }
    
    bool CheckBagAgainstOrder(OrderData order)
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
    
    public void ConfirmBag()
    {
        if (selectedOrder == null)
        {
            Debug.LogWarning("Sipariş seçilmedi!");
            return;
        }

        bool isOrderCorrect = CheckBagAgainstOrder(selectedOrder);

        if (isOrderCorrect)
        {
            Debug.Log("Sipariş doğru tamamlandı!");
            // Burada ödül verme, süre sayacı başlatma vs. yapılabilir
        }
        else
        {
            Debug.Log("Siparişte eksik ya da fazla ürün var.");
            // Oyuncuya uyarı verilebilir
        }
    }



    
    
}
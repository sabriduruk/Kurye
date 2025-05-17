using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    [Header("Sipariş Listesi")]
    public GameObject orderButtonPrefab;
    public Transform orderListParent;
    

    [Header("Detay Paneli")]
    public GameObject orderDetailPanel;
    public TMP_Text titleText;
    public TMP_Text itemsText;
    public TMP_Text rewardText;
    public TMP_Text durationText;
    public TMP_Text timeText;

    [Header("Sipariş Verileri")]
    public OrderData[] allOrders;

    void Start()
    {
        foreach (OrderData order in allOrders)
        {
            GameObject newButton = Instantiate(orderButtonPrefab, orderListParent);
            newButton.GetComponentInChildren<TMP_Text>().text = order.orderName;

            // Her butona bu siparişi tanımla
            newButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                ShowOrderDetail(order);
            });
        }
    }

    void ShowOrderDetail(OrderData order)
    {
        orderListParent.gameObject.SetActive(false);
        if (orderDetailPanel == null)
        {
            Debug.LogError("OrderDetailPanel bağlanmamış!");
            return;
        }
        orderDetailPanel.SetActive(true);

        titleText.text = order.orderName;

        itemsText.text = "";
        foreach (var item in order.items)
        {
            itemsText.text += "• " + item + "\n";
        }

        rewardText.text = order.reward + " TL";
        durationText.text = order.duration + " sn";
        timeText.text = order.deliveryTime;
    }

    public void OpenPhoneOrders()
    {
        Debug.Log("Telefon butonuna basıldı.");

        orderListParent.gameObject.SetActive(true); // Sipariş listesi aç
        orderDetailPanel.SetActive(false); // Detay paneli kapanır
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [Header("UI Bileşenleri")]
    public Image iconImage;                    // Buraya Image component atanacak
    public TextMeshProUGUI countText;          // Kaç tane olduğunu gösteren text

    [Header("Veri")]
    public ItemData itemData;                  // Bu slotun gösterdiği ürün
    public int count = 0;

    [Header("Ayarlar")]
    public bool isDepot = false;               // Depo mu, çanta mı?

    public void SetItem(ItemData data, int amount = 0)
    {
        itemData = data;
        iconImage.sprite = data.icon;          // Sprite'ı göster
        iconImage.enabled = true;              // Görünürlüğü aç
        count = amount;
        UpdateUI();
    }

    public void UpdateUI()
    {
        countText.text = count > 0 ? count.ToString() : "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDepot && eventData.button == PointerEventData.InputButton.Left)
        {
            InventoryManager.Instance.AddToBag(itemData);
        }
        else if (!isDepot && eventData.button == PointerEventData.InputButton.Right)
        {
            InventoryManager.Instance.RemoveFromBag(itemData);
        }
    }
}
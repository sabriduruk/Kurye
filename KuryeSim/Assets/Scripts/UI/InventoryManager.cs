using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Transform depotParent;
    public Transform bagParent;
    public GameObject itemSlotPrefab;

    public List<ItemData> depotItems;

    private Dictionary<string, InventorySlot> bagSlots = new();

    void Awake()
    {
        Instance = this;
        InitializeDepot();
    }

    void InitializeDepot()
    {
        foreach (var item in depotItems)
        {
            GameObject obj = Instantiate(itemSlotPrefab, depotParent);
            var slot = obj.GetComponent<InventorySlot>();
            slot.isDepot = true;
            slot.SetItem(item);
        }
    }

    public void AddToBag(ItemData item)
    {
        if (bagSlots.TryGetValue(item.itemName, out InventorySlot slot))
        {
            slot.count++;
            slot.UpdateUI();
        }
        else
        {
            GameObject obj = Instantiate(itemSlotPrefab, bagParent);
            var newSlot = obj.GetComponent<InventorySlot>();
            newSlot.SetItem(item, 1);
            bagSlots[item.itemName] = newSlot;
        }
    }

    public void RemoveFromBag(ItemData item)
    {
        if (bagSlots.TryGetValue(item.itemName, out InventorySlot slot))
        {
            slot.count--;
            if (slot.count <= 0)
            {
                bagSlots.Remove(item.itemName);
                Destroy(slot.gameObject);
            }
            else
            {
                slot.UpdateUI();
            }
        }
    }
}
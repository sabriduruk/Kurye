using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    
    public List<ItemData> mainDishes = new();
    public List<ItemData> sideDishes = new();
    public List<ItemData> drinks = new();
    
    public Transform mainDishParent;
    public Transform sideDishParent;
    public Transform drinkParent;



    public Transform depotParent;
    public Transform bagParent;
    public GameObject itemSlotPrefab;

    public List<ItemData> depotItems;

    public Dictionary<string, InventorySlot> bagSlots = new();

    void Awake()
    {
        Instance = this;
        InitializeDepot();
    }

    void InitializeDepot()
    {
        foreach (var item in depotItems)
        {
            GameObject obj = Instantiate(itemSlotPrefab); // paneli sonra belirteceğiz
            var slot = obj.GetComponent<InventorySlot>();
            slot.isDepot = true;
            slot.SetItem(item);

            switch (item.category)
            {
                case ItemCategory.MainDish:
                    obj.transform.SetParent(mainDishParent, false);
                    break;
                case ItemCategory.SideDish:
                    obj.transform.SetParent(sideDishParent, false);
                    break;
                case ItemCategory.Drink:
                    obj.transform.SetParent(drinkParent, false);
                    break;
            }
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
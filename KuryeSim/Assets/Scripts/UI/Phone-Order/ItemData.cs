using UnityEngine;

public enum ItemCategory
{
    MainDish,   // Ana Yemek
    SideDish,   // Ara Yemek
    Drink       // İçecek
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemCategory category; // 👈 yeni alan
}
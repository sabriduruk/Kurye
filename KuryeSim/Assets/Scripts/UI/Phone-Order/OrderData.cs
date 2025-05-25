using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OrderData
{
    public string orderName;
    public List<OrderItem> items = new();
    public int reward;
    public int duration;
    public string deliveryTime;
}

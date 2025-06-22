using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OrderData
{
    public string orderName;
    public List<OrderItem> items = new();
    public float reward;
    public float duration;
    public string deliveryTime;
    public int distance;
}

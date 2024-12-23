using System.Collections.Generic;
using ScriptableObject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour{
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private Vector2 offset;
    [SerializeField] private Vector2 padding;
    [SerializeField] private int column;
    
    public void Init(ItemDataList allItemDataList, List<int> itemAmountList)
    {
        for (var i = 0; i < allItemDataList.list.Count; i++)
        {
            var itemData = allItemDataList.list[i];
            var pos = new Vector3((i % column) * padding.x, -(i / column) * padding.y, 0) + new Vector3(offset.x, offset.y, 0);
            var inventoryItem = Instantiate(inventoryItemPrefab, pos, Quaternion.identity, content).GetComponent<InventoryItem>();
            inventoryItem.Init(itemData, itemAmountList[i]);
        }
    }
    
    public void UpdateAmount(int id, int amount)
    {
        content.GetChild(id).GetComponent<InventoryItem>().UpdateAmount(amount);
    }
}

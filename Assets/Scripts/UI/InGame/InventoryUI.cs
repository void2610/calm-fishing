using System.Collections.Generic;
using Fishing;
using ScriptableObject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour{
    [Header("インベントリ")]
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private Vector2 offset;
    [SerializeField] private Vector2 padding;
    [SerializeField] private int column;
    [Header("詳細パネル")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image iconImage;
    
    public void Init(ItemDataList allItemDataList, List<int> itemAmountList)
    {
        for (var i = 0; i < allItemDataList.list.Count; i++)
        {
            var itemData = allItemDataList.list[i];
            var pos = new Vector3((i % column) * padding.x, -(i / column) * padding.y, 0) + new Vector3(offset.x, offset.y, 0);
            var inventoryItem = Instantiate(inventoryItemPrefab, pos, Quaternion.identity, content);
            inventoryItem.GetComponent<InventoryItem>().Init(itemData, itemAmountList[i]);
            Utils.AddEventToObject(inventoryItem, () => ShowDetail(itemData), UnityEngine.EventSystems.EventTriggerType.PointerEnter);
        }
        
        ShowDetail(allItemDataList.list[0]);
    }
    
    public void UpdateAmount(int id, int amount)
    {
        content.GetChild(id).GetComponent<InventoryItem>().UpdateAmount(amount);
    }
    
    private void ShowDetail(ItemData itemData)
    {
        if(InventoryManager.Instance.GetItemAmount(itemData.id) > 0)
        {
            nameText.text = itemData.displayName;
            descriptionText.text = itemData.description;
            iconImage.sprite = itemData.sprite;
            iconImage.color = new Color(1, 1, 1, 1);
        }
        else
        {
            nameText.text = "???";
            descriptionText.text = "???";
            iconImage.sprite = null;
            iconImage.color = new Color(0, 0, 0, 0);
        }
    }
}

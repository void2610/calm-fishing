using System.Collections.Generic;
using UnityEngine;
using ScriptableObject;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] private ItemDataList allItemDataList;
    
    private List<int> _itemAmountList;

    public void AddRandomItem()
    {
        var itemData = allItemDataList.GetRandomItemData();
        Debug.Log($"アイテムを追加しました: {itemData.displayName}");
    }
    
    private void Save()
    {
        for (var i = 0; i < allItemDataList.list.Count; i++)
        {
            PlayerPrefs.SetInt($"item{i}", _itemAmountList[i]);
        }
    }
    
    private List<int> Load()
    {
        var list = new List<int>();
        for (var i = 0; i < allItemDataList.list.Count; i++)
        {
            list.Add(PlayerPrefs.GetInt($"item{i}", 0));
        }
        return list;
    }
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        
        allItemDataList.Register();
        _itemAmountList = Load();
    }
}

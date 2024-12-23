using System.Collections.Generic;
using ScriptableObject;
using UnityEngine;

namespace Fishing
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        [SerializeField] private ItemDataList allItemDataList;
        [SerializeField] private InventoryUI inventoryUI;
        
        private readonly List<int> _itemAmountList = new ();

        public void AddRandomItem()
        {
            var itemData = allItemDataList.GetRandomItemData();
            _itemAmountList[itemData.id]++;
            inventoryUI.UpdateAmount(itemData.id, _itemAmountList[itemData.id]);
            Debug.Log($"アイテムを追加しました: {itemData.displayName}");

            Save();
        }
    
        private void Save()
        {
            for (var i = 0; i < allItemDataList.list.Count; i++)
            {
                PlayerPrefs.SetInt($"item{i}", _itemAmountList[i]);
            }
        }
    
        private void Load()
        {
            _itemAmountList.Clear();
            for (var i = 0; i < allItemDataList.list.Count; i++)
            {
                _itemAmountList.Add(PlayerPrefs.GetInt($"item{i}", 0));
            }
        }
    
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        
            allItemDataList.Register();
            Load();
            inventoryUI.Init(allItemDataList, _itemAmountList);
        }
    
        private void OnDestroy()
        {
            Save();
        }
    }
}

using System.Collections.Generic;
using ScriptableObject;
using UnityEngine;
using unityroom.Api;

namespace Fishing
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        [SerializeField] private ItemDataList allItemDataList;
        [SerializeField] private InventoryUI inventoryUI;
        
        private readonly List<int> _itemAmountList = new ();
        private int _score = 0;

        public void GetRandomItem()
        {
            var itemData = allItemDataList.GetRandomItemData();
            _itemAmountList[itemData.id]++;
            inventoryUI.UpdateAmount(itemData.id, _itemAmountList[itemData.id]);
            
            _score += itemData.score;
            PlayerPrefs.SetInt("score", _score);
            UnityroomApiClient.Instance.SendScore(1, _score, ScoreboardWriteMode.HighScoreDesc);

            Save();
        }
        
        public int GetItemAmount(int id)
        {
            return _itemAmountList[id];
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
            _score = PlayerPrefs.GetInt("score", 0);
            PlayerPrefs.SetInt("score", _score);
        }
    
        private void OnDestroy()
        {
            Save();
        }
    }
}

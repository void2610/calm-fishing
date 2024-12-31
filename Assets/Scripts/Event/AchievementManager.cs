using System;
using System.Collections.Generic;
using Fishing;
using ScriptableObject;
using UnityEngine;

namespace Event
{
    public class AchievementManager : MonoBehaviour
    {
        public static AchievementManager Instance { get; private set; }
        
        [SerializeField] private AchievementDataList allAchievementDataList;
        [SerializeField] private AchievementUI achievementUI;
        [SerializeField] private AchievementNotice achievementNotice;

        
        private readonly List<bool> _isUnlockedList = new ();
        private readonly Dictionary<GameEventType, int> _callCountDictionary = new ();
        
        public bool IsUnlocked(int id) => _isUnlockedList[id];
        
        private void Save()
        {
            for (var i = 0; i < allAchievementDataList.list.Count; i++)
            {
                PlayerPrefs.SetInt($"achievement{i}", _isUnlockedList[i] ? 1 : 0);
            }
        }
    
        private void Load()
        {
            _isUnlockedList.Clear();
            for (var i = 0; i < allAchievementDataList.list.Count; i++)
            {
                _isUnlockedList.Add(PlayerPrefs.GetInt($"achievement{i}", 0) == 1);
            }
        }
        
        private void UnlockAchievement(int id)
        {
            if (_isUnlockedList[id]) return;
            
            _isUnlockedList[id] = true;
            achievementUI.UnLockAchievement(id);
            achievementNotice.Notice(allAchievementDataList.list[id]);
            Save();
        }
        
        private void CheckAchievement()
        {
            foreach (var achievement in allAchievementDataList.list)
            {
                var isUnlocked = new List<bool>();
                foreach (var condition in achievement.conditions)
                {
                    switch (condition.conditionType)
                    {
                        case ConditionType.CallCount:
                            isUnlocked.Add(_callCountDictionary[condition.eventType] >= condition.requiredCallCount);
                            break;
                        case ConditionType.InventoryCount:
                            isUnlocked.Add(InventoryManager.Instance.GetItemAmount(condition.requiredItem.id) >= condition.requiredValue);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                
                if (isUnlocked.TrueForAll(x => x))
                {
                    UnlockAchievement(achievement.id);
                }
            }
        }
        
        private void OnGameEvent(GameEventType eventType)
        {
            _callCountDictionary[eventType]++;
            CheckAchievement();
        }

        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            
            allAchievementDataList.Register();
            Load();
            Save();
            achievementUI.Init(allAchievementDataList, _isUnlockedList);
            
            // イベントの監視
            foreach(var eventType in Enum.GetValues(typeof(GameEventType)))
            {
                _callCountDictionary.Add((GameEventType)eventType, 0);
                EventManager.EventDictionary[(GameEventType)eventType].Subscribe(_ => OnGameEvent((GameEventType)eventType));
            }
        }


    }
}

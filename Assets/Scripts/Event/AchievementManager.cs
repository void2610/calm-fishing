using System;
using System.Collections.Generic;
using Fishing;
using ScriptableObject;
using UnityEngine;

namespace Event
{
    public class AchievementManager : MonoBehaviour
    {
        [SerializeField] private AchievementDataList allAchievementDataList;
        
        private readonly List<bool> _isUnlockedList = new ();
        private readonly Dictionary<GameEventType, int> _callCountDictionary = new ();
        
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
            Debug.Log($"Achievement Unlocked: {allAchievementDataList.list[id].title}");
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
            _callCountDictionary.TryAdd(eventType, 0);
            _callCountDictionary[eventType]++;
            CheckAchievement();
        }

        private void Awake()
        {
            allAchievementDataList.Register();
            Load();
            Save();
            
            // イベントの監視
            foreach(var eventType in Enum.GetValues(typeof(GameEventType)))
            {
                EventManager.EventDictionary[(GameEventType)eventType].Subscribe(_ => OnGameEvent((GameEventType)eventType));
            }
        }
    }
}

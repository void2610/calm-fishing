using System;
using System.Collections.Generic;
using Fishing;
using ScriptableObject;
using UnityEngine;
using unityroom.Api;

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
            
            foreach (var count in _callCountDictionary)
            {
                PlayerPrefs.SetInt($"callCount{count.Key.ToString()}", count.Value);
            }
        }
    
        private void Load()
        {
            _isUnlockedList.Clear();
            for (var i = 0; i < allAchievementDataList.list.Count; i++)
            {
                _isUnlockedList.Add(PlayerPrefs.GetInt($"achievement{i}", 0) == 1);
            }

            for(var i = 0; i < Enum.GetValues(typeof(GameEventType)).Length; i++)
            {
                var eventType = (GameEventType)i;
                _callCountDictionary[eventType] = PlayerPrefs.GetInt($"callCount{eventType.ToString()}", 0);
            }
        }
        
        private void UnlockAchievement(int id)
        {
            if (_isUnlockedList[id]) return;
            
            _isUnlockedList[id] = true;
            achievementUI.UnLockAchievement(id);
            achievementNotice.Notice(allAchievementDataList.list[id]).Forget();
            
            // 解除数を送信
            var unlockCount = _isUnlockedList.FindAll(x => x).Count;
            UnityroomApiClient.Instance.SendScore(2, unlockCount, ScoreboardWriteMode.HighScoreDesc);
        }
        
        private void OnGameEvent(GameEventType eventType)
        {
            _callCountDictionary[eventType]++;
            
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
            
            Save();
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
            
            // イベントの監視
            foreach(var eventType in Enum.GetValues(typeof(GameEventType)))
            {
                _callCountDictionary.Add((GameEventType)eventType, 0);
                EventManager.EventDictionary[(GameEventType)eventType].Subscribe(_ => OnGameEvent((GameEventType)eventType));
            }
            
            Load();
            Save();
            achievementUI.Init(allAchievementDataList, _isUnlockedList);
        }


    }
}

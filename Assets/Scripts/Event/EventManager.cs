using System;
using System.Collections.Generic;
using ScriptableObject;
using UnityEngine;

namespace Event
{
    public enum GameEventType
    {
        OnGameStart, 
        OnItemGet,
        OnRemoveCloud,
        OnTimePeriodChanged,
        OnWeatherChanged,
    }
    
    public static class EventManager
    {
        // ゲーム開始時: なし
        public static readonly GameEvent<int> OnGameStart = new (0);
        // アイテム取得時: 取得したアイテムのデータ
        public static readonly GameEvent<ItemData> OnItemGet = new (null);
        // 雲を取り除いた時: なし
        public static readonly GameEvent<int> OnRemoveCloud = new (0);
        // 時間帯変化時: 変化後の時間帯
        public static readonly GameEvent<TimePeriod> OnTimePeriodChanged = new (TimePeriod.Dusk);
        // 天候変化時: 変化後の天候
        public static readonly GameEvent<Weather> OnWeatherChanged = new (Weather.Sunny);
        
        public static readonly Dictionary<GameEventType, GameEventBase> EventDictionary = new ()
        {
            {GameEventType.OnGameStart, OnGameStart},
            {GameEventType.OnItemGet, OnItemGet},
            {GameEventType.OnRemoveCloud, OnRemoveCloud},
            {GameEventType.OnTimePeriodChanged, OnTimePeriodChanged},
            {GameEventType.OnWeatherChanged, OnWeatherChanged},
        };
    
        // ゲーム開始時にイベントをリセット
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetEventManager()
        {
            foreach(var p in typeof(EventManager).GetFields())
            {
                if(p.FieldType == typeof(GameEvent<int>))
                {
                    var e = (GameEvent<int>)p.GetValue(null);
                    e.ResetAll();
                }
                else if(p.FieldType == typeof(GameEvent<float>))
                {
                    var e = (GameEvent<float>)p.GetValue(null);
                    e.ResetAll();
                }
                else if(p.FieldType == typeof(GameEvent<bool>))
                {
                    var e = (GameEvent<bool>)p.GetValue(null);
                    e.ResetAll();
                }
            }
        }
    }
}
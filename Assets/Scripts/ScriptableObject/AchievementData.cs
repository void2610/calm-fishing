using System.Collections.Generic;
using Event;
using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "AchievementData", menuName = "Scriptable Objects/AchievementData")]
    public class AchievementData : UnityEngine.ScriptableObject
    {
        public int id;
        public string title;
        public Sprite icon;
        public string description;
        public bool isConditionHidden; // 条件を非表示にするか
        public List<AchievementCondition> conditions; // リスト内の全てを満たすと解除
    }
    
    [System.Serializable]
    public class AchievementCondition
    {
        public ConditionType conditionType; // 条件の種類
        public GameEventType eventType; // イベントの種類 (呼び出し回数比較の場合のみ)
        public int requiredCallCount; // 必要な呼び出し回数 (呼び出し回数比較の場合のみ)
        public ItemData requiredItem; // 必要なアイテム (インベントリ比較の場合のみ)
        public int requiredValue; // 必要なアイテム数 (インベントリ比較の場合のみ)
    }

    public enum ConditionType
    {
        CallCount,         // 呼び出し回数の比較
        InventoryCount,    // インベントリ内のアイテム数の比較
    }
}
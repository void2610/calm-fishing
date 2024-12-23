using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
    public class ItemData : UnityEngine.ScriptableObject
    {
        public int id;
        public Sprite sprite;
        public string displayName;
        public string description;
        public Rarity rarity;
        public int price;
    }
}
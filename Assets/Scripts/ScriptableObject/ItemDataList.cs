using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "ItemDataList", menuName = "Scriptable Objects/ItemDataList")]
    public class ItemDataList : UnityEngine.ScriptableObject
    {
        [FormerlySerializedAs("itemDataList")] [SerializeField] 
        public List<ItemData> list = new ();

        private float _totalWeight;

        public List<ItemData> GetItemDataFromRarity(Rarity r)
        {
            return list.Where(bd => bd.rarity == r).ToList();
        }
        
        public ItemData GetRandomItemData()
        {
            var value = Random.Range(0, _totalWeight);
            
            foreach (var itemData in list)
            {
                value -= itemData.probability;
                if (value <= 0)
                {
                    return itemData;
                }
            }
            return list[0];
        }

        public void Register()
        {
#if UNITY_EDITOR
            // ScriptableObject (このスクリプト) と同じディレクトリパスを取得
            var path = UnityEditor.AssetDatabase.GetAssetPath(this);
            path = System.IO.Path.GetDirectoryName(path);

            // 指定ディレクトリ内の全てのItemDataを検索
            var guids = UnityEditor.AssetDatabase.FindAssets("t:ItemData", new[] { path });

            // 検索結果をリストに追加
            list.Clear();
            var id = 0;
            foreach (var guid in guids)
            {
                var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var itemData = UnityEditor.AssetDatabase.LoadAssetAtPath<ItemData>(assetPath);
                if (itemData != null)
                {
                    list.Add(itemData);
                    itemData.id = id++;
                }
            }

            UnityEditor.EditorUtility.SetDirty(this); // ScriptableObjectを更新
#endif
            
            _totalWeight = list.Sum(itemData => itemData.probability);
        }
    }
}
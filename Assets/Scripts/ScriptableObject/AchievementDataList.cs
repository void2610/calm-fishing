using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "AchievementDataList", menuName = "Scriptable Objects/AchievementDataList")]
    public class AchievementDataList : UnityEngine.ScriptableObject
    {
        [FormerlySerializedAs("achievementDataList")] [SerializeField] 
        public List<AchievementData> list = new ();

        public void Register()
        {
#if UNITY_EDITOR
            // ScriptableObject (このスクリプト) と同じディレクトリパスを取得
            var path = UnityEditor.AssetDatabase.GetAssetPath(this);
            path = System.IO.Path.GetDirectoryName(path);

            // 指定ディレクトリ内の全てのAchievementDataを検索
            var guids = UnityEditor.AssetDatabase.FindAssets("t:AchievementData", new[] { path });

            // 検索結果をリストに追加
            list.Clear();
            var ids = new List<int>();
            foreach (var guid in guids)
            {
                var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var achievementData = UnityEditor.AssetDatabase.LoadAssetAtPath<AchievementData>(assetPath);
                if (achievementData != null)
                {
                    list.Add(achievementData);
                    if(ids.Contains(achievementData.id))
                        Debug.LogError($"Duplicate ID: {achievementData.title}");
                    ids.Add(achievementData.id);
                }
            }
            
            //idでソート
            list = list.OrderBy(x => x.id).ToList();
            // id=0を最初に移動
            var zeroIndex = list.FindIndex(x => x.id == 0);
            if (zeroIndex != -1)
            {
                var zero = list[zeroIndex];
                list.RemoveAt(zeroIndex);
                list.Insert(0, zero);
            }

            UnityEditor.EditorUtility.SetDirty(this); // ScriptableObjectを更新
#endif
        }
    }
}
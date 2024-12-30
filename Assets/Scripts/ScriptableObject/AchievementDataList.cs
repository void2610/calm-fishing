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
            var id = 0;
            foreach (var guid in guids)
            {
                var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var achievementData = UnityEditor.AssetDatabase.LoadAssetAtPath<AchievementData>(assetPath);
                if (achievementData != null)
                {
                    list.Add(achievementData);
                    achievementData.id = id++;
                }
            }

            UnityEditor.EditorUtility.SetDirty(this); // ScriptableObjectを更新
#endif
        }
    }
}
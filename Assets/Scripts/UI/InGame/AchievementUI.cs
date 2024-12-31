using System.Collections.Generic;
using Event;
using Fishing;
using ScriptableObject;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour{
    [FormerlySerializedAs("achievementItemPrefab")]
    [Header("インベントリ")]
    [SerializeField] private GameObject achievementItemPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private Vector2 offset;
    [SerializeField] private Vector2 padding;
    [SerializeField] private int column;
    [Header("詳細パネル")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image iconImage;
    
    public void Init(AchievementDataList allAchievementDataList, List<bool> isUnlockedList)
    {
        for (var i = 0; i < allAchievementDataList.list.Count; i++)
        {
            var itemData = allAchievementDataList.list[i];
            var pos = new Vector2(
                (i % column) * padding.x + offset.x,
                -(i / column) * padding.y + offset.y
            );

            var achievementItem = Instantiate(achievementItemPrefab, content);
            var rectTransform = achievementItem.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = pos;
            }

            achievementItem.GetComponent<AchievementItem>().Init(itemData, isUnlockedList[i]);
            Utils.AddEventToObject(achievementItem, () => ShowDetail(itemData), UnityEngine.EventSystems.EventTriggerType.PointerEnter);
        }
        
        ShowDetail(allAchievementDataList.list[0]);
    }
    
    public void UnLockAchievement(int id)
    {
        if(!content) return;
        content.GetChild(id).GetComponent<AchievementItem>().Unlock();
    }
    
    private void ShowDetail(AchievementData achievementData)
    {
        if(AchievementManager.Instance.IsUnlocked(achievementData.id - 1))
        {
            nameText.text = achievementData.title;
            descriptionText.text = achievementData.description;
            iconImage.sprite = achievementData.icon;
            iconImage.color = new Color(1, 1, 1, 1);
        }
        else
        {
            nameText.text = "???";
            descriptionText.text = achievementData.isConditionHidden ? "???" : achievementData.description;
            iconImage.sprite = null;
            iconImage.color = new Color(0, 0, 0, 0);
        }
    }
}

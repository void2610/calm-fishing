using ScriptableObject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementItem : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite unknownSprite;
    
    private AchievementData _achievementData;
    
    public void Init(AchievementData achievementData, bool isUnlocked = false)
    {
        _achievementData = achievementData;
        image.sprite = isUnlocked ? achievementData.icon : unknownSprite;
    }
    
    public void Unlock()
    {
        image.sprite = _achievementData.icon;
    }
}

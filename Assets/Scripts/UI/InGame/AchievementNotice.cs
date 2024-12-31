using Cysharp.Threading.Tasks;
using DG.Tweening;
using ScriptableObject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementNotice : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private float showDuration = 0.5f;
    [SerializeField] private float waitDuration = 3f;
    [SerializeField] private float closeDuration = 1f;
    [SerializeField] private float rightMoveDistance = 100f;
    [SerializeField] private float upMoveDistance = 100f;

    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    
    public void Notice(AchievementData achievement)
    {
        NoticeAsync(achievement).Forget();
    }
    
    private async UniTaskVoid NoticeAsync(AchievementData achievement)
    {
        icon.sprite = achievement.icon;
        title.text = achievement.title;

        await _rectTransform.DOMoveX(rightMoveDistance, showDuration).SetEase(Ease.OutSine).SetRelative().ToUniTask();
        await UniTask.Delay((int)(waitDuration * 1000));
        _rectTransform.DOMoveY(upMoveDistance, closeDuration).SetEase(Ease.InSine).SetRelative();
        await _canvasGroup.DOFade(0, closeDuration).SetEase(Ease.InSine).ToUniTask();
        
        // Reset
        _rectTransform.DOMoveX(-rightMoveDistance, 0).SetRelative();
        _rectTransform.DOMoveY(-upMoveDistance, 0).SetRelative();
        _canvasGroup.alpha = 1;
    }

    private void Awake()
    {
        _canvasGroup = this.GetComponent<CanvasGroup>();
        _rectTransform = this.GetComponent<RectTransform>();
        
        _rectTransform.DOMoveX(-rightMoveDistance, 0).SetRelative();
    }
    
}
